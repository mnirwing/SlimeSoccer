using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamSelectInputManager : MonoBehaviour
{

    private Canvas teamSelectCanvas;
    private GameManager gameManager;

    [SerializeField]
    private Button playerOrNPCButtonLeft;
    [SerializeField]
    private Button playerOrNPCButtonRight;

    private Text playerOrNPCTextLeft;
    private Text playerOrNPCTextRight;

    [SerializeField]
    private Button readyButtonLeft;
    [SerializeField]
    private Button readyButtonRight;

    [SerializeField]
    private Image[] highlightColoursLeft;
    [SerializeField]
    private Image[] highlightColoursRight;

    private Color defaultColor;

    private int currentHighlightPlayerLeft;
    private int currentHighlightPlayerRight;


    // Use this for initialization
    void Start()
    {
        teamSelectCanvas = FindObjectOfType<Canvas>();
        gameManager = FindObjectOfType<GameManager>();

        playerOrNPCTextLeft = playerOrNPCButtonLeft.GetComponentInChildren<Text>();
        playerOrNPCTextRight = playerOrNPCButtonRight.GetComponentInChildren<Text>();

        defaultColor = new Color(0, 0, 0, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHighlightedField();

        //highlighting of menu left---------------------------------------------------------------
        if (Input.GetButtonDown("Vertical Player 1"))
        {
            if (Input.GetAxisRaw("Vertical Player 1") == -1)
            {
                currentHighlightPlayerLeft++;

                if (currentHighlightPlayerLeft == highlightColoursLeft.Length)
                    currentHighlightPlayerLeft = 0;
            }

            if (Input.GetAxisRaw("Vertical Player 1") == 1)
            {
                currentHighlightPlayerLeft--;

                if (currentHighlightPlayerLeft < 0)
                    currentHighlightPlayerLeft = highlightColoursLeft.Length - 1;
            }

            UpdateHighlightedField();
        }

        //highlighting of menu right---------------------------------------------------------------
        if (Input.GetButtonDown("Vertical Player 2"))
        {
            if (Input.GetAxisRaw("Vertical Player 2") == -1)
            {
                currentHighlightPlayerRight++;

                if (currentHighlightPlayerRight == highlightColoursRight.Length)
                    currentHighlightPlayerRight = 0;
            }

            if (Input.GetAxisRaw("Vertical Player 2") == 1)
            {
                currentHighlightPlayerRight--;

                if (currentHighlightPlayerRight < 0)
                    currentHighlightPlayerRight = highlightColoursRight.Length - 1;
            }

            UpdateHighlightedField();
        }

        //flaggen und farben wahl mit horizontalem input left
        if (Input.GetButtonDown("Horizontal Player 1"))
        {
            switch (currentHighlightPlayerLeft)
            {
                case 1:
                    if (Input.GetAxisRaw("Horizontal Player 1") == 1)
                    {
                        gameManager.OnClickNextColourLeft();
                    }
                    else
                        gameManager.OnClickPreviousColourLeft();
                    return;

                case 2:
                    if (Input.GetAxisRaw("Horizontal Player 1") == 1)
                    {
                        gameManager.OnClickNextFlagLeft();
                    }
                    else
                        gameManager.OnClickPreviousFlagLeft();
                    return;
            }
        }

        //flaggen und farben wahl mit horizontalem input right
        if (Input.GetButtonDown("Horizontal Player 2"))
        {
            switch (currentHighlightPlayerRight)
            {
                case 1:
                    if (Input.GetAxisRaw("Horizontal Player 2") == 1)
                    {
                        gameManager.OnClickNextColourRight();
                    }
                    else
                        gameManager.OnClickPreviousColourRight();
                    return;

                case 2:
                    if (Input.GetAxisRaw("Horizontal Player 2") == 1)
                    {
                        gameManager.OnClickNextFlagRight();
                    }
                    else
                        gameManager.OnClickPreviousFlagRight();
                    return;
            }
        }

        //ready und random auswahl left
        if (Input.GetButtonDown("Dash Player 1"))
        {
            switch (currentHighlightPlayerLeft)
            {
                case 0:
                    gameManager.OnClickPlayerOrNPC1();
                    return;

                case 3:
                    gameManager.OnClickRandomLeft();
                    return;

                case 4:
                    gameManager.OnClickReadyLeft();
                    return;

            }
        }

        //ready und random auswahl right
        if (Input.GetButtonDown("Dash Player 2"))
        {
            switch (currentHighlightPlayerRight)
            {
                case 0:
                    gameManager.OnClickPlayerOrNPC2();
                    return;

                case 3:
                    gameManager.OnClickRandomRight();
                    return;

                case 4:
                    gameManager.OnClickReadyRight();
                    return;
            }
        }

    }



    //set the highlight colours
    private void UpdateHighlightedField()
    {
        //left
        if (gameManager.ReadyLeft)
        {
            readyButtonLeft.gameObject.GetComponentInParent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        }

        //right
        if (gameManager.ReadyRight)
        {
            readyButtonRight.gameObject.GetComponentInParent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        }

        for (int i = 0; i < highlightColoursLeft.Length; i++)
        {
            if (i == currentHighlightPlayerLeft)
            {
                highlightColoursLeft[i].color = new Color(0, 0, 0, 0.7f);
            }
            else
                highlightColoursLeft[i].color = defaultColor;
        }

        for (int i = 0; i < highlightColoursRight.Length; i++)
        {
            if (i == currentHighlightPlayerRight)
            {
                highlightColoursRight[i].color = new Color(0, 0, 0, 0.7f);
            }
            else
                highlightColoursRight[i].color = defaultColor;
        }
    }


}
