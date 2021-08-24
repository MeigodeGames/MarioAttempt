using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBlock : MonoBehaviour
{
    public float m_BounceTime = 0.3f;
    public float m_BounceDistance = 0.3f;
    public AnimationCurve m_Curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f,1), new Keyframe(1,0));
    private Vector3 m_Source;
    private bool m_CanBounce;
    private float m_StartTime;
    private Bounds m_Bounds;

    // Start is called before the first frame update
    private void Start()
    {
        m_Source = transform.position;
        m_Bounds = GetComponent<Collider>().bounds;
    }

    // Update is called once per frame
    private void Update()
    {
        if(!m_CanBounce)
            return;
        
        float time = (Time.time - m_StartTime) / m_BounceTime;
        transform.position = m_Source + Vector3.up * m_BounceDistance * m_Curve.Evaluate(time);
        m_CanBounce = (time < 1.0f);

        // Disable block after first hit
        // if(!m_CanBounce) this.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Colisão");

        Vector3 impact = collision.contacts[0].point;
        bool isBelowBrick = (impact.y <= m_Bounds.min.y);

        if (!isBelowBrick)
            return;

        //if (impact.y <= m_Bounds.min.y)
        m_StartTime = Time.time;
        m_CanBounce = true;
    }
}
