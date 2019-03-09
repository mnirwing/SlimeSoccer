using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbar : MonoBehaviour {

    private BoxCollider2D crossbarCollider;
    private AudioSource crossbarHitSound;

	// Use this for initialization
	void Start () {
        crossbarCollider = GetComponent<BoxCollider2D>();
        crossbarHitSound = GetComponent<AudioSource>();
    
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            crossbarHitSound.Play();
            Debug.Log("CrossbarHit");
        }
    }
    
}
