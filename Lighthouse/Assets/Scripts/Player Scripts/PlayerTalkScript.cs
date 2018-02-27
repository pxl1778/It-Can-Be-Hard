using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTalkScript : MonoBehaviour {
    [SerializeField]
    private float textSpeed = .01f;
    private bool active = false;
    private bool state = true; //true = dialog, false = option
    private int currentCharacter = 0;
    protected int currentText = 0;
    private float timer = 0;
    private float boxTimer = 0;
    private float boxDuration = 3.0f;

    [SerializeField]
    private Canvas textUI;
    private Canvas optionsUI;
    private Text text;
    private Text[] optionsArray;
    protected string[] lines;
    protected GameManager gm;

    void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //textUI = this.transform.GetComponentInChildren<Canvas>();
        
        text = GameObject.Find("ThoughtPanel").GetComponentInChildren<Text>();
        /*player.GetComponent<PlayerTalkScript>().*/StartDialogue(new string[] { gm.DialogueMan.getLine("player_prologue_1") });
    }
	
	// Update is called once per frame
	void Update () {
        handleInput();
        textAnimation();
    }

    //Moves the text across the text box.
    void textAnimation()
    {
        if (active && textUI.enabled && currentCharacter < lines[currentText].Length)
        {
            timer += Time.deltaTime;
            if (timer >= textSpeed)
            {
                text.text = lines[currentText].Substring(0, currentCharacter + 1);
                currentCharacter++;
                timer = 0;
            }
        }
    }

    //handles the input from the player for the update loop
    void handleInput()
    {
        boxTimer += Time.deltaTime;
        if(boxTimer >= boxDuration)
        {
            currentText++;
            if (currentText >= lines.Length)
            {
                EndDialogue();
            }
        }
    }

    /// <summary>
    /// Begins a text dialogue that is above the player's head and doesn't pause gameplay.
    /// </summary>
    /// <param name="pLines">An array of strings that the player will say.</param>
    public void StartDialogue(string[] pLines)
    {
        Debug.Log("Started player dialogue");
        lines = pLines;
        active = true;
        textUI.enabled = true;
    }

    /// <summary>
    /// Ends the current open dialogue the player has.
    /// </summary>
    public void EndDialogue()
    {
        active = false;
        textUI.enabled = false;
        currentCharacter = 0;
        currentText = 0;
        text.text = "";
    }
}
