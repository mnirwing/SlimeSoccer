using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor : MonoBehaviour {

    private float velocity = 0f;
    private Vector3 jumpForce = Vector3.zero;
    private float dashForce = 0f;
    public bool jumpAllowed;

    private AudioSource jumpSound;
    private Rigidbody2D rb;

    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpSound = GetComponent<AudioSource>();
    }

    //Gets a movement vector
    public void Move(float _velocity)
    {
        velocity = _velocity;
    }

    public void ApplyJumpForce(Vector3 _jumpForce)
    {
        jumpForce = _jumpForce;
    }

    public void ApplyDashForce(float _dashForce)
    {
        dashForce = _dashForce;
    }

    //run every physics iteration
    private void FixedUpdate()
    {
        PerformMovement();
    }


    //perform movement based on velocity variable
    private void PerformMovement()
    {


        if (velocity == 0f)
        {
            rb.velocity = new Vector3(0, rb.velocity.y);

        }
        else
        {
            //rb.MovePosition(new Vector2(rb.position.x + velocity, rb.position.y + rb.velocity.y));
            rb.velocity  = new Vector3(velocity, rb.velocity.y);
        }

        if((jumpForce != Vector3.zero))
            Debug.Log("JumpForce applied");

        if(jumpForce != Vector3.zero && jumpAllowed)
        {
            Debug.Log("Jump");

            rb.AddForce(jumpForce);
            jumpAllowed = false;
            jumpSound.Play();
            StartCoroutine("WaitForNextJump");

        }

        //if (jumpForce == Vector3.zero)
        //{
        //    jumpAllowed = true;
        //}

        if (dashForce != 0f ){

            
            rb.AddForce(new Vector3(rb.velocity.x  * dashForce, rb.velocity.y));

        }

        

    }

    IEnumerator WaitForNextJump()
    {
        yield return new WaitForSeconds(0.5f);
        jumpAllowed = true;
    }


}
