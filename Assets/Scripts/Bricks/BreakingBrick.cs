using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingBrick : MonoBehaviour
{
    public GameObject m_Explosion;
    private Bounds m_Bounds;

    // Start is called before the first frame update
    private void Start()
    {
        m_Bounds = GetComponent<Collider>().bounds;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        //if (!collision.gameObject.CompareTag("BigPlayer"))
        if (!DataManager.instance.isBig())
            return;
        
        Vector3 impact = collision.contacts[0].point;
        bool isBelowBrick = (impact.y <= m_Bounds.min.y);

        if (!isBelowBrick)
            return;
        
        //if (impact.y <= m_Bounds.min.y)
        Instantiate(m_Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
