using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{
    public bool m_StarPower = false;
    public bool m_ChangeModel = false;
    public float m_starPowerDuration = 1.0f;
    private float m_timeSinceStarPower = 0f;



    //public string m_CurrentModel = "small";
    public int m_PowerLevel = 0;
    public GameObject m_SmallModel;
    public GameObject m_BigModel;
    public GameObject m_FireModel;
    private GameObject[] m_ModelList;

    private GameObject m_PlayerModel;
    private Renderer m_PlayerRenderer = null;
    public Collider m_SmallCollider;
    public Collider m_BigCollider;
    public bool m_isCrouching = false;


    // Start is called before the first frame update
    void Start()
    {
        m_ModelList = new GameObject[]{m_SmallModel, m_BigModel, m_FireModel};
        //m_PlayerModel = m_SmallModel;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeModel();
        StarPower();
    }

    private void ChangeModel() {

        /*
        while (m_isCrouching)
            if (m_PowerLevel != 0)
                m_SmallCollider.enabled = true;
        */

        if (!m_ChangeModel)
            return;
        
        //Debug.Log("Mudando modelo");
        foreach (GameObject model in m_ModelList)
            model.SetActive(false);
        //Debug.Log("Crianças desativadas");

        if (m_PowerLevel == 0) {
            m_SmallCollider.enabled = true;
            m_BigCollider.enabled = false;
        } else {
            m_SmallCollider.enabled = false;
            m_BigCollider.enabled = true;
        }

        m_ModelList[m_PowerLevel].SetActive(true);
        DataManager.instance.setPowerLevel(m_PowerLevel);
        m_ChangeModel = false;

        /*
        switch (m_CurrentModel) {
            case "Big":
                Debug.Log("Cresceu");
                //m_BigModel.transform.position = m_PlayerModel.transform.position;                
                m_BigModel.SetActive(true);
                m_PlayerModel = m_BigModel;
                //m_PlayerRenderer = m_PlayerModel.GetComponent<Renderer>();
                break;
            case "Fire":
                //m_FireModel.transform.position = m_PlayerModel.transform.position;                
                m_FireModel.SetActive(true);
                m_PlayerModel = m_FireModel;
                //m_PlayerRenderer = m_PlayerModel.GetComponent<Renderer>();
                break;
            case "Star":
                if (m_PlayerModel.Equals(m_SmallModel)) {
                    //m_BigModel.transform.position = m_PlayerModel.transform.position;                
                    m_BigModel.SetActive(true);
                    m_PlayerModel = m_BigModel;
                    //m_PlayerRenderer = m_PlayerModel.GetComponent<Renderer>();
                }
                m_StarPower = true;
                break;
            default:
                //m_SmallModel.transform.position = m_PlayerModel.transform.position;                
                m_SmallModel.SetActive(true);
                m_PlayerModel = m_SmallModel;
                //m_PlayerRenderer = null;
                break;
        }
        */
        //Physics.SyncTransforms();
    }

    private void StarPower() {
        if (!m_StarPower)
            return;
        
        m_timeSinceStarPower += Time.deltaTime;

        foreach (Material m_Material in m_PlayerRenderer.materials) {
            //Color newColor = new Color(Random.value, Random.value, Random.value);
            //m_Material.color = newColor;
            m_Material.color = new Color(Random.value, Random.value, Random.value);
        }

        if (m_timeSinceStarPower >= m_starPowerDuration) {
            m_StarPower = false;
            foreach (Material m_Material in m_PlayerRenderer.materials)
                m_Material.color = Color.white;
            
            //m_PlayerRenderer.materials.Initialize();
        }
    }
}
