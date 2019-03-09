using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class NPCController3 : MonoBehaviour
{
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

    private float speed = 6f;

    [SerializeField]
    private float jumpForce = 10000f;

    [SerializeField]
    private float dashForce = 1000f;

    private PlayerMotor motor;

    private int timer;

    private bool dashAllowed;
    private bool dash;

    [SerializeField]
    private int dashStamina = 100;
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
    private float distancePlayerBall;
    private float distanceEnemyBall;
    private float distancePlayerEnemy;

    private float counterBallOnGoal;

    public LayerMask mask;
    public LayerMask maskRebound;
    RaycastHit2D hit;
    RaycastHit2D reflectionHit;

    //[SerializeField]
    //private Transform goal;

    [SerializeField]
    private bool thisIsLeftPlayer;

    float _xMov;



    void Start()
    {
        motor = GetComponent<PlayerMotor>();

        jumpSound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        groundCheckLeft = transform.Find("CheckGroundLeft");
        groundCheckMiddle = transform.Find("CheckGroundMiddle");
        groundCheckRight = transform.Find("CheckGroundRight");

        trailRenderer = transform.Find("Trail").gameObject.GetComponent<TrailRenderer>();
        particles = transform.Find("BlobParticle").gameObject.GetComponent<ParticleSystem>();

        dashAllowed = true;
        dashStaminaTemp = dashStamina;
        jumpAllowed = true;
        

        if (!thisIsLeftPlayer)
        {
            goalPosition = 8.5f;
            zonePosition = 5.3f;
        }
        else
        {
            goalPosition = -8.5f;
            zonePosition = -5.3f;
        }

        ballIsFarFromEnemyDistance = 5.0f;
        ballIsFarFromPlayerDistance = 5.0f;
        playerIsFarFromEnemyDistance = 5.0f;


    }

    private void Update()
    {
        if (Mathf.Abs(rbBall.transform.position.x) > Mathf.Abs(goalPosition))
        {
            counterBallOnGoal += Time.deltaTime;
        }
        else
            counterBallOnGoal = 0;

        Vector3 _jumpForce = Vector3.zero;
        _xMov = 0f;
        float _dashForce = 0f;


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


        //calculate target position
        //RaycastHit2D hit = Physics2D.Raycast(rbBall.transform.position, rbBall.velocity.normalized, 100, 1 << 10);
        hit = Physics2D.Raycast(rbBall.transform.position, rbBall.velocity.normalized, 100, mask);
        Debug.DrawRay(rbBall.transform.position, new Vector3(hit.point.x, hit.point.y, 0) - rbBall.transform.position, Color.red, 2);

        if (hit.collider != null && thisIsLeftPlayer)
        {
            Debug.Log("Should move on x");
            if (hit.transform.tag == "Ground" || hit.transform.tag == "CrossbarLeft" || hit.transform.tag == "CrossbarRight")
            {
                //Debug.DrawRay(rbBall.transform.position, new Vector3(hit.point.x, hit.point.y, 0) - rbBall.transform.position, Color.red, 2);

                if (rb.transform.position.x > hit.point.x - 0.6f)
                {
                    _xMov = -1f;
                }

                if ((rb.transform.position.x < hit.point.x - 0.6f)) // && rb.transform.position.x >= 0f
                {
                    if (distanceEnemyBall > distancePlayerBall - (distancePlayerBall * 0.25f))
                        _xMov = 1f;
                }

                if (rb.transform.position.x > hit.point.x - 0.63f && (rb.transform.position.x < hit.point.x - 0.67f))
                {
                    _xMov = 0f;
                }
            }
            else
            {
                //wenn 1. Ray boundary links oder rechts trifft
                if (hit.transform.tag == "Boundary")
                {
                    reflectionHit = Physics2D.Raycast(hit.point,
                        new Vector2(rbBall.velocity.x * -1, rbBall.velocity.y),
                        100, mask);
                }

                //wenn 1. Ray boundary oben trifft
                if (hit.transform.tag == "BoundaryTop")
                {
                    reflectionHit = Physics2D.Raycast(hit.point,
                        new Vector2(rbBall.velocity.x, rbBall.velocity.y * -1).normalized,
                        100, maskRebound);
                    Debug.DrawRay(hit.point, reflectionHit.point - hit.point, Color.blue, 2);

                }

                //wenn 2. Ray Boden oder Latte trifft
                if (reflectionHit.transform.tag == "Ground"
                    || reflectionHit.transform.tag == "CrossbarLeft"
                    || reflectionHit.transform.tag == "CrossbarRight")
                {
                    if (rb.transform.position.x > reflectionHit.point.x - 0.6f)
                    {
                        _xMov = -1f;
                    }

                    if ((rb.transform.position.x < reflectionHit.point.x - 0.6f))  //&& rb.transform.position.x >= 0f && reflectionHit.point.x > -1f
                    {
                        if (distanceEnemyBall > distancePlayerBall - (distancePlayerBall * 0.25f)) // höherer Wert => aggressiver

                            _xMov = 1f;
                    }
                }

                //höchste priorität: wenn Ball Richtung Tor fliegt oder ball + spieler auf der linken seite + 
                if ((hit.point.x < -6 && hit.point.y > 1 && hit.point.y < 4) || (rbBall.velocity.x >= 0.1f && ballIsRight && enemyIsRight))
                {

                    _xMov = -1f;
                }
            }
        }
        else
        {
            GoToGoal();
        }


        if (hit.collider != null && !thisIsLeftPlayer)
        {
            Debug.Log(hit.collider.gameObject.name);
            Debug.Log(hit.transform.tag);
            if (hit.transform.tag == "Ground" || hit.transform.tag == "CrossbarLeft" || hit.transform.tag == "CrossbarRight")
            {
                Debug.Log("Should move on x");

                //Debug.DrawRay(rbBall.transform.position, new Vector3(hit.point.x, hit.point.y, 0) - rbBall.transform.position, Color.red, 2);

                if (rb.transform.position.x < hit.point.x + 0.6f)
                {
                    _xMov = 1f;
                }

                if ((rb.transform.position.x > hit.point.x + 0.6f)) // && rb.transform.position.x >= 0f
                {
                    if (distanceEnemyBall > distancePlayerBall - (distancePlayerBall * 0.25f))
                        _xMov = -1f;
                }

                if (rb.transform.position.x < hit.point.x + 0.63f && (rb.transform.position.x > hit.point.x + 0.67f))
                {
                    _xMov = 0f;
                }
            }
            else
            {
                //wenn 1. Ray boundary links oder rechts trifft
                if (hit.transform.tag == "Boundary")
                {
                    reflectionHit = Physics2D.Raycast(hit.point,
                        new Vector2(rbBall.velocity.x * -1, rbBall.velocity.y),
                        100, mask);
                }

                //wenn 1. Ray boundary oben trifft
                if (hit.transform.tag == "BoundaryTop")
                {
                    reflectionHit = Physics2D.Raycast(hit.point,
                        new Vector2(rbBall.velocity.x, rbBall.velocity.y * -1).normalized,
                        100, maskRebound);
                    Debug.DrawRay(hit.point, reflectionHit.point - hit.point, Color.blue, 2);

                }

                //wenn 2. Ray Boden oder Latte trifft
                if (reflectionHit.transform.tag == "Ground"
                    || reflectionHit.transform.tag == "CrossbarLeft"
                    || reflectionHit.transform.tag == "CrossbarRight")
                {
                    if (rb.transform.position.x < reflectionHit.point.x + 0.6f)
                    {
                        _xMov = 1f;
                    }

                    if ((rb.transform.position.x > reflectionHit.point.x + 0.6f))  //&& rb.transform.position.x >= 0f && reflectionHit.point.x > -1f
                    {
                        if (distanceEnemyBall > distancePlayerBall - (distancePlayerBall * 0.25f)) // höherer Wert => aggressiver

                            _xMov = -1f;
                    }
                }

                //höchste priorität: wenn Ball Richtung Tor fliegt oder ball + spieler auf der linken seite + 
                if ((hit.point.x > 6 && hit.point.y > 1 && hit.point.y < 4) || (rbBall.velocity.x <= -0.1f && !ballIsRight && !enemyIsRight))
                {

                    _xMov = 1f;
                }
            }
        }
        else
        {
            GoToGoal();
        }

        //wenn der Ball über dem Tor ist, werden die vorherigen Befehle überschrieben
        if (counterBallOnGoal > 1.5f && thisIsLeftPlayer)
        {
            _xMov = 1;
            if (counterBallOnGoal > 3)
            {
                if (rb.transform.position.x < -7.87)
                {
                    _xMov = 1;
                }
                if (rb.transform.position.x > -7.5)
                {
                    _xMov = -1;
                }
            }
        }

        if (counterBallOnGoal > 1.5f && !thisIsLeftPlayer)
        {
            _xMov = 1;
            if (counterBallOnGoal > 3)
            {
                if (rb.transform.position.x > 7.87)
                {
                    _xMov = -1;
                }
                if (rb.transform.position.x < 7.5)
                {
                    _xMov = 1;
                }
            }

        }

        //wenn der Ball mittig über dem Spieler ist, noch ein Stückchen nach rechts
        if ((rbBall.transform.position.x > rb.transform.position.x - 0.25f
            && rbBall.transform.position.x < rb.transform.position.x + 0.25f)
            && (rbBall.transform.position.y - rb.transform.position.y) < 1.25f
            && !thisIsLeftPlayer)
        {
            _xMov = 1;
        }

        if ((rbBall.transform.position.x < rb.transform.position.x + 0.25f
            && rbBall.transform.position.x > rb.transform.position.x - 0.25f)
            && (rbBall.transform.position.y - rb.transform.position.y) < 1.25f
            && thisIsLeftPlayer)
        {
            _xMov = -1;
        }

        //wenn er auf dem Tor stehen geblieben ist
        if (rb.transform.position.x > 6.28 && rb.transform.position.y > 4 && zoneDetector.timer > 2.95f && counterBallOnGoal == 0 && !thisIsLeftPlayer)
            _xMov = -1;

        if (rb.transform.position.x < -6.28 && rb.transform.position.y > 4 && zoneDetector.timer > 2.95f && counterBallOnGoal == 0 && thisIsLeftPlayer)
            _xMov = 1;

        //wenn er den sprung nicht geschafft hat und im tor steht
        if (zoneDetector.timer < 2.3f && !thisIsLeftPlayer)
        {
            _xMov = -1;
        }

        if (zoneDetector.timer < 2.3f && thisIsLeftPlayer)
        {
            _xMov = 1;
        }

        motor.Move(_xMov * speed);

        //wenn der Ball bei dem Spieler ist und 4 > y > 2 süpringt er
        if ((Mathf.Abs(rb.position.x - rbBall.transform.position.x) < 1f) &&
            (rbBall.transform.position.y < 4f) &&
            (rbBall.transform.position.y > 2) &&
            onGround)
        {
            //22.5 is Ball max speed
            if (rbBall.velocity.magnitude > 20)
            {

                if (Random.Range(0, 100) > 80)
                {
                    return;
                }
                else
                {
                    _jumpForce = Vector3.up * jumpForce;
                    jumpSound.Play();
                }
            }
            else
            {
                _jumpForce = Vector3.up * jumpForce;
                jumpSound.Play();
            }
        }

        //wenn der Ball über dem Tor ist, werden die vorherigen Befehle überschrieben
        if (counterBallOnGoal > 1.5f && onGround)
        {
            _jumpForce = Vector3.up * jumpForce;
            jumpSound.Play();
        }

        //apply jumpforce
        motor.ApplyJumpForce(_jumpForce);

        if (dashStaminaTemp > 0)
        {
            dashAllowed = true;
        }
        else
        {
            dashAllowed = false;
        }

        if (distancePlayerBall < 1.5f && rbBall.velocity.x > 0)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
        }
    }

    //player goes to the goal and stays there
    private void GoToGoal()
    {
        if (rb.transform.position.x < zonePosition)
        {
            _xMov = 1f;
        }

        if (rb.transform.position.x > zonePosition)
        {
            _xMov = -1f;

        }

        if (rb.transform.position.x > (zonePosition - 0.1f) && rb.transform.position.x < (zonePosition + 0.1f))
        {
            _xMov = 0f;

        }
    }

    public void ResetCounterBallOnGoal()
    {
        counterBallOnGoal = 0;
    }


}