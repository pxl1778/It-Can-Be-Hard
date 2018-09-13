﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

abstract public class NPCTalkScript : MonoBehaviour {

    [SerializeField]
    private float textSpeed = 0.01f;
    [SerializeField]
    protected bool turnTowardsPlayer = true;
    private bool active = false;
    private bool state = true; //true = dialog, false = option
    private int currentCharacter = 0;
    protected int currentText = 0;
    private int choice = 0;
    protected int optionNumber = -1;
    private float timer = 0;
    protected Vector3 targetRotation;
    protected Vector3 originalRotation;
    protected Vector3 originalCamPosition;

    private Canvas textUI;
    private PlayerOptionsBox optionsBox;
    private Text text;
    private Text[] optionsArray;
    protected DialogueLine[] lines;
    protected DialogueLine[] originalLines;
    protected string[] options = new string[] { };
    protected string[] originalOptions = new string[] { };
    protected GameManager gm;
    protected NPCSpeechBubble speechBubble;
    protected Cinemachine.CinemachineVirtualCamera npcCam;
    public AudioSource[] talkSounds;
    private int currentSound = 0;
    private int soundCount = 0;

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
        speechBubble = this.transform.parent.GetComponentInChildren<NPCSpeechBubble>();
        speechBubble.GetComponent<MeshRenderer>().enabled = false;
        npcCam = this.transform.parent.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();
        originalCamPosition = npcCam.transform.position;
        secondStart();
        talkSounds = this.GetComponentsInChildren<AudioSource>();
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
            speechBubble.GetComponent<MeshRenderer>().enabled = true;
            checkWhenActivated();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {//makes the text window go away
            exitDialogue();
            active = false;
            speechBubble.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    protected virtual void exitDialogue()
    {
        textUI.enabled = false;
        speechBubble.GetComponent<MeshRenderer>().enabled = true;
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
                speechBubble.GetComponent<MeshRenderer>().enabled = false;
                currentCharacter = 0;
                textUI.enabled = true;
                lines[currentText].doLineStart();
                gm.Player.State = PlayerState.INACTIVE;
                TalkedTo();
                if(npcCam != null){
                    npcCam.Priority = 11; 
                }
                if (turnTowardsPlayer)
                {
                    TurnTowardsPlayer();
                    TurnPlayerTowardsNPC();
                }
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
                if (npcCam != null)
                {
                    npcCam.Priority = 1;
                }
                exitDialogue();
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
                soundCount++;
                if(soundCount >= 5)
                {
                    talkSounds[currentSound].pitch = Random.Range(0.7f, 0.9f);
                    talkSounds[currentSound].Play();
                    timer = 0;
                    currentSound++;
                    if (currentSound >= talkSounds.Length)
                    {
                        currentSound = 0;
                    }
                    soundCount = 0;
                }
                if(currentCharacter >= lines[currentText].line.Length)
                {
                    lines[currentText].doLineEnd();
                }
            }
        }
    }

    void TurnTowardsPlayer()
    {
        Vector3 tempNormal = -Vector3.Normalize(this.transform.position - GameManager.instance.Player.transform.position);
        targetRotation = Vector3.Normalize(Vector3.ProjectOnPlane(tempNormal, new Vector3(0, 1, 0)));
        originalRotation = this.transform.forward;
        if(Vector3.Dot(this.transform.parent.transform.right, Camera.main.transform.forward) < 0)
        {
            if (npcCam.GetCinemachineComponent<Cinemachine.CinemachineTrackedDolly>() != null)
            {
                npcCam.GetCinemachineComponent<Cinemachine.CinemachineTrackedDolly>().m_PathPosition = 0;
            }
        }
        else
        {
            if(npcCam.GetCinemachineComponent<Cinemachine.CinemachineTrackedDolly>() != null)
            {
                npcCam.GetCinemachineComponent<Cinemachine.CinemachineTrackedDolly>().m_PathPosition = 1;
            }
        }
        transform.parent.GetComponentInChildren<Animator>().SetBool("Turning", true);
        StartCoroutine("LerpTowardsPlayer");
    }

    void TurnPlayerTowardsNPC()
    {
        Vector3 tempNormal = -Vector3.Normalize(GameManager.instance.Player.transform.position - this.transform.position);
        Vector3 playerTargetRotation = Vector3.Normalize(Vector3.ProjectOnPlane(tempNormal, new Vector3(0, 1, 0)));
        Vector3 playerOriginalRotation = GameManager.instance.Player.transform.forward;

        GameManager.instance.Player.GetComponentInChildren<Animator>().SetBool("Turning", true);
        StartCoroutine(LerpPlayerTowardsNPC(playerOriginalRotation, playerTargetRotation));
    }

    IEnumerator LerpTowardsPlayer()
    {
        float elapsedTime = 0.0f;
        float alpha = 0;
        float duration = 0.3f;
        Transform self = this.transform.parent.GetComponentInChildren<Animator>().transform;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            alpha = elapsedTime / duration;
            self.forward = Vector3.Lerp(originalRotation, targetRotation, alpha);
            yield return new WaitForEndOfFrame();
        }
        transform.parent.GetComponentInChildren<Animator>().SetBool("Turning", false);
    }

    IEnumerator LerpPlayerTowardsNPC(Vector3 playerOriginalRotation, Vector3 playerTargetRotation)
    {
        float elapsedTime = 0.0f;
        float alpha = 0;
        float duration = 0.3f;
        Transform target = GameManager.instance.Player.transform;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            alpha = elapsedTime / duration;
            target.forward = Vector3.Lerp(playerOriginalRotation, playerTargetRotation, alpha);
            yield return new WaitForEndOfFrame();
        }
        GameManager.instance.Player.GetComponentInChildren<Animator>().SetBool("Turning", false);
    }

    protected virtual void TalkedTo()
    {

    }

    public virtual void topOption()
    {
        state = true;
        currentText = 0;
        currentCharacter = 0;
        optionsBox.gameObject.SetActive(false);
        textUI.enabled = true;
    }
    public virtual void bottomOption()
    {
        state = true;
        currentText = 0;
        currentCharacter = 0;
        optionsBox.gameObject.SetActive(false);
        textUI.enabled = true;
    }
    public abstract void secondStart();
    public abstract void checkWhenActivated();
}
