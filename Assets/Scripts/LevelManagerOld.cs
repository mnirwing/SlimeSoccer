//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class LevelManagerOld : MonoBehaviour
//{

//    public int scoreLeftPlayer;
//    public int scoreRightPlayer;

//    [SerializeField]

//    public PlayerControllerOffline gamePlayerLeft;
//    public Player2ControllerOfflineSingle gamePlayerRight;

//    public BallMovementOffline gameBall;

//    public ZoneDetectorLeftSingle zoneDetectorLeft;
//    public ZoneDetectorRightSingle zoneDetectorRight;

//    public BoxCollider2D zoneDetectorRightCol;
//    public BoxCollider2D zoneDetectorLeftCol;



//    public GoalDetectionLeftSingle goalDetectionLeft;
//    public GoalDetectionRightSingle goalDetectionRight;

//    public Text scoreLeftText;
//    public Text scoreRightText;
//    public Text gameTime;

//    public Text nameLeftTeam;
//    public Text nameRightTeam;

//    public Text roundCounterText;



//    public Text Countdown;
//    public Text Announcement;

//    public float seconds;

//    public float gameTotalTime;

//    public int goalsToWin = 10;


//    public AudioSource stadiumSound;

//    public Rigidbody2D rbBall;

//    public MainMenuCanvas master;

//    public Sprite playerGER;

//    public Sprite player2BRA;
//    public Sprite player2ESP;
//    public Sprite player2FRA;
//    public Sprite player2HR;
//    public List<Sprite> sprites;

//    private bool gamePaused;
//    public bool goldenGoal;

//    public int roundCounter;


//    // Use this for initialization
//    void Start()
//    {

//        scoreLeftText.text = "" + scoreLeftPlayer;
//        scoreRightText.text = "" + scoreRightPlayer;
//        gameTime.text = "00:00";


//        //gameTotalTime = 10;

//        master = FindObjectOfType<MainMenuCanvas>();

//        if (master.startWM)
//        {
//            gamePlayerLeft.GetComponent<SpriteRenderer>().sprite = playerGER;
//            sprites.Add(player2BRA);
//            sprites.Add(player2ESP);
//            sprites.Add(player2FRA);
//            sprites.Add(player2HR);

//            int spriteNumber = Random.Range(0, sprites.Capacity);
//            gamePlayerRight.GetComponent<SpriteRenderer>().sprite = sprites[spriteNumber];
//            sprites.Remove(sprites[spriteNumber]);

//            nameLeftTeam.text = "Deutschland";

//            roundCounterText.text = "1. Runde";
//            roundCounter = 1;

//            NameRightTeam();

//            InitialSpawn();


//        }
//        else
//        {
//            nameLeftTeam.text = "";
//            nameRightTeam.text = "";
//            InitialSpawn();
//        }
//    }


//    // Update is called once per frame
//    void Update()
//    {

//        if (Input.GetKey("escape"))
//        {
//            SceneManager.LoadScene("Main");
//        }

//        if (!gamePaused)
//            seconds += Time.deltaTime;
//        int minutes = Mathf.RoundToInt(seconds) / 60;
//        int secondsOfMinute = Mathf.RoundToInt(seconds) % 60;
//        if (minutes < 10)
//        {

//            if (secondsOfMinute < 10)
//            {
//                gameTime.text = "0" + minutes + ":" + "0" + secondsOfMinute;
//            }
//            else
//            {
//                gameTime.text = "0" + minutes + ":" + secondsOfMinute;
//            }
//        }
//        else
//        {
//            if (secondsOfMinute < 10)
//            {
//                gameTime.text = minutes + ":" + "0" + secondsOfMinute;
//            }
//            else
//            {
//                gameTime.text = minutes + ":" + secondsOfMinute;
//            }
//        }




//        if (seconds >= gameTotalTime)
//        {
//            if (scoreLeftPlayer == scoreRightPlayer)
//            {
//                Announcement.text = "Golden Goal";
//                goldenGoal = true;
//            }

