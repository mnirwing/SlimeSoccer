using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VersusManager : MonoBehaviour {

    [SerializeField]
    private Transform SpawnPointLeft;
    [SerializeField]
    private Transform SpawnPointRight;
    [SerializeField]
    private Transform SpawnPointBallLeft;
    [SerializeField]
    private Transform SpawnPointBallRight;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private Canvas mainCanvas;
    private Transform Scoreboard;

    private int goalsLeftPlayer;
    private int goalsRightPlayer;

    private bool gamePaused;
    private float seconds;

    private Text[] scoreBoardText;

    public GameObject Player1;
    public GameObject Player2;
    public GameObject Ball;

    [SerializeField]
    private ZoneDetector zoneDetectorLeft;
    [SerializeField]
    private ZoneDetector zoneDetectorRight;

    [SerializeField]
    private GoalDetection goalDetectionLeft;

    [SerializeField]
    private GoalDetection goalDetectionRight;

    private bool inVictoryScreenCoroutine;

    public int endGameAmount;
    public bool endGameTime;

    void Awake () {
        // todo: scoreboard var zuweisen
        Scoreboard = mainCanvas.transform.Find("ScoreboardPanel");

        //Set the scoreboard text
        scoreBoardText = Scoreboard.GetComponentsInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update () {

        //gametime counter-----------------------------------------------------------
        if (!gamePaused)
            seconds += Time.deltaTime;
        int minutes = Mathf.RoundToInt(seconds) / 60;
        int secondsOfMinute = Mathf.RoundToInt(seconds) % 60;

        if (minutes < 10)
        {

            if (secondsOfMinute < 10)
            {
                scoreBoardText[0].text = "0" + minutes + ":" + "0" + secondsOfMinute;
            }
            else
            {
                scoreBoardText[0].text = "0" + minutes + ":" + secondsOfMinute;
            }
        }
        else
        {
            if (secondsOfMinute < 10)
            {
                scoreBoardText[0].text = minutes + ":" + "0" + secondsOfMinute;
            }
            else
            {
                scoreBoardText[0].text = minutes + ":" + secondsOfMinute;
            }
        }

        //----------------------------------------------------------------------------
        if (seconds >= endGameAmount * 60 && endGameTime)
        {
            if(goalsLeftPlayer == goalsRightPlayer)
            {
                if (seconds <= (endGameAmount * 60) + 5)
                    scoreBoardText[7].text = "Golden Goal!";
                else
                    scoreBoardText[7].text = "";
            }
            else
            {
                if(goalsLeftPlayer > goalsRightPlayer)
                {
                    StartCoroutine("VictoryScreen", true);
                }
                else
                {
                    StartCoroutine("VictoryScreen", false);
                }
            }
        }
    }


    //goal score methods------------------------------------------------------------------------------
    public void GoalScored(bool leftPlayer)
    {
        if (leftPlayer)
            goalsLeftPlayer++;
        if (!leftPlayer)
            goalsRightPlayer++;

        UpdateScoreboard();

        gamePaused = true;

        zoneDetectorLeft.enabled = false;
        zoneDetectorRight.enabled = false;
        goalDetectionLeft.enabled = false;
        goalDetectionRight.enabled = false;

        if (!endGameTime && (goalsLeftPlayer == endGameAmount || goalsRightPlayer == endGameAmount))
        {
            if (goalsLeftPlayer == endGameAmount)
            {
                if (!inVictoryScreenCoroutine)
                StartCoroutine("VictoryScreen", true);
                return;
            }
            if (goalsRightPlayer == endGameAmount)
            {
                if (!inVictoryScreenCoroutine)
                StartCoroutine("VictoryScreen", false);
                return;

            }
        }
        else
        {
            if (leftPlayer)
                StartCoroutine(Respawn(false));
            else
                StartCoroutine(Respawn(true));
        }
    }

        //respawn methods------------------------------------------------------------------------------
        IEnumerator Respawn(bool leftSide)
    {
        Debug.Log("Coroutine Respawn");

        scoreBoardText[6].text = "3";
        yield return new WaitForSeconds(1);
        scoreBoardText[6].text = "2";

        if(Player1.GetComponent<PlayerController>() != null)
            Player1.GetComponent<PlayerController>().enabled = false;
        if (Player1.GetComponent<NPCController>() != null)
            Player1.GetComponent<NPCController>().enabled = false;

        if (Player2.GetComponent<PlayerController>() != null)
            Player2.GetComponent<PlayerController>().enabled = false;
        if (Player2.GetComponent<NPCController>() != null)
            Player2.GetComponent<NPCController>().enabled = false;
        

        Player1.transform.position = Vector3.MoveTowards(transform.position, SpawnPointLeft.transform.position, 20f);
        Player2.transform.position = Vector3.MoveTowards(transform.position, SpawnPointRight.transform.position, 20f);

        if(leftSide)
            Ball.transform.position = Vector3.MoveTowards(transform.position, SpawnPointBallLeft.transform.position, 20f);
        else
            Ball.transform.position = Vector3.MoveTowards(transform.position, SpawnPointBallRight.transform.position, 20f);

        Player1.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Player2.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Ball.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        yield return new WaitForSeconds(1);

        scoreBoardText[6].text = "1";

        yield return new WaitForSeconds(1);
        scoreBoardText[6].text = "";

        Player1.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        Player2.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        Ball.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        if (Player1.GetComponent<PlayerController>() != null)
            Player1.GetComponent<PlayerController>().enabled = true;
        if (Player1.GetComponent<NPCController>() != null)
            Player1.GetComponent<NPCController>().enabled = true;

        if (Player2.GetComponent<PlayerController>() != null)
            Player2.GetComponent<PlayerController>().enabled = true;
        if (Player2.GetComponent<NPCController>() != null)
            Player2.GetComponent<NPCController>().enabled = true;

        zoneDetectorLeft.enabled = true;
        zoneDetectorRight.enabled = true;
        goalDetectionLeft.enabled = true;
        goalDetectionRight.enabled = true;

        gamePaused = false;
    }

    IEnumerator VictoryScreen(bool left)
    {
        inVictoryScreenCoroutine = true;
        Debug.Log("Coroutine VictoryScreen");
        yield return new WaitForSeconds(1);

        if (Player1.GetComponent<PlayerController>() != null)
            Player1.GetComponent<PlayerController>().enabled = false;
        if (Player1.GetComponent<NPCController>() != null)
            Player1.GetComponent<NPCController>().enabled = false;

        if (Player2.GetComponent<PlayerController>() != null)
            Player2.GetComponent<PlayerController>().enabled = false;
        if (Player2.GetComponent<NPCController>() != null)
            Player2.GetComponent<NPCController>().enabled = false;

        if (left)
            scoreBoardText[7].text = scoreBoardText[1].text + " has won!";

        if(!left)
            scoreBoardText[7].text = scoreBoardText[4].text + " has won!";

        yield return new WaitForSeconds(3);

        Destroy(Player1);
        Destroy(Player2);
        Destroy(Ball);

        //mainCanvas.transform.Find("TeamSelect").gameObject.SetActive(true);
        //mainCanvas.transform.Find("GameSettings").gameObject.SetActive(true);

        SceneManager.LoadScene("SampleScene");
        //TODO: VICTORY SCREEN UND ABBRUCH
    }


    //----------------------------------------------------------------------------------------------
    private void UpdateScoreboard()
    {
        scoreBoardText[2].text = "" + goalsLeftPlayer;
        scoreBoardText[3].text = "" + goalsRightPlayer;
    }
}
