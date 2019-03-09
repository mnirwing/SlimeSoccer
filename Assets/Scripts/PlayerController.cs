using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{


    [SerializeField]
    private Transform groundCheckRight;
    [SerializeField]
    private Transform groundCheckLeft;
    [SerializeField]
    private Transform groundCheckMiddle;

    //[SerializeField]
    private float groundCheckRadius = 0.02f;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private bool onGround;
    [SerializeField]
    private bool jumpAllowed;

    [SerializeField]    //item is protected but visible in inspector
    private float speed;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float dashForce;

    private PlayerMotor motor;

    private int timer;

    private bool dashAllowed;
    private bool dash;

    [SerializeField]
    private int dashStamina;
    private int dashStaminaTemp;

    [SerializeField]
    private TrailRenderer trailRenderer;


    private Rigidbody2D rb;

    private ParticleSystem particles;

    [SerializeField]
    private string player1Or2;

    private bool dashLock;
    private bool jumpLock;
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        rb = GetComponent<Rigidbody2D>();

        groundCheckLeft = transform.Find("CheckGroundLeft");
        groundCheckMiddle = transform.Find("CheckGroundMiddle");
        groundCheckRight = transform.Find("CheckGroundRight");

        trailRenderer = transform.Find("Trail").gameObject.GetComponent<TrailRenderer>();

        

        particles = transform.Find("BlobParticle").gameObject.GetComponent<ParticleSystem>();
         
        dashAllowed = true;
        dashStaminaTemp = dashStamina;
        jumpAllowed = true;
    }

    private void Update()
    {
       
        //Calculate movement velocity as a 3d vector
        float _xMov = Input.GetAxisRaw("Horizontal Player " + player1Or2);

        //Apply movement
        motor.Move(_xMov * speed);

        if (!Input.GetButton("Jump Player " + player1Or2))
            jumpLock = false;

        //calculate jumpforce based on player input
        Vector3 _jumpForce = Vector3.zero;
        if (Input.GetButton("Jump Player " + player1Or2) && onGround && jumpAllowed && (Mathf.Abs(rb.velocity.y) < 0.1f) && !jumpLock)
        {
            _jumpForce = Vector3.up * jumpForce;
            jumpLock = true;
        }

        //apply jumpforce
        motor.ApplyJumpForce(_jumpForce);


        float _dashForce = 0f;
        if (dashStaminaTemp > 0)
        {
            dashAllowed = true;
        }
        else
        {
            dashAllowed = false;
            dashLock = true;
        }

        if (!Input.GetButton("Dash Player " + player1Or2))
            dashLock = false;

        if (Input.GetButton("Dash Player " + player1Or2) && dashAllowed && !dashLock)
        {
            _dashForce = dashForce;
            dash = true;
            particles.Play();

        }
        else
        {
            dash = false;
            particles.Stop();

        }

        motor.ApplyDashForce(_dashForce);


        

    }


    private void FixedUpdate()
    {
        onGround = Physics2D.OverlapCircle(groundCheckRight.position, groundCheckRadius, whatIsGround)
                || Physics2D.OverlapCircle(groundCheckLeft.position, groundCheckRadius, whatIsGround)
                || Physics2D.OverlapCircle(groundCheckMiddle.position, groundCheckRadius, whatIsGround);

        if (!dash)
        {
            if(dashStaminaTemp <= dashStamina)
            dashStaminaTemp +=4;
            trailRenderer.time = 0;

        }
        else
        {
            dashStaminaTemp -=20;
            trailRenderer.time = 3;

        }

    }



}
