using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {


    [SerializeField]
    private Transform groundCheckRight;
    [SerializeField]
    private Transform groundCheckLeft;
    [SerializeField]
    private Transform groundCheckMiddle;

    private float groundCheckRadius = 0.02f;
    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private bool onGround;
    [SerializeField]
    private bool jumpAllowed;


    [SerializeField]
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

    private TrailRenderer trailRenderer;

    private AudioSource jumpSound;


    private Rigidbody2D rb;

    public Rigidbody2D rbEnemy;

    public Rigidbody2D rbBall;

    private ParticleSystem particles;

    public ZoneDetector zoneDetector;

    private float goalPosition;
    private float zonePosition;
    private bool ballIsFarFromPlayer;
    private float ballIsFarFromPlayerDistance;
    private bool ballIsFarFromEnemy;
    private float ballIsFarFromEnemyDistance;
    private bool playerIsFarFromEnemy;
    private float playerIsFarFromEnemyDistance;
    private bool ballIsRight;
    private bool playerIsRight;
    private bool enemyIsRight;

    public float distancePlayerBall;

    private float distanceEnemyBall;
    private float distancePlayerEnemy;

    private float counterBallOnGoal;

    private float movementSmoothing;

    public LayerMask mask;
    public LayerMask maskRebound;
    private RaycastHit2D hit;
    private RaycastHit2D reflectionHit;

    [SerializeField]
    private bool thisIsLeftPlayer;

    float _xMov;
    Vector3 _jumpForce;
    float _dashForce;

    private List<float> movementPriorities;

    private float offset;

    // Use this for initialization
    void Start () {
        motor = GetComponent<PlayerMotor>();
        jumpSound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        groundCheckLeft = transform.Find("CheckGroundLeft");
        groundCheckMiddle = transform.Find("CheckGroundMiddle");
        groundCheckRight = transform.Find("CheckGroundRight");

        trailRenderer = transform.Find("Trail").gameObject.GetComponent<TrailRenderer>();
        particles = transform.Find("BlobParticle").gameObject.GetComponent<ParticleSystem>();

        movementPriorities = new List<float>();

        dashAllowed = true;
        dashStaminaTemp = dashStamina;
        jumpAllowed = true;
        movementSmoothing = 0.3f;

        goalPosition = 8.5f;
        zonePosition = 5f;

        offset = 1f;

        playerIsFarFromEnemyDistance = 5f;
    }

    // Update is called once per frame
    void Update() {
        movementPriorities.Clear();

        _jumpForce = Vector3.zero;
        _xMov = 0f;
        _dashForce = 0f;

        if (!thisIsLeftPlayer)
        {
            if (rbBall.transform.position.x > 8
                && rbBall.transform.position.y > 3)
                counterBallOnGoal += Time.deltaTime;
            else
                counterBallOnGoal = 0;
        }
        if (thisIsLeftPlayer)
        {
            if (rbBall.transform.position.x < -8
                && rbBall.transform.position.y > 3)
                counterBallOnGoal += Time.deltaTime;
            else
                counterBallOnGoal = 0;
        }
        //calculate distances
        distanceEnemyBall = (rbEnemy.transform.position - rbBall.transform.position).magnitude;
        distancePlayerBall = (rb.transform.position - rbBall.transform.position).magnitude;
        distancePlayerEnemy = (rb.transform.position - rbEnemy.transform.position).magnitude;

        if (rbBall.position.x >= 0)
            ballIsRight = true;
        else
            ballIsRight = false;

        if (rb.transform.position.x >= 0)
            playerIsRight = true;
        else
            playerIsRight = false;

        if (rbEnemy.transform.position.x >= 0)
            enemyIsRight = true;
        else
            enemyIsRight = false;


        if (Mathf.Abs(distanceEnemyBall) > ballIsFarFromEnemyDistance)
            ballIsFarFromEnemy = true;
        else
            ballIsFarFromEnemy = false;

        if (Mathf.Abs(distancePlayerBall) > ballIsFarFromPlayerDistance)
            ballIsFarFromPlayer = true;
        else
            ballIsFarFromPlayer = false;

        if (Mathf.Abs(distancePlayerEnemy) > playerIsFarFromEnemyDistance)
            playerIsFarFromEnemy = true;
        else
            playerIsFarFromEnemy = false;


        hit = Physics2D.Raycast(rbBall.transform.position, rbBall.velocity.normalized, 100, mask);

        if (hit.collider != null)
        {
            //Debug.DrawRay(rbBall.transform.position, new Vector3(hit.point.x, hit.point.y, 0) - rbBall.transform.position, Color.red, 1);

            if ((hit.transform.tag == "Player2" && !thisIsLeftPlayer) || (hit.transform.tag == "Player1" && thisIsLeftPlayer))
            {
                if (distancePlayerBall < 3
                    && rbBall.transform.position.y > 1
                    && rbBall.transform.position.y < 4)
                {
                    Jump();
                }
            }

            if (Mathf.Abs(rbBall.transform.position.x) < Mathf.Abs(rb.transform.position.x) + 1.5f
                && Mathf.Abs(rbBall.transform.position.x) > Mathf.Abs(rb.transform.position.x) - 1.5f
                && rbBall.transform.position.y > 1.25f
                && rbBall.transform.position.y < 4
                && (!(Mathf.Abs(hit.point.x) > 3) && playerIsFarFromEnemy)
                && (!(Mathf.Abs(hit.point.x) < 7) && playerIsFarFromEnemy)
                && ((playerIsRight && ballIsRight) || (!playerIsRight && !ballIsRight)))
            {
                Jump();
            }


            //wenn der Ball richtung boden fliegt Priorität 0
            if (hit.transform.tag == "Ground" || hit.transform.tag == "CrossbarLeft" || hit.transform.tag == "CrossbarRight")
            {
                MoveTowards(hit.point, offset);
            }

            //wenn der Ball die Boundaries trifft Priorität 1
            else
            {
                if (hit.transform.tag == "Boundary")
                {
                    reflectionHit = Physics2D.Raycast(hit.point, new Vector2(rbBall.velocity.x * -1, rbBall.velocity.y), 100, maskRebound);
                }

                if (hit.transform.tag == "BoundaryTop")
                {
                    reflectionHit = Physics2D.Raycast(hit.point, new Vector2(rbBall.velocity.x, rbBall.velocity.y * -1).normalized, 100, maskRebound);
                }

                if (reflectionHit.collider != null)
                {
                    //Debug.DrawRay(hit.point, reflectionHit.point - hit.point, Color.blue, 1);
                    //wenn 2. Ray Boden oder Latte trifft
                    if (reflectionHit.transform.tag == "Ground" || reflectionHit.transform.tag == "CrossbarLeft" || reflectionHit.transform.tag == "CrossbarRight")
                {
                    MoveTowards(reflectionHit.point, offset);
                }
                }

            }

            ////wenn der Ball zum einem Tor fliegt
            //if ((Mathf.Abs(hit.point.x) > 6 && hit.point.y > 0 && hit.point.y < 8))
            //{
            //    GoToGoal();
            //}
        }
        else
            GoToGoal();

        //check if movement is safe
        if (!thisIsLeftPlayer)
        {
            if (playerIsRight && rbBall.velocity.x < -3f && !enemyIsRight)
            {
                GoToGoal();
            }
        }
        if (thisIsLeftPlayer)
        {
            if (!playerIsRight && rbBall.velocity.x > 3f && enemyIsRight)
            {
                GoToGoal();
            }
        }

        //wenn der Ball über dem Tor ist, werden die vorherigen Befehle überschrieben
        if (counterBallOnGoal > 1.5f)
        {
            GoOnTopOfGoal();
        }
        
        //wenn der spieler lange im tor steht
        if (zoneDetector.timer < 2.3f)
        {
            GoToGoal();
        }

        //dash
        if (dashStaminaTemp > 0)
        {
            dashAllowed = true;
        }
        else
        {
            dashAllowed = false;
        }

        //dasht wenn der Ball nah an dem Spieler ist und der Ball zum eigenen Tor fliegt
        if (distancePlayerBall < 1.5f && ((rbBall.velocity.x > 0 && !thisIsLeftPlayer) || (rbBall.velocity.x < 0 && thisIsLeftPlayer)))
        {
            if (dashAllowed)
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

        }
        else
        {
            dash = false;
            particles.Stop();
        }

        if (_jumpForce != Vector3.zero)
        {
        }

        if(movementPriorities.Count == 0)
        {
            movementPriorities.Add(0);
        }
        Debug.Log("Last movement: " + movementPriorities[movementPriorities.Count - 1]);
        motor.Move(movementPriorities[movementPriorities.Count - 1] * speed);
        motor.ApplyJumpForce(_jumpForce);
        motor.ApplyDashForce(_dashForce);


        

    }

    private void FixedUpdate()
    {
        onGround = Physics2D.OverlapCircle(groundCheckRight.position, groundCheckRadius, whatIsGround)
                || Physics2D.OverlapCircle(groundCheckLeft.position, groundCheckRadius, whatIsGround)
                || Physics2D.OverlapCircle(groundCheckMiddle.position, groundCheckRadius, whatIsGround);

        if (!dash)
        {
            if (dashStaminaTemp <= dashStamina)
                dashStaminaTemp += 2;
            trailRenderer.time = 0;

        }
        else
        {
            dashStaminaTemp -= 20;
            trailRenderer.time = 3;

        }

    }

    //----------------------------------------------------------------------------------------------------------------------------------------------
    private void MoveTowards(Vector2 target)
    {
        if (rb.transform.position.x < target.x - movementSmoothing)
            movementPriorities.Add(1);
        else if (rb.transform.position.x > target.x + movementSmoothing)
            movementPriorities.Add(-1);
        else
        {
            movementPriorities.Add(0);
            Debug.Log("Added 0 which should not happen. Target.x = " + target.x);
        }
    }

    //offset is towards own goal
    private void MoveTowards(Vector2 target, float offset)
    {
        if (thisIsLeftPlayer)
        {
            if (rb.transform.position.x < target.x - offset)
                movementPriorities.Add(1);
            else if (rb.transform.position.x > target.x - offset)
                movementPriorities.Add(-1);

            //if (rb.transform.position.x > target.x - offset -)
            //    movementPriorities.Add(0);
        }
        else if(!thisIsLeftPlayer)
        {
            if (rb.transform.position.x < target.x + offset)
                movementPriorities.Add(1);
            else if (rb.transform.position.x > target.x + offset)
                movementPriorities.Add(-1);
            else
                movementPriorities.Add(0);
        }
      
    }

    private void GoToGoal()
    {
        if (!thisIsLeftPlayer)
        {
            MoveTowards(new Vector2(zonePosition, 0));
        }
        else
        {
            MoveTowards(new Vector2(zonePosition * -1, 0));
        }
    }

    private void GoOnTopOfGoal()
    {
        if (!thisIsLeftPlayer)
        {
            MoveTowards(new Vector2(8, 0));
        }
        else
        {
            MoveTowards(new Vector2(-8, 0));
        }
        Jump();
    }

    private void Jump()
    {
        if(onGround)
        _jumpForce = Vector3.up * jumpForce;
    }


    
}
