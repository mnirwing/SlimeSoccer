using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDetector : MonoBehaviour
{
    [SerializeField]
    private string playerToDetect;

    private SpriteRenderer zone;

    public VersusManager versusManager;
    public WorldcupManager worldcupManager;

    int color;
    public float timer;
    float colorRatio;

    bool inZone;



    // Use this for initialization
    void Start()
    {
        zone = this.transform.GetComponentInParent<SpriteRenderer>();
        timer = 3f;
        zone.color = new Color(255, 255, 255);
        inZone = false;
    }

    private void FixedUpdate()
    {
        if (this.enabled == false)
            return;

        if (inZone)
        {

            timer -= Time.deltaTime;
            colorRatio = timer / 3;
            zone.color = Color.Lerp(Color.red, Color.white, colorRatio);

            if (timer <= 0)
            {
                timer = 3f;
                inZone = false;
                zone.color = Color.white;

                if(versusManager != null)
                {
                    if(playerToDetect == "Player1" || playerToDetect == "Player1AI")
                    {
                        versusManager.GoalScored(false);
                    }
                    else if (playerToDetect == "Player2" || playerToDetect == "Player2AI")
                    {
                        versusManager.GoalScored(true);
                    }

                }
                else if (worldcupManager != null)
                {
                    if (playerToDetect == "Player1" || playerToDetect == "Player1AI")
                    {
                        worldcupManager.GoalScored(false);
                    }
                    else if (playerToDetect == "Player2" || playerToDetect == "Player2AI")
                    {
                        worldcupManager.GoalScored(true);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == playerToDetect)
        {
            inZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == playerToDetect)
        {
            inZone = false;
            timer = 3f;
            zone.color = Color.white;
        }

    }

    public void Reset()
    {
        timer = 3f;
        inZone = false;
        zone.color = Color.white;
    }

}
