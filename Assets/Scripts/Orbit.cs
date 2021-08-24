using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject m_FlagPole;
    public float m_speed = 90;
    private Rigidbody m_Body;
    void Start()
    {
        m_Body = GetComponent<Rigidbody>();
        m_Body.gameObject.GetComponent<PlayerMovement>().enabled = false;
        m_Body.velocity = new Vector3(0,0,0);
        m_Body.useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        OrbitAround(m_FlagPole);
    }

    void OrbitAround(GameObject axis) {
        Debug.Log("Rotating " + m_Body.transform.position.ToString());
        m_Body.transform.RotateAround (axis.transform.position, Vector3.up, m_speed * Time.fixedDeltaTime);
        Debug.Log("Rotate around " + axis.transform.position.ToString());
    }
}