//            if (scoreLeftPlayer > scoreRightPlayer)
//            {
//                VictoryScreen();
//            }

//            if (scoreLeftPlayer < scoreRightPlayer)
//            {
//                DefeatScreen();
//            }



//        }

//        Debug.Log("Round: " + roundCounter);

//    }

//    public void NameRightTeam()
//    {
//        if (gamePlayerRight.GetComponent<SpriteRenderer>().sprite.name.Contains("ESP"))
//            nameRightTeam.text = "Spanien";

//        if (gamePlayerRight.GetComponent<SpriteRenderer>().sprite.name.Contains("HR"))
//            nameRightTeam.text = "Kroatien";

//        if (gamePlayerRight.GetComponent<SpriteRenderer>().sprite.name.Contains("FRA"))
//            nameRightTeam.text = "Frankreich";

//        if (gamePlayerRight.GetComponent<SpriteRenderer>().sprite.name.Contains("BRA"))
//            nameRightTeam.text = "Brasilien";
//    }


//    public void InitialSpawn()
//    {
//        StartCoroutine("InitialSpawnCoroutine");

//    }

//    public void LoadNextGame()
//    {
//        roundCounter++;

//        StartCoroutine("LoadNextGameCoroutine");

//    }

//    public void VictoryScreen()
//    {
//        StartCoroutine("VictoryScreenCoroutine");
//    }

//    public void DefeatScreen()
//    {
//        StartCoroutine("DefeatScreenCoroutine");
//    }

//    public void RespawnLeft()
//    {
//        StartCoroutine("RespawnCoroutineLeft");
//    }

//    public void RespawnRight()
//    {
//        StartCoroutine("RespawnCoroutineRight");
//    }

//    public IEnumerator VictoryScreenCoroutine()
//    {
//        gamePaused = true;
//        goldenGoal = false;

//        zoneDetectorRight.Reset();
//        zoneDetectorLeft.Reset();


//        zoneDetectorLeft.gameObject.SetActive(false);
//        zoneDetectorRight.gameObject.SetActive(false);
//        zoneDetectorLeftCol.enabled = false;
//        zoneDetectorRightCol.enabled = false;

//        goalDetectionRight.gameObject.SetActive(false);
//        goalDetectionLeft.gameObject.SetActive(false);

//        Announcement.text = "Spiel gewonnen!";

//        yield return new WaitForSeconds(5);

//        LoadNextGame();
//        StopCoroutine("VictoryScreenCoroutine");


//    }

//    public IEnumerator DefeatScreenCoroutine()
//    {
//        gamePaused = true;
//        goldenGoal = false;

//        zoneDetectorRight.Reset();
//        zoneDetectorLeft.Reset();


//        zoneDetectorLeft.gameObject.SetActive(false);
//        zoneDetectorRight.gameObject.SetActive(false);
//        zoneDetectorLeftCol.enabled = false;
//        zoneDetectorRightCol.enabled = false;

//        goalDetectionRight.gameObject.SetActive(false);
//        goalDetectionLeft.gameObject.SetActive(false);

//        Announcement.text = "Spiel verloren!";

//        yield return new WaitForSeconds(2);

//        SceneManager.LoadScene("Main");


//    }


//    public IEnumerator LoadNextGameCoroutine()
//    {
//        goldenGoal = false;
//        gamePaused = true;
//        seconds = 0;
//        stadiumSound.volume *= 2;

//        Announcement.text = "";

//        scoreLeftPlayer = 0;
//        scoreRightPlayer = 0;
//        scoreLeftText.text = "0";
//        scoreRightText.text = "0";

//        int spriteNumber = Random.Range(0, sprites.Capacity);
//        gamePlayerRight.GetComponent<SpriteRenderer>().sprite = sprites[spriteNumber];
//        sprites.Remove(sprites[spriteNumber]);
//        NameRightTeam();


//        zoneDetectorRight.Reset();
//        zoneDetectorLeft.Reset();


