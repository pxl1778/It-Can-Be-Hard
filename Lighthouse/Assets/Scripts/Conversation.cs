using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Conversation : MonoBehaviour {

    [SerializeField]
    private SugarShaker sugarShaker;
    [SerializeField]
    private TownChair townChair;

    [SerializeField]
    private Canvas NPCCanvas;
    [SerializeField]
    private Canvas playerCanvas;
    private Text npcText;
    private Text playerText;

    protected DialogueLine[] lines1;
    protected DialogueLine[] lines2;
    protected DialogueLine[] lines3;
    protected DialogueLine[] lines4;
    protected DialogueLine[][] lines;

    private int currentText = 0;
    private int currentLines = -1;
    private int currentCharacter = 0;
    private float timer = 0;
    private float textSpeed = .01f;

    private bool enableNext = false;
    private bool active = true;

    public AudioSource[] talkSounds;
    private int currentSound = 0;
    private int soundCount = 0;


    void Start () {
        npcText = NPCCanvas.GetComponentInChildren<Text>();
        playerText = playerCanvas.GetComponentInChildren<Text>();
        
        talkSounds = NPCCanvas.transform.parent.GetComponentsInChildren<AudioSource>();

        lines1 = new DialogueLine[] { new DialogueLine("mateo1_town1seated_1", null, null, null, "mateo"),
            new DialogueLine("player_town1seated_1", null, null, null, "player"),
            new DialogueLine("mateo1_town1seated_2", null, null, null, "mateo"),
            new DialogueLine("mateo1_town1seated_3", null, null, null, "mateo"),
            new DialogueLine("player_town1seated_2", null, null, null, "player"),
            new DialogueLine("player_town1seated_3", null, null, null, "player"),
            new DialogueLine("mateo1_town1seated_4", null, null, null, "mateo"),
            new DialogueLine("player_town1seated_4", null, null, null, "player"),
            new DialogueLine("mateo1_town1seated_5", null, null, null, "mateo"),
            new DialogueLine("player_town1seated_5", null, null, null, "player")};
        lines2 = new DialogueLine[] { new DialogueLine("mateo1_town1seated_6", null, null, null, "mateo"),
            new DialogueLine("player_town1seated_6", null, null, null, "player"),
            new DialogueLine("mateo1_town1seated_7", null, null, null, "mateo"),
            new DialogueLine("player_town1seated_7", null, null, null, "player"),
            new DialogueLine("player_town1seated_8", null, null, null, "player")};
        lines3 = new DialogueLine[] { new DialogueLine("mateo1_town1seated_8", null, null, null, "mateo"),
            new DialogueLine("player_town1seated_9", null, null, null, "player")};
        lines4 = new DialogueLine[] { new DialogueLine("mateo1_town1seated_9", null, null, null, "mateo"),
            new DialogueLine("player_town1seated_10", null, null, null, "mateo")};//add extra dialogue
        lines = new DialogueLine[][]{ lines1, lines2, lines3};
        nextLines();
	}

    void Update()
    {
        if (active)
        {
            handleInput();
            textAnimation();
        }
    }

    //Moves the text across the text box.
    void textAnimation()
    {
        if (!active)
        {
            return;
        }
        switch (lines[currentLines][currentText].speaker)
        {
            case "mateo":
                if (currentCharacter < lines[currentLines][currentText].line.Length)
                {
                    timer += Time.deltaTime;
                    if (timer >= textSpeed)
                    {
                        npcText.text = lines[currentLines][currentText].line.Substring(0, currentCharacter + 1);
                        currentCharacter++;
                        timer = 0;
                        soundCount++;
                        if (soundCount >= 5)
                        {
                            talkSounds[currentSound].pitch = Random.Range(0.65f, 0.85f);
                            talkSounds[currentSound].volume = 0.1f;
                            talkSounds[currentSound].Play();
                            timer = 0;
                            currentSound++;
                            if (currentSound >= talkSounds.Length)
                            {
                                currentSound = 0;
                            }
                            soundCount = 0;
                        }
                    }
                }
                else
                {
                    enableNext = true;
                }
                break;
            case "player":
                if (currentCharacter < lines[currentLines][currentText].line.Length)
                {
                    timer += Time.deltaTime;
                    if (timer >= textSpeed)
                    {
                        playerText.text = lines[currentLines][currentText].line.Substring(0, currentCharacter + 1);
                        currentCharacter++;
                        timer = 0;
                        soundCount++;
                        if (soundCount >= 5)
                        {
                            talkSounds[currentSound].pitch = Random.Range(0.85f, 1.0f);
                            talkSounds[currentSound].volume = 0.1f;
                            talkSounds[currentSound].Play();
                            timer = 0;
                            currentSound++;
                            if (currentSound >= talkSounds.Length)
                            {
                                currentSound = 0;
                            }
                            soundCount = 0;
                        }
                    }
                }
                else
                {
                    enableNext = true;
                }
                break;
            default:
                Debug.Log("something's wrong in conversation.cs");
                break;
        }
    }

    //handles the input from the player for the update loop
    void handleInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (enableNext)
            {
                currentText++;
                currentCharacter = 0;
                enableNext = false;
                if (currentText >= lines[currentLines].Length)
                {
                    active = false;
                    playerText.text = "";
                    npcText.text = "";
                    playerCanvas.enabled = false;
                    NPCCanvas.enabled = false;
                    switch (currentLines)
                    {
                        case 0:
                            sugarShaker.enabled = true;
                            break;
                        case 1:
                            townChair.enabled = true;
                            break;
                        case 2:
                            GameManager.instance.StartLoadScene("EndScreen");
                            break;
                        case 3:
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    //resetting texts and canvases
                    playerText.text = "";
                    npcText.text = "";
                    playerCanvas.enabled = false;
                    NPCCanvas.enabled = false;
                    switch (lines[currentLines][currentText].speaker)
                    {
                        case "mateo":
                            NPCCanvas.enabled = true;
                            break;
                        case "player":
                            playerCanvas.enabled = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public void nextLines()
    {
        currentLines++;
        currentText = 0;
        currentCharacter = 0;
        switch (lines[currentLines][currentText].speaker)
        {
            case "mateo":
                NPCCanvas.enabled = true;
                break;
            case "player":
                playerCanvas.enabled = true;
                break;
            default:
                break;
        }
        active = true;
    }
}
