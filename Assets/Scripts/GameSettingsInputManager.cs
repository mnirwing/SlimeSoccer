using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsInputManager : MonoBehaviour
{

    private Canvas teamSelectCanvas;
    private GameManager gameManager;


    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Image[] highlightColours;


    private Color defaultColor;

    private int currentHighlight;


    // Use this for initialization
    void Start()
    {
        teamSelectCanvas = FindObjectOfType<Canvas>();
        gameManager = FindObjectOfType<GameManager>();

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
                currentHighlight++;

                if (currentHighlight == highlightColours.Length)
                    currentHighlight = 0;
            }

            if (Input.GetAxisRaw("Vertical Player 1") == 1)
            {
                currentHighlight--;

                if (currentHighlight < 0)
                    currentHighlight = highlightColours.Length - 1;
            }

            UpdateHighlightedField();
        }

        //highlighting of menu right---------------------------------------------------------------
        if (Input.GetButtonDown("Vertical Player 2"))
        {
            if (Input.GetAxisRaw("Vertical Player 2") == -1)
            {
                currentHighlight++;

                if (currentHighlight == highlightColours.Length)
                    currentHighlight = 0;
            }

            if (Input.GetAxisRaw("Vertical Player 2") == 1)
            {
                currentHighlight--;

                if (currentHighlight < 0)
                    currentHighlight = highlightColours.Length - 1;
            }

            UpdateHighlightedField();
        }

        //flaggen und farben wahl mit horizontalem input left
        if (Input.GetButtonDown("Horizontal Player 1"))
        {
            switch (currentHighlight)
            {
                case 0:
                    if (Input.GetAxisRaw("Horizontal Player 1") == 1)
                    {
                        gameManager.OnClickChangeEndGameCondition(false);
                    }
                    else
                        gameManager.OnClickChangeEndGameCondition(true);

                    return;

                case 1:
                    if (Input.GetAxisRaw("Horizontal Player 1") == 1)
                    {
                        gameManager.OnClickChangeAmount(false);
                    }
                    else
                        gameManager.OnClickChangeAmount(true);
                    return;
            }
        }

        //flaggen und farben wahl mit horizontalem input right
        if (Input.GetButtonDown("Horizontal Player 2"))
        {
            switch (currentHighlight)
            {
                case 0:
                    if (Input.GetAxisRaw("Horizontal Player 2") == 1)
                    {
                        gameManager.OnClickChangeEndGameCondition(false);
                    }
                    else
                        gameManager.OnClickChangeEndGameCondition(true);

                    return;

                case 1:
                    if (Input.GetAxisRaw("Horizontal Player 2") == 1)
                    {
                        gameManager.OnClickChangeAmount(false);
                    }
                    else
                        gameManager.OnClickChangeAmount(true);
                    return;
            }
        }

        //ready und random auswahl left
        if (Input.GetButtonDown("Dash Player 1"))
        {
            switch (currentHighlight)
            {
                case 2:
                    gameManager.OnClickStartGame();
                    return;

            }
        }

        //ready und random auswahl right
        if (Input.GetButtonDown("Dash Player 2"))
        {
            switch (currentHighlight)
            {
                case 2:
                    gameManager.OnClickStartGame();
                    return;
            }
        }

    }



    //set the highlight colours
    private void UpdateHighlightedField()
    {

        for (int i = 0; i < highlightColours.Length; i++)
        {
            if (i == currentHighlight)
            {
                highlightColours[i].color = new Color(0, 0, 0, 0.7f);
            }
            else
                highlightColours[i].color = defaultColor;
        }
    }


}