//        zoneDetectorLeft.gameObject.SetActive(false);
//        zoneDetectorRight.gameObject.SetActive(false);
//        zoneDetectorLeftCol.enabled = false;
//        zoneDetectorRightCol.enabled = false;

//        goalDetectionRight.gameObject.SetActive(false);
//        goalDetectionLeft.gameObject.SetActive(false);

//        gamePlayerLeft.gameObject.SetActive(false);
//        gamePlayerRight.gameObject.SetActive(false);

//        Countdown.text = "3";
//        yield return new WaitForSeconds(1);
//        gameBall.gameObject.SetActive(false);

//        roundCounterText.text = roundCounter + ". Runde";

//        Countdown.text = "2";
//        yield return new WaitForSeconds(1);
//        gameBall.gameObject.SetActive(true);
//        gameBall.transform.position = gameBall.RespawnPointBallLeft;

//        gameBall.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
//        Countdown.text = "1";
//        yield return new WaitForSeconds(1);
//        gameBall.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;


//        Countdown.text = "";
//        gamePlayerLeft.transform.position = gamePlayerLeft.RespawnPointLeft;
//        gamePlayerRight.transform.position = gamePlayerRight.RespawnPointRight;

//        gamePlayerLeft.gameObject.SetActive(true);
//        gamePlayerRight.gameObject.SetActive(true);


//        zoneDetectorLeft.gameObject.SetActive(true);
//        zoneDetectorRight.gameObject.SetActive(true);
//        zoneDetectorLeftCol.enabled = true;
//        zoneDetectorRightCol.enabled = true;

//        goalDetectionRight.gameObject.SetActive(true);
//        goalDetectionLeft.gameObject.SetActive(true);

//        gamePlayerRight.ResetCounterBallOnGoal();

//        gamePaused = false;


//        stadiumSound.volume /= 2;
//    }

//    public IEnumerator RespawnCoroutineLeft()
//    {
//        gamePaused = true;
//        Debug.Log("Starting Coroutine");
//        stadiumSound.volume *= 2;

//        zoneDetectorRight.Reset();
//        zoneDetectorLeft.Reset();

//        zoneDetectorLeft.gameObject.SetActive(false);
//        zoneDetectorRight.gameObject.SetActive(false);
//        zoneDetectorLeftCol.enabled = false;
//        zoneDetectorRightCol.enabled = false;

//        goalDetectionRight.gameObject.SetActive(false);
//        goalDetectionLeft.gameObject.SetActive(false);

//        gamePlayerLeft.gameObject.SetActive(false);
//        gamePlayerRight.gameObject.SetActive(false);

//        Countdown.text = "3";
//        yield return new WaitForSeconds(1);
//        gameBall.gameObject.SetActive(false);


//        Countdown.text = "2";
//        yield return new WaitForSeconds(1);
//        gameBall.gameObject.SetActive(true);
//        gameBall.transform.position = gameBall.RespawnPointBallLeft;

//        gameBall.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
//        Countdown.text = "1";
//        yield return new WaitForSeconds(1);
//        gameBall.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;


//        Countdown.text = "";
//        gamePlayerLeft.transform.position = gamePlayerLeft.RespawnPointLeft;
//        gamePlayerRight.transform.position = gamePlayerRight.RespawnPointRight;

//        gamePlayerLeft.gameObject.SetActive(true);
//        gamePlayerRight.gameObject.SetActive(true);


//        zoneDetectorLeft.gameObject.SetActive(true);
//        zoneDetectorRight.gameObject.SetActive(true);
//        zoneDetectorLeftCol.enabled = true;
//        zoneDetectorRightCol.enabled = true;

//        goalDetectionRight.gameObject.SetActive(true);
//        goalDetectionLeft.gameObject.SetActive(true);

//        gamePlayerRight.ResetCounterBallOnGoal();

//        gamePaused = false;


//        stadiumSound.volume /= 2;
//    }

//    public IEnumerator RespawnCoroutineRight()
//    {
//        Debug.Log("Starting Coroutine");
//        stadiumSound.volume *= 2;

