using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private VersusManager versusManager;
    [SerializeField]
    private WorldcupManager worldcupManager;


    [SerializeField]
    private Transform SpawnPointLeft;
    [SerializeField]
    private Transform SpawnPointRight;
    [SerializeField]
    private Transform SpawnPointBall;


    private Color[] teamColours;
    private Image colourLeft;
    private Image colourRight;

    private Sprite[] flags;

    private Image imageLeft;
    private Image imageRight;

    [SerializeField]
    private Canvas mainCanvas;
    private GameObject teamSelect;

    private TextMeshProUGUI teamSelectTextLeft;
    private TextMeshProUGUI teamSelectTextRight;

    private int TeamColourLeft;
    private int TeamColourRight;

    private int TeamLeft;
    private int TeamRight;

    public bool ReadyLeft;
    public bool ReadyRight;

    [SerializeField]
    private GameObject BallPrefab;

    [SerializeField]
    private GameObject Player1Prefab;
    [SerializeField]
    private GameObject Player2Prefab;

    [SerializeField]
    private GameObject NPC1Prefab;
    [SerializeField]
    private GameObject NPC2Prefab;

    private GameObject Player1;
    private GameObject Player2;
    private GameObject Ball;

    private GameObject Scoreboard;

    [SerializeField]
    private ZoneDetector zoneDetectorLeft;
    [SerializeField]
    private ZoneDetector zoneDetectorRight;

    private bool player1IsNPC;
    private bool player2IsNPC;

    private Text playerOrNPCText1;
    private Text playerOrNPCText2;

    private Button playerOrNPCButton1;
    private Button playerOrNPCButton2;

    private GameObject randomLeft;
    private GameObject randomRight;

    private TextMeshProUGUI gameEndOptionText;
    private TextMeshProUGUI amountOptionText;

    private Button startButton;

    private List<string> endGameOptions;
    private int currentEndGameOption;
    private int currentAmount;
    private bool startGame;
    private GameObject gameSettings;

    private TeamSelectInputManager teamSelectInputManager;
    private GameSettingsInputManager gameSettingsInputManager;

    // Use this for initialization
    void Awake() {

        //Instantiate Background NPC Play----------------


        //-----------------------------------------------
        Scoreboard = mainCanvas.transform.Find("ScoreboardPanel").gameObject;
        teamSelectInputManager = FindObjectOfType<TeamSelectInputManager>();
        gameSettingsInputManager = FindObjectOfType<GameSettingsInputManager>();
        gameSettings = gameSettingsInputManager.gameObject;
        

        endGameOptions = new List<string>();
        endGameOptions.Add("Time");
        endGameOptions.Add("Goals");

        teamSelect = mainCanvas.transform.Find("TeamSelect").gameObject;

        imageLeft = mainCanvas.transform.Find("TeamSelect").Find("Left").Find("Team").Find("1FlagImage").GetComponent<Image>();
        imageRight = mainCanvas.transform.Find("TeamSelect").Find("Right").Find("Team").Find("2FlagImage").GetComponent<Image>();

        colourLeft = mainCanvas.transform.Find("TeamSelect").Find("Left").Find("Colour").Find("1TeamColour").GetComponent<Image>();
        colourRight = mainCanvas.transform.Find("TeamSelect").Find("Right").Find("Colour").Find("2TeamColour").GetComponent<Image>();

        teamSelectTextLeft = mainCanvas.transform.Find("TeamSelect").Find("Left").Find("Team").Find("1CountryText").GetComponent<TextMeshProUGUI>();
        teamSelectTextRight = mainCanvas.transform.Find("TeamSelect").Find("Right").Find("Team").Find("2CountryText").GetComponent<TextMeshProUGUI>();

        playerOrNPCText1 = mainCanvas.transform.Find("TeamSelect").Find("Left").Find("Control")
            .Find("PlayerOrNPCButtonLeft").GetComponentInChildren<Text>();
        playerOrNPCText2 = mainCanvas.transform.Find("TeamSelect").Find("Right").Find("Control")
            .Find("PlayerOrNPCButtonRight").GetComponentInChildren<Text>();

        gameEndOptionText = mainCanvas.transform.Find("GameSettings")
            .Find("GameEnd").Find("GameEndOption").GetComponent<TextMeshProUGUI>();

        amountOptionText = mainCanvas.transform.Find("GameSettings")
            .Find("Numbers").Find("AmountNumbers").GetComponent<TextMeshProUGUI>();

        flags = Resources.LoadAll<Sprite>("Flags");
        TeamLeft = 0;
        TeamRight = 0;

        teamColours = new Color[8];
        teamColours[0] = Color.red;
        teamColours[1] = Color.yellow;
        teamColours[2] = Color.green;
        teamColours[3] = Color.blue;
        teamColours[4] = Color.black;
        teamColours[5] = Color.cyan;
        teamColours[6] = Color.grey;
        teamColours[7] = Color.magenta;

        imageLeft.sprite = flags[TeamLeft];
        imageRight.sprite = flags[TeamRight];
        teamSelectTextLeft.text = flags[TeamLeft].name;
        teamSelectTextRight.text = flags[TeamRight].name;
        UpdateColoursOfImages();

        teamSelectInputManager.enabled = true;
        gameSettings.SetActive(false);
        Scoreboard.SetActive(false);
    }

    private void Update()
    {
        //---------------------------------------------------------------------------------------
        if (ReadyLeft && ReadyRight)
        {
            teamSelect.SetActive(false);
            teamSelectInputManager.enabled = false;

            gameSettings.SetActive(true);
            gameSettingsInputManager.enabled = true;
        }

        if (startGame)
        {
            versusManager.endGameAmount = currentAmount;
            if (currentEndGameOption == 0)
                versusManager.endGameTime = true;
            LoadGame();
        }
    }

    //Start the game--------------------------------------------------------------------------------------------
    public void LoadGame()
    {
        gameSettings.SetActive(false);
        gameSettingsInputManager.enabled = false;
        Scoreboard.SetActive(true);
        versusManager.gameObject.SetActive(true);

        //Set the scoreboard text
        Text[] scoreBoardText = Scoreboard.GetComponentsInChildren<Text>();
        scoreBoardText[0].text = "00:00";
        scoreBoardText[1].text = flags[TeamLeft].name.Replace("_"," ");
        scoreBoardText[2].text = "0";
        scoreBoardText[3].text = "0";
        scoreBoardText[4].text = flags[TeamRight].name.Replace("_", " ");

        //6: countdown
        //7: announcement
        //8: round counter

        //instantiate players and ball
        if(!player1IsNPC)
            Player1 = Instantiate(Player1Prefab, SpawnPointLeft.transform.position, Quaternion.identity);
        if(player1IsNPC)
            Player1 = Instantiate(NPC1Prefab, SpawnPointLeft.transform.position, Quaternion.identity);
        if(!player2IsNPC)
            Player2 = Instantiate(Player2Prefab, SpawnPointRight.transform.position, Quaternion.identity);
        if (player2IsNPC)
            Player2 = Instantiate(NPC2Prefab, SpawnPointRight.transform.position, Quaternion.identity);

        Ball = Instantiate(BallPrefab, SpawnPointBall.transform.position, Quaternion.identity);

        //set player references
        if (player1IsNPC)
        {
            Player1.GetComponent<NPCController>().rbEnemy = Player2.GetComponent<Rigidbody2D>();
            Player1.GetComponent<NPCController>().rbBall = Ball.GetComponent<Rigidbody2D>();
            Player1.GetComponent<NPCController>().zoneDetector = zoneDetectorLeft;
        }

        if (player2IsNPC)
        {
            Player2.GetComponent<NPCController>().rbEnemy = Player1.GetComponent<Rigidbody2D>();
            Player2.GetComponent<NPCController>().rbBall = Ball.GetComponent<Rigidbody2D>();
            Player2.GetComponent<NPCController>().zoneDetector = zoneDetectorRight;
        }

        //set references for versusManager
        versusManager.Player1 = Player1;
        versusManager.Player2 = Player2;
        versusManager.Ball = Ball;

        //Set the particle colour
        var main = Player1.transform.Find("BlobParticle").GetComponent<ParticleSystem>().main;
        main.startColor = teamColours[TeamColourLeft];

        var main2 = Player2.transform.Find("BlobParticle").GetComponent<ParticleSystem>().main;
        main2.startColor = teamColours[TeamColourRight];

        //set the player colours
        Player1.transform.GetComponent<SpriteRenderer>().color = teamColours[TeamColourLeft];
        Player2.transform.GetComponent<SpriteRenderer>().color = teamColours[TeamColourRight];

        //set the player flags
        Player1.transform.Find("Flag").GetComponent<SpriteRenderer>().sprite = flags[TeamLeft];
        Player2.transform.Find("Flag").GetComponent<SpriteRenderer>().sprite = flags[TeamRight];

        //disable teamselect canvas
        teamSelect.SetActive(false);
        this.gameObject.SetActive(false);
    }

    //Next, previous and update colours---------------------------------------------------------------------------

    public void UpdateColoursOfImages()
    {
        colourLeft.color = teamColours[TeamColourLeft];
        colourRight.color = teamColours[TeamColourRight];
    }

    public void OnClickNextColourLeft()
    {
        UncheckReadyLeft();

        if (TeamColourLeft < teamColours.Length - 1)
        {
            TeamColourLeft++;
        }
        else if (TeamColourLeft == teamColours.Length - 1)
        {
            TeamColourLeft = 0;
        }

        UpdateColoursOfImages();
    }

    public void OnClickPreviousColourLeft()
    {
        UncheckReadyLeft();

        if (TeamColourLeft > 0)
        {
            TeamColourLeft--;
        }
        else if (TeamColourLeft == 0)
        {
            TeamColourLeft = teamColours.Length - 1;
        }

        UpdateColoursOfImages();
    }

    public void OnClickNextColourRight()
    {
        UncheckReadyRight();


        if (TeamColourRight < teamColours.Length - 1)
        {
            TeamColourRight++;
        }

        if (TeamColourRight == teamColours.Length - 1)
        {
            TeamColourRight = 0;
        }

        UpdateColoursOfImages();
    }

    public void OnClickPreviousColourRight()
    {
        UncheckReadyRight();

        if (TeamColourRight > 0)
        {
            TeamColourRight--;
        }

        if (TeamColourRight == 0)
        {
            TeamColourRight = teamColours.Length - 1;
        }

        UpdateColoursOfImages();
    }



    //Rename Teams----------------------------------------------------------------------------------------
    public void RenameTeams()
    {
        teamSelectTextLeft.text = flags[TeamLeft].name.Replace("_", " ");
        teamSelectTextRight.text = flags[TeamRight].name.Replace("_", " ");

    }

    //Random Buttons-------------------------------------------------------------------------------------
    public void OnClickRandomLeft()
    {
        UncheckReadyLeft();

        TeamLeft = Random.Range(0, flags.Length - 1);
        imageLeft.sprite = flags[TeamLeft];
        RenameTeams();
        TeamColourLeft = Random.Range(0, teamColours.Length - 1);
        UpdateColoursOfImages();
    }

    public void OnClickRandomRight()
    {
        UncheckReadyRight();

        TeamRight = Random.Range(0, flags.Length - 1);
        imageRight.sprite = flags[TeamRight];
        RenameTeams();
        TeamColourRight = Random.Range(0, teamColours.Length - 1);
        UpdateColoursOfImages();

    }



    //Next and previous flags-----------------------------------------------------------------------------
    public void OnClickNextFlagLeft()
    {
        UncheckReadyLeft();

        if (TeamLeft < flags.Length - 1){
            TeamLeft++;
            imageLeft.sprite = flags[TeamLeft];
        }

        if (TeamLeft == flags.Length - 1)
        {
            TeamLeft = 0;
            imageLeft.sprite = flags[TeamLeft];
        }

        RenameTeams();


    }

    public void OnClickPreviousFlagLeft()
    {
        UncheckReadyLeft();


        if (TeamLeft > 0)
        {
            TeamLeft--;
            imageLeft.sprite = flags[TeamLeft];
        }

        if (TeamLeft == 0)
        {
            TeamLeft = flags.Length - 1;
            imageLeft.sprite = flags[TeamLeft];
        }

        RenameTeams();
    }

    public void OnClickNextFlagRight()
    {
        UncheckReadyRight();

        if (TeamRight < flags.Length - 1)
        {
            TeamRight++;
            imageRight.sprite = flags[TeamRight];
        }

        if (TeamRight == flags.Length - 1)
        {
            TeamRight = 0;
            imageRight.sprite = flags[TeamRight];
        }
        RenameTeams();
    }

    public void OnClickPreviousFlagRight()
    {
        UncheckReadyRight();

        if (TeamRight > 0)
        {
            TeamRight--;
            imageRight.sprite = flags[TeamRight];
        }

        if (TeamRight == 0)
        {
            TeamRight = flags.Length - 1;
            imageRight.sprite = flags[TeamRight];
        }
        RenameTeams();
    }


    //Ready Buttons-----------------------------------------------------------------------------------------
    public void OnClickReadyLeft()
    {
        ReadyLeft = !ReadyLeft;

    }

    public void OnClickReadyRight()
    {
        ReadyRight = !ReadyRight;

    }

    public void UncheckReadyLeft()
    {
        if (ReadyLeft)
            ReadyLeft = false;
    }

    public void UncheckReadyRight()
    {
        if (ReadyRight)
            ReadyRight = false;
    }

    //Player Or NPC toggle----------------------------------------------------------------------------------
    public void OnClickPlayerOrNPC1()
    {
        UncheckReadyLeft();

        if (playerOrNPCText1.text == "NPC"){
            playerOrNPCText1.text = "Player";
            player1IsNPC = false;
        }
        else
        {
            playerOrNPCText1.text = "NPC";
            player1IsNPC = true;
        }

        
    }

    public void OnClickPlayerOrNPC2()
    {
        UncheckReadyRight();

        if (playerOrNPCText2.text == "NPC")
        {
            playerOrNPCText2.text = "Player";
            player2IsNPC = false;
        }
        else
        {
            playerOrNPCText2.text = "NPC";
            player2IsNPC = true;
        }
    }

    //game settings methods-----------------------------------------------------------------
    public void OnClickChangeEndGameCondition(bool left)
    {
        if (left)
        {
            currentEndGameOption--;
            if (currentEndGameOption < 0)
                currentEndGameOption = endGameOptions.Count - 1;
        }
        if (!left)
        {
            currentEndGameOption++;
            if (currentEndGameOption > endGameOptions.Count - 1)
                currentEndGameOption = 0;
        }

        gameEndOptionText.text = endGameOptions[currentEndGameOption];
    }

    public void OnClickChangeAmount(bool left)
    {
        if (left)
        {
            currentAmount--;
            if (currentAmount < 0)
                currentAmount = 0;
        }
        if (!left)
        {
            currentAmount++;
            if (currentAmount > 20)
                currentAmount = 20;
        }

        gameEndOptionText.text = endGameOptions[currentEndGameOption];

        if (currentEndGameOption == 0)
            amountOptionText.text = "" + currentAmount + ":00";
        if (currentEndGameOption == 1)
            amountOptionText.text = "" + currentAmount;
    }

    public void OnClickStartGame()
    {
        startGame = true;
    }
}
