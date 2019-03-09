using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetection : MonoBehaviour {

    private BoxCollider2D goalCollider;
    private AudioSource goalNetSound;

    [SerializeField]
    private VersusManager versusManager;

    [SerializeField]
    private WorldcupManager worldcupManager;

    private void Start()
    {
        goalNetSound = GetComponent<AudioSource>();
        goalCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.enabled == false)
            return;

        if(collision.tag == "Ball" && this.gameObject.name.Equals("GoalRight"))
        {
            versusManager.GoalScored(true);
            worldcupManager.GoalScored(true);
            goalNetSound.Play();
        }

        if (collision.tag == "Ball" && this.gameObject.name.Equals("GoalLeft"))
        {
            versusManager.GoalScored(false);
            worldcupManager.GoalScored(false);
            goalNetSound.Play();


        }
    }
}