//        gamePaused = true;


//        zoneDetectorRight.Reset();
//        zoneDetectorLeft.Reset();

//        zoneDetectorLeft.gameObject.SetActive(false);
//        zoneDetectorRight.gameObject.SetActive(false);
//        zoneDetectorLeftCol.enabled = false;
//        zoneDetectorRightCol.enabled = false;

//        goalDetectionRight.gameObject.SetActive(false);
//        goalDetectionLeft.gameObject.SetActive(false);

//        gamePlayerLeft.gameObject.SetActive(false);
//        gamePlayerRight.gameObject.SetActive(false);

//        Countdown.text = "3";
//        yield return new WaitForSeconds(1);
//        gameBall.gameObject.SetActive(false);


//        Countdown.text = "2";
//        yield return new WaitForSeconds(1);
//        gameBall.gameObject.SetActive(true);
//        gameBall.transform.position = gameBall.RespawnPointBallRight;

//        gameBall.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;


//        Countdown.text = "1";
//        yield return new WaitForSeconds(1);
//        gameBall.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;

//        Countdown.text = "";
//        gamePlayerLeft.transform.position = gamePlayerLeft.RespawnPointLeft;
//        gamePlayerRight.transform.position = gamePlayerRight.RespawnPointRight;

//        gamePlayerLeft.gameObject.SetActive(true);
//        gamePlayerRight.gameObject.SetActive(true);


//        zoneDetectorLeft.gameObject.SetActive(true);
//        zoneDetectorRight.gameObject.SetActive(true);
//        zoneDetectorLeftCol.enabled = true;
//        zoneDetectorRightCol.enabled = true;

//        goalDetectionRight.gameObject.SetActive(true);
//        goalDetectionLeft.gameObject.SetActive(true);

//        gamePlayerRight.ResetCounterBallOnGoal();

//        gamePaused = false;


//        stadiumSound.volume /= 2;
//    }

//    public IEnumerator InitialSpawnCoroutine()
//    {
//        gamePaused = true;


//        zoneDetectorLeft.gameObject.SetActive(false);
//        zoneDetectorRight.gameObject.SetActive(false);
//        zoneDetectorLeftCol.enabled = false;
//        zoneDetectorRightCol.enabled = false;
//        goalDetectionRight.gameObject.SetActive(false);
//        goalDetectionLeft.gameObject.SetActive(false);
//        gamePlayerLeft.gameObject.SetActive(false);
//        gamePlayerRight.gameObject.SetActive(false);
//        gameBall.transform.position = gameBall.RespawnPointBall;
//        gameBall.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;

//        Countdown.text = "3";
//        yield return new WaitForSeconds(1);
//        Countdown.text = "2";
//        yield return new WaitForSeconds(1);
//        Countdown.text = "1";
//        yield return new WaitForSeconds(1);
//        Countdown.text = "";
//        gameBall.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;


//        gamePlayerLeft.transform.position = gamePlayerLeft.RespawnPointLeft;
//        gamePlayerRight.transform.position = gamePlayerRight.RespawnPointRight;
//        gamePlayerLeft.gameObject.SetActive(true);
//        gamePlayerRight.gameObject.SetActive(true);
//        zoneDetectorLeft.gameObject.SetActive(true);
//        zoneDetectorRight.gameObject.SetActive(true);
//        zoneDetectorLeftCol.enabled = true;
//        zoneDetectorRightCol.enabled = true;
//        goalDetectionRight.gameObject.SetActive(true);
//        goalDetectionLeft.gameObject.SetActive(true);

//        gamePaused = false;

//        gamePlayerRight.ResetCounterBallOnGoal();


//    }

//    public void GoalLeftPlayer()
//    {
//        scoreLeftPlayer++;
//        scoreLeftText.text = "" + scoreLeftPlayer;

//    }

//    public void GoalRightPlayer()
//    {
//        scoreRightPlayer++;
//        scoreRightText.text = "" + scoreRightPlayer;


//    }
//}
