using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	private Player playerInfo;
	public Player PlayerInfo{ get { return playerInfo; } set { playerInfo = value; } }
    private InventoryManager inventoryMan;
    public InventoryManager InventoryMan { get { return inventoryMan; } }
    private DialogueManager dialogueMan;
    public DialogueManager DialogueMan { get { return dialogueMan; } }
    private EventManager eventMan;
    public EventManager EventMan { get { return eventMan; } }

	void Awake(){
		if (instance == null) {
			instance = this;
			playerInfo = (Player)(GameObject.Find("Player").GetComponent<Player>());
            inventoryMan = this.gameObject.GetComponent<InventoryManager>();
            dialogueMan = this.gameObject.GetComponent<DialogueManager>();
            eventMan = this.gameObject.GetComponent<EventManager>();
        } else if (instance != null) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
