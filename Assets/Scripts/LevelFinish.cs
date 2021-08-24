using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    [SerializeField] public bool FlagArrived = false;
    [SerializeField] private bool EndSequence = false;
    [SerializeField] private float m_DescentSpeed = 2.5f;
    [SerializeField] private Vector3 m_EndPosition;
    public GameObject m_Flag;
    private Renderer m_PoleRenderer;

    [Header("Sounds")]
    public AudioClip m_LevelEndSound;
    private AudioSource m_Sounds;

    // Start is called before the first frame update
    void Start()
    {
        m_PoleRenderer = GetComponent<Renderer>();
        m_Sounds = GetComponent<AudioSource>();
        m_EndPosition = m_Flag.transform.position;
        m_EndPosition.y = m_PoleRenderer.bounds.min.y;
    }

    void FixedUpdate()
    {
        //if (EndSequence)
            FlagDescent();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player"))
        if (other.tag.Contains("Player"))
        {
            EndSequence = true;
            m_Sounds.PlayOneShot(m_LevelEndSound);
        }
    }

    private void FlagDescent()
    {
        if (FlagArrived || !EndSequence)
            return;
        
        //if (!FlagArrived && EndSequence)
        //{
        m_Flag.transform.position = Vector3.MoveTowards (m_Flag.transform.position, m_EndPosition, m_DescentSpeed*Time.fixedDeltaTime);
        FlagArrived = (Vector3.Distance(m_Flag.transform.position, m_EndPosition) < 0.01f);
        /*    if (Vector3.Distance(m_Flag.transform.position, m_EndPosition) < 0.01f)
            {
                FlagArrived = true;
            }
        */
        //}
    }
    /*
    private IEnumerator FlagAnimation() {
        while (!FlagArrived) {
            m_Flag.transform.position = Vector3.MoveTowards (m_Flag.transform.position, m_EndPosition, 0.01f*Time.fixedDeltaTime);
            if (Vector3.Distance(m_Flag.transform.position, m_EndPosition) < 0.01f) {
                FlagArrived = true;

                yield break;
            }
            yield return null; 
        }
    }
    */
}
