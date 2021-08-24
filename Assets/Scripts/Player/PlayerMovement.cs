using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Model")]
    private bool m_ChangeModel = false;


    [Header("Move")]
    public float m_MoveSpeed;
    public float m_RunSpeed;
    private bool m_IsRunning;
    public float m_RotationSpeed = 15.0f;
    private Vector3 m_Movement;

    [Header("Ground")]
    public float m_GroundDistance = 0.01f;
    public LayerMask m_GroundLayer;
    public Transform m_Feet;
    private bool m_IsGrounded;
    private Vector3 m_GroundDetector;

    [Header("Jump")]
    public float m_JumpForce = 230f;
    public float m_JumpTime = 0.33f;
    private float m_JumpElapsedTime;
    private bool m_IsJumping;
    private bool m_IsSpinJumping;

    [Header("Sounds")]
    public AudioClip ShortJumpSound;
    public AudioClip LongJumpSound;
    public AudioClip SpinJumpSound;
    private AudioSource m_Sounds;


    private Rigidbody m_Body;
    

    [Header("Animation")]
    public Animator m_anim;

    // Start is called before the first frame update
    void Start()
    {
        //m_PlayerModel = m_SmallModel;
        //m_SmallModel.SetActive(true);

        //m_Body = GetComponentInChildren<Rigidbody>();
        //m_Feet = m_Body.GetComponentInChildren<Transform>();
        //m_GroundDetector = m_Body.GetComponent<BoxCollider>().size/2;

        m_Body = GetComponent<Rigidbody>();
        //m_Feet = m_Body.GetComponentInChildren<Transform>();
        m_GroundDetector = GetComponent<BoxCollider>().size/2;

        m_GroundDetector.y = m_GroundDistance;
        m_Sounds = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //m_IsGrounded = Physics.CheckSphere(m_Feet.position, m_GroundDistance, m_GroundLayer, QueryTriggerInteraction.Ignore);
        m_IsGrounded = Physics.CheckBox(m_Feet.position, m_GroundDetector, Quaternion.Euler(0, 0, 0), m_GroundLayer, QueryTriggerInteraction.Ignore);
        m_Movement.x = Input.GetAxis("Horizontal");

        //m_IsRunning = Input.GetButton("Fire1"); // Press to run
        if (Input.GetButtonDown("Fire1")) m_IsRunning = !m_IsRunning; // Toggle to run

        if (Input.GetButtonDown("Jump") && m_IsGrounded)
        {
            m_IsJumping = true;
            m_JumpElapsedTime = 0;
        }

        //Moving
        m_anim.SetBool("Moving", Mathf.Abs(m_Movement.x) >0);
        m_anim.SetBool("IsGrounded", m_IsGrounded);
        m_anim.SetBool("Running", m_IsRunning && Mathf.Abs(m_Movement.x) >0);

        /*
        if (Input.GetButtonDown("Fire2") && m_IsGrounded)
            m_ChangeModel = true;
        
        
        if (Input.GetButtonDown("Fire2") && m_IsGrounded)
        {
            m_IsSpinJumping = true;
            m_JumpElapsedTime = 0;
        }
        */
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
        Jump();
        SpinJump();

        //ChangeModel();
    }

    private void Move()
    {
        float speed = m_IsRunning ? m_RunSpeed : m_MoveSpeed;
        m_Body.MovePosition(m_Body.position + m_Movement * speed * Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        if (m_Movement.sqrMagnitude > 0.001f)
        {
            var forwardRotation = Quaternion.Euler(0, -90, 0) * Quaternion.LookRotation(m_Movement);

            m_Body.MoveRotation(Quaternion.Slerp(m_Body.rotation, forwardRotation, m_RotationSpeed * Time.fixedDeltaTime));
        }
    }

    private void SpinJump()
    {
        if (m_IsSpinJumping && m_JumpElapsedTime > (m_JumpTime /3))
        {
            if (!Input.GetButton("Fire2"))
            {
                m_IsSpinJumping = false;
                m_Sounds.PlayOneShot(SpinJumpSound);
            }
        }

        if (m_IsSpinJumping && m_JumpElapsedTime < m_JumpTime)
        {
            m_JumpElapsedTime += Time.fixedDeltaTime;

            float proportionCompleted = Mathf.Clamp01(m_JumpElapsedTime / m_JumpTime);
            float currentForce = Mathf.Lerp(m_JumpForce, 0, proportionCompleted);

            m_Body.AddForce(Vector3.up * currentForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
            m_Body.AddRelativeTorque(Vector3.up * currentForce * Time.fixedDeltaTime*50, ForceMode.Force);
        }
        else
        {
            m_IsSpinJumping = false;
        }

        if(m_IsSpinJumping && m_JumpElapsedTime > m_JumpTime)
        {
            m_Sounds.PlayOneShot(SpinJumpSound);
        }
    }
    private void Jump()
    {
        if (m_IsJumping && m_JumpElapsedTime > (m_JumpTime /3))
        {
            if (!Input.GetButton("Jump"))
            {
                m_IsJumping = false;
                m_Sounds.PlayOneShot(ShortJumpSound);
            }
        }

        if (m_IsJumping && m_JumpElapsedTime < m_JumpTime)
        {
            m_JumpElapsedTime += Time.fixedDeltaTime;

            float proportionCompleted = Mathf.Clamp01(m_JumpElapsedTime / m_JumpTime);
            float currentForce = Mathf.Lerp(m_JumpForce, 0, proportionCompleted);

            m_Body.AddForce(Vector3.up * currentForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
        else
        {
            m_IsJumping = false;
        }

        if(m_IsJumping && m_JumpElapsedTime > m_JumpTime)
        {
            m_Sounds.PlayOneShot(LongJumpSound);
        }

    }

    private void ChangeModel() {
        if (!m_ChangeModel)
            return;

        m_Body = GetComponentInChildren<Rigidbody>();
        m_Feet = m_Body.GetComponentInChildren<Transform>();

        m_ChangeModel = false;
    }
}
