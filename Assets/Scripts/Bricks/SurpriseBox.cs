using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurpriseBox : MonoBehaviour
{
    private Bounds m_Bounds;
    public GameObject m_emptyBlock;
    // Start is called before the first frame update
    void Start()
    {
        m_Bounds = GetComponent<Collider>().bounds;
    }

    // Update is called once per frame

    private void OnCollisionEnter(Collision collision)
    {
        //if (!collision.gameObject.CompareTag("BigPlayer"))
        //    return;
        
        Vector3 impact = collision.contacts[0].point;
        bool isBelowBrick = (impact.y <= m_Bounds.min.y);

        if (!isBelowBrick)
            return;
        
        m_emptyBlock.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
