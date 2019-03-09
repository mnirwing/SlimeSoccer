using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private CircleCollider2D collider;
    private AudioSource ballSound;
    private Rigidbody2D rb;

    [SerializeField]
    private float maxSpeed;

    // Use this for initialization
    void Start () {
        collider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        ballSound = GetComponent<AudioSource>();
	}

    private void Update()
    {
        if(rb.velocity.magnitude >= maxSpeed)
        {
            rb.velocity *= (maxSpeed / rb.velocity.magnitude); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collider.gameObject.tag != "Crossbar")
        {
            if(Mathf.Abs(rb.velocity.magnitude) > 5f)
            {
                ballSound.volume = 1;
                ballSound.Play();

            }
            if (Mathf.Abs(rb.velocity.magnitude) < 5f)
            {
                ballSound.volume *= (rb.velocity.magnitude / 5);
                ballSound.Play();

            }
        }
    }
}
