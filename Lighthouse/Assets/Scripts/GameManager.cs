using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	private Player player;
	public Player Player{ get { return player; } set { player = value; } }
    private InventoryManager inventoryMan;
    public InventoryManager InventoryMan { get { return inventoryMan; } }
    private DialogueManager dialogueMan;
    public DialogueManager DialogueMan { get { return dialogueMan; } }
    private EventManager eventMan;
    public EventManager EventMan { get { return eventMan; } }

    private Canvas transitionCanvas;

	void Awake(){
		if (instance == null) {
			instance = this;
			player = (Player)(GameObject.Find("Player").GetComponent<Player>());
            inventoryMan = this.gameObject.GetComponent<InventoryManager>();
            dialogueMan = this.gameObject.GetComponent<DialogueManager>();
            eventMan = this.gameObject.GetComponent<EventManager>();
            transitionCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
        } else if (instance != null) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}
	// Use this for initialization
	void Start () {
	}

    private void OnEnable()
    {
        SceneManager.LoadScene("Prologue", LoadSceneMode.Additive); //Use this when playing through game. Make Scene Manager
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene pScene, LoadSceneMode pMode)
    {
        if (pScene.name != "Template")
        {
            StartCoroutine(Example());
            SceneManager.SetActiveScene(pScene);
            GameObject spawnPoint = GameObject.Find("SpawnPoint");
            if(spawnPoint != null)
            {
                player.transform.position = spawnPoint.transform.position;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void LoadScene(string sceneName)
    {
        transitionCanvas.enabled = true;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    IEnumerator Example()
    {
        yield return new WaitForSeconds(2);
        transitionCanvas.enabled = false;
    }
}
