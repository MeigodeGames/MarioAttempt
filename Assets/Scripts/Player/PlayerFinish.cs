using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFinish : MonoBehaviour
{
    [Header("Triggers")]
    [SerializeField] private bool EndSequence = false;
    [SerializeField] private bool PlayerArrived = false;
    [SerializeField] private bool PlayerRotated = false;
    [SerializeField] private bool FlagArrived = false;
    [SerializeField] private bool PlayerJumped = false;
    [SerializeField] private bool PlayerDroped = false;
    [SerializeField] private bool PlayerDroped2 = false;
    public AudioClip SpinJumpSound;
    private AudioSource m_Sounds;
    
    
    [SerializeField] private LevelFinish m_FlagScript;
    [SerializeField] private float m_DescentSpeed = 2.5f;
    [SerializeField] private float m_MoveSpeed = 5.0f;
    [SerializeField] private Vector3 m_EndPositionPole;
    [SerializeField] private GameObject m_FlagPole;
    private Rigidbody m_Body;
    public Transform m_EndPositionCastle;
    

    // Start is called before the first frame update
    void Start()
    {
        m_Body = GetComponentInChildren<Rigidbody>();
        //m_Body = GetComponent<Rigidbody>();
        m_Sounds = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerEnding();
        //PlayerEndingPoled();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Finish") && !EndSequence) {
            //Debug.Log("Entrou collider");
            m_FlagPole = other.gameObject;
            m_FlagScript = m_FlagPole.GetComponent<LevelFinish>();
            m_Body.gameObject.GetComponent<PlayerMovement>().enabled = false;
            m_Body.velocity = new Vector3(0,0,0);
            m_Body.useGravity = false;
            m_Body.transform.position = new Vector3(Mathf.Round(m_Body.transform.position.x), m_Body.transform.position.y, m_Body.transform.position.z);
            Physics.SyncTransforms();
            EndSequence = true;
            m_EndPositionPole = this.transform.position;
            m_EndPositionPole.y = other.bounds.min.y;
        }
        

        /* For pole dance
        if (other.CompareTag("Finish") && !EndSequence) {
            //Debug.Log("Entrou collider");
            m_FlagPole = other.gameObject;
            m_FlagScript = m_FlagPole.GetComponent<LevelFinish>();
            //m_FlagScript = other.gameObject.GetComponent<LevelFinish>();
            m_Body.gameObject.GetComponent<PlayerMovement>().enabled = false;
            m_Body.velocity = new Vector3(0,0,0);
            Physics.gravity /= 50;
            //m_Body.useGravity = false;
            //Debug.Log("Pos x: " + m_Body.transform.position.x);
            //Debug.Log("Pos x rounded before: " + Mathf.Round(m_Body.transform.position.x));
            m_Body.transform.position = new Vector3(Mathf.Round(m_Body.transform.position.x), m_Body.transform.position.y, m_Body.transform.position.z);
            Physics.SyncTransforms();
            //Debug.Log("Pos x after: " + m_Body.transform.position.x);
            EndSequence = true;
            m_EndPositionPole = m_Body.transform.position;
            m_EndPositionPole.y = other.bounds.min.y;
            other.GetComponent<AudioSource>().enabled = false;
        }
        */
    }

    private void PlayerEnding() {

        PlayerDescent();
        
        PlayerRotateAround(m_FlagPole);

        PlayerToCastle();

        PlayerJump();

        nextLevel();
    }

    private void PlayerEndingPoled() {

        PoleDance(m_FlagPole);

        PlayerToCastle();

        PlayerJump();

        nextLevel();
    }

    private void PlayerDescent() {
        if (!EndSequence || FlagArrived)
            return;
            
        m_Body.transform.position = Vector3.MoveTowards (m_Body.transform.position, m_EndPositionPole, m_DescentSpeed*Time.fixedDeltaTime);

        if (Vector3.Distance(m_Body.transform.position, m_EndPositionPole) < 0.01f)
        {
            FlagArrived = m_FlagScript.FlagArrived;
            if (FlagArrived) {
                m_EndPositionPole.x += m_Body.GetComponent<Renderer>().bounds.size.x;
            }
        }
    }

    private void PlayerRotateAround(GameObject axisObject) {
        if (!FlagArrived || PlayerRotated)
            return;

        m_Body.transform.RotateAround(axisObject.transform.position, Vector3.up, 60 * Time.fixedDeltaTime);
        
        /*
        float dist_V = Vector3.Distance(m_Body.transform.position, m_EndPositionPole);
        bool result_V = (dist_V < 0.2f);

        if (result_V)
        */
        if (Vector3.Distance(m_Body.transform.position, m_EndPositionPole) < 0.2f)
        {
            PlayerRotated = true;
            m_Body.useGravity = true;
            m_Body.transform.localRotation *= Quaternion.Euler(0,180,0);
        }
        
    }
    private void PlayerToCastle() {
        if (!PlayerRotated || PlayerArrived)
            return;

        m_Body.transform.position = Vector3.MoveTowards (m_Body.transform.position, m_EndPositionCastle.position, m_MoveSpeed*Time.fixedDeltaTime);

        if (Vector3.Distance(m_Body.transform.position, m_EndPositionCastle.position) < 0.01f) {
            PlayerArrived = true;
        }
    }

    private void PlayerJump() {
        if(!PlayerArrived || PlayerJumped)
            return;

        if(!PlayerJumped)
        {
            m_Body.AddForce(Vector3.up * 460 * Time.fixedDeltaTime, ForceMode.VelocityChange);
            m_Body.AddRelativeTorque(Vector3.up * 230 * Time.fixedDeltaTime*50, ForceMode.Force);
        }

        if (Vector3.Distance(m_Body.transform.position, m_EndPositionCastle.position) > 0.2f)
        {
            m_Sounds.PlayOneShot(SpinJumpSound);
            PlayerJumped = true;
        }
    }
  
    private void nextLevel()
    {
        if(!PlayerJumped)
            return;
        
        if (Vector3.Distance(m_Body.transform.position, m_EndPositionCastle.position) < 0.01f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void PoleDance(GameObject axisObject)
    {
        if (!EndSequence || PlayerRotated)
            return;
        
        m_Body.transform.RotateAround(axisObject.transform.position, Vector3.up, 75 * Time.fixedDeltaTime);

        if(!PlayerDroped && (m_Body.transform.position.y < 1f)) {
            m_Body.AddForce(Vector3.up * 230 * Time.fixedDeltaTime, ForceMode.Impulse);
            PlayerDroped = true;
        }

        if(!PlayerDroped2 && (m_Body.transform.position.y < -1.5f)) {
            m_Body.AddForce(Vector3.up * 330 * Time.fixedDeltaTime, ForceMode.VelocityChange);
            PlayerDroped2 = true;
            m_EndPositionPole.x += m_Body.GetComponent<Renderer>().bounds.size.x;
        }

        if (PlayerDroped2 && (Vector3.Distance(m_Body.transform.position, m_EndPositionPole) < 0.2f))
        {
            PlayerRotated = true;
            Physics.gravity *= 50;
            m_Body.transform.localRotation *= Quaternion.Euler(0,180,0);
        }
    }
}
