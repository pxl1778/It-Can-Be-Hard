using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
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
    private UIManager uiMan;
    public UIManager UIMan { get { return uiMan; } }
    private Globals globals;
    public Globals Globals { get { return globals; } }

    private Canvas transitionCanvas;
    private string previousScene = "";
    private string sceneToLoad = "";

	void Awake(){
		if (instance == null) {
			instance = this;
            if(GameObject.Find("Player") != null)
            {
                player = (Player)(GameObject.Find("Player").GetComponent<Player>());
            }
            inventoryMan = this.gameObject.GetComponent<InventoryManager>();
            dialogueMan = this.gameObject.GetComponent<DialogueManager>();
            eventMan = this.gameObject.GetComponent<EventManager>();
            transitionCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
            globals = this.gameObject.GetComponent<Globals>();
            uiMan = this.gameObject.GetComponent<UIManager>();
        } else if (instance != null) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}
	// Use this for initialization
	void Start () {
        if (GameObject.Find("Player") != null)
        {
            player = (Player)(GameObject.Find("Player").GetComponent<Player>());
        }
    }

    private void OnEnable()
    {
        //SceneManager.StartLoadScene("NeighborhoodTemplate", LoadSceneMode.Additive); //Use this when playing through game. Make Scene Manager
        //StartLoadScene("TownTemplate");
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        SceneManager.sceneUnloaded += OnLevelFinishedUnloading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        SceneManager.sceneUnloaded -= OnLevelFinishedUnloading;
    }

    void OnLevelFinishedLoading(Scene pScene, LoadSceneMode pMode)
    {
        if (!pScene.name.Contains("Template") && pScene.name != "Title" && !pScene.name.Contains("PhoneTransition"))
        {
            //StartCoroutine(Example());
            string templateName = pScene.name.Substring(0, pScene.name.Length - 1);
            if(pScene.name == "Town1Seated")
            {
                SceneManager.LoadScene("TownTemplate", LoadSceneMode.Additive);
                return;
            }
            if (SceneManager.GetSceneByName(templateName + "Template").buildIndex < 0)
            {
                SceneManager.LoadScene(templateName + "Template", LoadSceneMode.Additive);
            }
            SceneManager.SetActiveScene(pScene);
        }
        if(pScene.name == "Title")
        {
            SceneManager.LoadScene("NeighborhoodTemplate", LoadSceneMode.Additive);
            SceneManager.SetActiveScene(pScene);
        }
        else
        {
            if (Player != null) { Player.State = PlayerState.ACTIVE; }
            transitionCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
            if (transitionCanvas != null)
            {
                transitionCanvas.enabled = true;
                transitionCanvas.GetComponent<UITransition>().StartFadeIn();
            }
        }
        uiMan.ResetPauseMenu();
    }

    void OnLevelFinishedUnloading(Scene pScene)
    {

    }
	
	// Update is called once per frame
	void Update () {

    }

    public void StartLoadScene(string sceneName)
    {
        sceneToLoad = sceneName;
        if(transitionCanvas == null)
        {
            transitionCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
        }
        transitionCanvas.enabled = true;
        if(SceneManager.GetActiveScene().name == "Neighborhood0")
        {
            transitionCanvas.GetComponent<UITransition>().StartFadeOut(0.1f);
        }
        else
        {
            transitionCanvas.GetComponent<UITransition>().StartFadeOut(0.5f);
        }
    }

    public void LoadScene()
    {
        string sceneName = sceneToLoad;
        string templateName = sceneName.Substring(0, sceneName.Length - 1);
        if (SceneManager.GetSceneByName(templateName + "Template").buildIndex < 0)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

        SaveData data = new SaveData();
        data.currentScene = SceneManager.GetActiveScene().name;
        data.mateo1Count = (int)Globals.Dictionary["mateo1Count"];
        data.inventory = inventoryMan.GetAllItems();

        bf.Serialize(file, data);
        file.Close();
    }

    public string Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            Globals.Dictionary["mateo1Count"] = data.mateo1Count;
            for(int i = 0; i < data.inventory.Length; i++)
            {
                inventoryMan.AddItemToInventory(data.inventory[i], 1);
            }

            return data.currentScene;
        }
        else
        {
            return "";
        }
    }

    [Serializable]
    class SaveData
    {
        public string currentScene;
        public int mateo1Count;
        public string[] inventory;
    }

    IEnumerator Example()
    {
        yield return new WaitForSeconds(2);
        if(transitionCanvas != null)
        {
            transitionCanvas.enabled = false;
        }
    }
}
