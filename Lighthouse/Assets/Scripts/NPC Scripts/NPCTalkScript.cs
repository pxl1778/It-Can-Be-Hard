using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

abstract public class NPCTalkScript : MonoBehaviour {

    [SerializeField]
    private float textSpeed = .01f;
    private bool active = false;
    private bool state = true; //true = dialog, false = option
    private int currentCharacter = 0;
    protected int currentText = 0;
    private int choice = 0;
    protected int optionNumber = -1;
    private float timer = 0;

    private Canvas textUI;
    private PlayerOptionsBox optionsBox;
    private Text text;
    private Text[] optionsArray;
    protected DialogueLine[] lines;
    protected DialogueLine[] originalLines;
    protected string[] options = new string[] { };
    protected string[] originalOptions = new string[] { };
    protected GameManager gm;

    // Use this for initialization
    void Start() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //textUI = GameObject.Find("OnScreenText").GetComponent<Canvas>();
        textUI = this.transform.parent.GetComponentInChildren<Canvas>();
        text = textUI.GetComponentInChildren<Text>();
        optionsBox = textUI.GetComponentInChildren<PlayerOptionsBox>();
        optionsBox.OtherBox = textUI.GetComponentInChildren<DialogueBox>().GetComponent<RectTransform>();
        optionsArray = optionsBox.GetComponentsInChildren<Text>();
        optionsBox.transform.gameObject.SetActive(false);
        secondStart();
    }

    // Update is called once per frame
    void Update() {
        handleInput();
        textAnimation();
    }

    //entering the NPC's collider
    void OnTriggerEnter(Collider col)
    {
        //allows the player to press the jump button to interact
        if (col.gameObject.tag == "Player")
        {
            active = true;
            checkWhenActivated();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {//makes the text window go away
            exitDialogue();
        }
    }

    private void exitDialogue()
    {
        active = false;
        textUI.enabled = false;
        optionsBox.transform.gameObject.SetActive(false);
        currentCharacter = 0;
        currentText = 0;
        optionNumber = -1;
        //lines = originalLines;
        //options = originalOptions;
        gm.Player.State = PlayerState.ACTIVE;
        text.text = "";
    }

    //handles the input from the player for the update loop
    void handleInput()
    {
        if (active && Input.GetButtonDown("Jump") && state)//if jump button is pressed and player is next to npc and it's a text line
        {
            if (!textUI.enabled)//starting conversation
            {
                Debug.Log("1");
                currentCharacter = 0;
                textUI.enabled = true;
                lines[currentText].doLineStart();
                gm.Player.State = PlayerState.INACTIVE;
            }
            else if (textUI.enabled && currentCharacter < lines[currentText].line.Length - 1)
            {//if the text hasn't finished, the player can skip to display the whole text.
                Debug.Log("2");
                text.text = lines[currentText].line;
                currentCharacter = lines[currentText].line.Length;
                lines[currentText].doLineEnd();
            }
            else if (currentText < lines.Length - 1)
            {//if there's more text lines left
                Debug.Log("3");
                currentText++;
                lines[currentText].doLineStart();
                currentCharacter = 0;
            }
            else if (options.Length > 0)
            {
                optionNumber++;
                this.optionsBox.transform.gameObject.SetActive(true);
                state = false;
                choice = 0;
                for (int i = 0; i < options.Length; i++)
                {
                    optionsArray[i].text = options[i];
                }
            }
            else
            {//else close the canvas
                lines[currentText].doLineComplete();
                exitDialogue();
            }
        }
        else if (active && !state)//if we are at an option
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                choice--;
                if (choice < 0)
                {
                    choice = 1;
                }

                for (int i = 0; i < optionsArray.Length; i++)
                {
                    optionsArray[i].fontStyle = FontStyle.Normal;
                    optionsArray[i].color = new Color(optionsArray[i].color.r, optionsArray[i].color.g, optionsArray[i].color.b, 0.5f);
                    if (i == choice)
                    {
                        optionsArray[i].fontStyle = FontStyle.Bold;
                        optionsArray[i].color = new Color(optionsArray[i].color.r, optionsArray[i].color.g, optionsArray[i].color.b, 1.0f);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                choice++;
                if (choice > 1)
                {
                    choice = 0;
                }

                for (int i = 0; i < optionsArray.Length; i++)
                {
                    optionsArray[i].fontStyle = FontStyle.Normal;
                    optionsArray[i].color = new Color(optionsArray[i].color.r, optionsArray[i].color.g, optionsArray[i].color.b, 0.5f);
                    if (i == choice)
                    {
                        optionsArray[i].fontStyle = FontStyle.Bold;
                        optionsArray[i].color = new Color(optionsArray[i].color.r, optionsArray[i].color.g, optionsArray[i].color.b, 1.0f);
                    }
                }
            }
            if (Input.GetButtonDown("Jump"))//selecting an option
            {
                state = true;
                currentText = 0;
                currentCharacter = 0;
                optionsBox.gameObject.SetActive(false);
                textUI.enabled = true;
                if(choice == 0)
                {
                    topOption();
                }
                else
                {
                    bottomOption();
                }
            }
        }
    }

    void textAnimation()
    {
        if (active && textUI.enabled)
        {
            //Debug.Log(originalDialog.textLines[0]);
        }
        if (active && textUI.enabled && currentCharacter < lines[currentText].line.Length)
        {
            timer += Time.deltaTime;
            if (timer >= textSpeed)
            {
                text.text = lines[currentText].line.Substring(0, currentCharacter + 1);
                currentCharacter++;
                timer = 0;
                if(currentCharacter >= lines[currentText].line.Length)
                {
                    lines[currentText].doLineEnd();
                }
            }
        }
    }

    public abstract void topOption();
    public abstract void bottomOption();
    public abstract void secondStart();
    public abstract void checkWhenActivated();
}
