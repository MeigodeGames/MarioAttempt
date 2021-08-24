using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Goomba : MonoBehaviour
{
    [Header("Move")]
    public float m_MoveSpeed = 4.0f;
    public float m_RotationSpeed = 15.0f;
    public Vector3 m_Movement = Vector3.left;
    public float m_ImpulseForce = 10.0f;
    public int m_Score = 100;
    public bool m_Died = false;

    private Rigidbody m_Body;
    private Collider m_Collider;

    // Start is called before the first frame update
    void Start()
    {
        m_Body = GetComponent<Rigidbody>();
        // [64 | 32 | 16| 8 | 4 | 2 | 1]
        // [rotX, rotY, rotZ, posX, posY, posZ]
        m_Body.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        m_Body.MovePosition(m_Body.position + m_Movement * m_MoveSpeed * Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        if (m_Movement.sqrMagnitude > 0.001f)
        {
            var forwardRotation = Quaternion.Euler(0, -90, 0) * Quaternion.LookRotation(m_Movement);

            m_Body.MoveRotation(Quaternion.Slerp(m_Body.rotation, forwardRotation, m_RotationSpeed * Time.fixedDeltaTime));
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (m_Died)
            return;
        if (collision.gameObject.CompareTag("Ground"))
            return;
        
        if (collision.gameObject.tag.Contains("Player")) {
            Destroy(collision.gameObject); // Dano no player
        }

        m_Movement.x *= -1;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Contains("Player")) {
            m_Died = true;
            Rigidbody body = other.GetComponent<Rigidbody>();

            Vector3 velocity = body.velocity;
            velocity.y = 0.0f;

            body.velocity = velocity;
            other.GetComponent<Rigidbody>().AddForce(Vector3.up * m_ImpulseForce, ForceMode.Impulse);

            //ScoreUI score = GameObject.FindObjectOfType(typeof(ScoreUI)) as ScoreUI;

            DataManager.instance.addScore(m_Score);

            Destroy(gameObject);
        }
    }
}
