using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldcupManager : MonoBehaviour {

    [SerializeField]
    private GameManager gameManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}




    //goal score methods------------------------------------------------------------------------------
    public void GoalScored(bool leftPlayer)
    {
        //if (leftPlayer)
        //    goalsLeftPlayer++;
        //if (!leftPlayer)
        //    goalsRightPlayer++;

        //UpdateScoreboard();

        //zoneDetectorLeft.enabled = false;
        //zoneDetectorRight.enabled = false;

        //if (leftPlayer)
        //    Respawn(false);
        //else
        //    Respawn(true);

    }


    //respawn methods------------------------------------------------------------------------------
    IEnumerator Respawn(bool leftSide)
    {
        
        yield return new WaitForSeconds(1);
        //Player1.GetComponent<Rigidbody2D>().isKinematic = true;
        //Player2.GetComponent<Rigidbody2D>().isKinematic = true;
        //Ball.GetComponent<Rigidbody2D>().isKinematic = true;

        //Player1.GetComponent<Rigidbody2D>().MovePosition(SpawnPointLeft.transform.position);

        //yield return new WaitForSeconds(1);


    }
}
