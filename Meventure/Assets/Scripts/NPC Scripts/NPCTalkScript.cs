using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

abstract public class NPCTalkScript : MonoBehaviour {

    [SerializeField]
    private float textSpeed = .2f;
    private bool active = false;
    private bool state = true; //true = dialog, false = option
    private Object json;
    private int currentCharacter = 0;
    protected int currentText = 0;
    private int choice = 0;
    protected int optionNumber = -1;
    private float timer = 0;

    private Canvas textUI;
    private Canvas optionsUI;
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
        textUI = GameObject.Find("OnScreenText").GetComponent<Canvas>();
        optionsUI = GameObject.Find("DialogueOptions").GetComponent<Canvas>();
        text = textUI.GetComponentInChildren<Text>();
        optionsArray = optionsUI.GetComponentsInChildren<Text>();
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
        optionsUI.enabled = false;
        currentCharacter = 0;
        currentText = 0;
        optionNumber = -1;
        //lines = originalLines;
        //options = originalOptions;
        gm.PlayerInfo.State = PlayerState.ACTIVE;
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
                gm.PlayerInfo.State = PlayerState.INACTIVE;
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
                this.optionsUI.enabled = true;
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
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                choice--;
                if (choice < 0)
                {
                    choice = 1;
                }

                for (int i = 0; i < optionsArray.Length; i++)
                {
                    optionsArray[i].color = new Color(0, 0, 0, 0.5f);
                    if (i == choice)
                    {
                        optionsArray[i].color = new Color(0, 0, 0, 1.0f);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                choice++;
                if (choice > 1)
                {
                    choice = 0;
                }

                for (int i = 0; i < optionsArray.Length; i++)
                {
                    optionsArray[i].color = new Color(0, 0, 0, 0.5f);
                    if (i == choice)
                    {
                        optionsArray[i].color = new Color(0, 0, 0, 1.0f);
                    }
                }
            }
            if (Input.GetButtonDown("Jump"))//selecting an option
            {
                state = true;
                currentText = 0;
                currentCharacter = 0;
                optionsUI.enabled = false;
                textUI.enabled = true;
                //Debug.Log(dialog.options[choice].nextObject);
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
