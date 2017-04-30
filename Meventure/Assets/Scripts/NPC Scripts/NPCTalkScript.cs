using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

abstract public class NPCTalkScript : MonoBehaviour {

    [SerializeField]
    private string dialoguePath;
    [SerializeField]
    private float textSpeed = .2f;
    private bool active = false;
    private bool state = true; //true = dialog, false = option
    private Object json;
    private DialogueObject originalDialog;
    private DialogueObject dialog;
    private int currentCharacter = 0;
    private int currentText = 0;
    private float timer = 0;

    private Canvas textUI;
    private Canvas optionsUI;
    private Text text;

	// Use this for initialization
	void Start () {
        textUI = GameObject.Find("OnScreenText").GetComponent<Canvas>();
        optionsUI = GameObject.Find("DialogueOptions").GetComponent<Canvas>();
        text = textUI.GetComponentInChildren<Text>();
        dialoguePath = Application.dataPath + "/Scripts/NPC Scripts/CharacterDialogue/" + dialoguePath;
        string jsonText = "";
        //reading in the file
        try
        {
            StreamReader sr = new StreamReader(dialoguePath);
            using (sr)
            {
                while (sr.Peek() >= 0)
                {
                    jsonText += sr.ReadLine();
                }
            }
            originalDialog = JsonUtility.FromJson<DialogueObject>(jsonText);
            dialog = originalDialog;
        }
        catch(IOException e)
        {
            print(e.Message);
        }
    }
	
	// Update is called once per frame
	void Update () {
        handleInput();
        textAnimation();
    }

    //entering the NPC's collider
    void OnTriggerEnter(Collider col)
    {
        //allows the player to press the jump button to interact
        if(col.gameObject.tag == "Player")
        {
            active = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {//makes the text window go away
            active = false;
            textUI.enabled = false;
            currentCharacter = 0;
            currentText = 0;
        }
    }

    //handles the input from the player for the update loop
    void handleInput()
    {
        if (active && Input.GetButtonDown("Jump"))//if jump button is pressed and player is next to npc
		{
            if (state)
            {
                if (textUI.enabled && currentCharacter < originalDialog.textLines[currentText].Length - 1)
                {//if the text hasn't finished, the player can skip to display the whole text.
                    text.text = originalDialog.textLines[currentText];
                    currentCharacter = originalDialog.textLines[currentText].Length;
                }
                else if (currentText < originalDialog.textLines.Length - 1)
                {//if there's more text lines left
                    if (textUI.enabled)
                    {//go onto the next text
                        currentText++;
                    }
                    currentCharacter = 0;
                    textUI.enabled = true;
                }
                else if (dialog.options != null)
                {
                    this.optionsUI.enabled = true;
                    state = false;
                }
                else
                {//else close the canvas
                    textUI.enabled = false;
                    currentText = 0;
                }
            }
            else
            {

            }
        }
    }

    void textAnimation()
    {
        if (active && textUI.enabled && currentCharacter < originalDialog.textLines[currentText].Length)
        {
            timer += Time.deltaTime;
            if (timer >= textSpeed)
            {
                text.text = originalDialog.textLines[currentText].Substring(0, currentCharacter+1);
                currentCharacter++;
                timer = 0;
            }
        }
    }

    public abstract void topOption();
    public abstract void bottomOption();
}
