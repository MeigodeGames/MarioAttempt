using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimBrick : MonoBehaviour
{
    private Bounds m_Bounds;
    private Animator m_Anim;
    public bool m_Breakable;

    // Start is called before the first frame update
    void Start()
    {
        m_Bounds = GetComponent<Collider>().bounds;
        m_Anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 impact = collision.contacts[0].point;
        bool isBelowBrick = (impact.y <= m_Bounds.min.y);

        if (!isBelowBrick)
            return;
        
        if (collision.gameObject.CompareTag("BigPlayer") && m_Breakable)
        {
            m_Anim.SetTrigger("Break");
        }
        else
            m_Anim.SetTrigger("Bounce");
    }

    private void f_Destroy()
    {
        Destroy(gameObject);
    }
}
