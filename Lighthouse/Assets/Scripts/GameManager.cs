using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public enum Character { LUKE, MATEO, SHAY, JACE }

[System.Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(float pX, float pY, float pZ)
    {
        x = pX; y = pY; z = pZ;
    }

    public SerializableVector3(Vector3 pVector)
    {
        x = pVector.x; y = pVector.y; z = pVector.z;
    }

    public Vector3 toVector3()
    {
        return new Vector3(x, y, z);
    }
}

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
            //Starting up the game
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
        if (!pScene.name.Contains("Template") && pScene.name != "Title" && pScene.name != "EndScreen" && !pScene.name.Contains("PhoneTransition"))
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
            if(GameObject.Find("AudioManager") != null)
            {
                GameObject.Find("AudioManager").GetComponent<AudioSource>().Stop();
                Destroy(GameObject.Find("AudioManager").gameObject);
            }
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
        if(pScene.name == "Neighborhood1")
        {
            if(this.GetComponentInChildren<AudioSource>() != null)
            {
                this.GetComponentInChildren<AudioSource>().Play();
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
            transitionCanvas.GetComponent<UITransition>().StartFadeOut(0.05f);
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
        data.inventory = inventoryMan.GetAllItems();
        data.peopleSpokenTo = globals.GetPeopleSpokenTo();
        data.playerPos = new SerializableVector3(GameManager.instance.player.transform.position);
        data.characterProgress = globals.GetCharacterProgress();
        data.dayNum = (int)globals.Dictionary[GlobalData.CURRENT_DAY];
        data.questsDone = globals.GetQuestsDone();
        data.ongoingQuests = globals.GetOngoingQuests();

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

            for(int i = 0; i < data.inventory.Length; i++)
            {
                inventoryMan.AddItemToInventory(data.inventory[i], 1);
            }
            globals.LoadPeopleSpokenTo(data.peopleSpokenTo);
            globals.LoadCharacterProgress(data.characterProgress);
            globals.LoadDayNum(data.dayNum);
            globals.LoadQuestsDone(data.questsDone);
            globals.LoadOngoingQuests(data.ongoingQuests);

            return data.currentScene;
        }
        else
        {
            return "";
        }
    }

    public void ClearSaveData()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            try
            {
                File.Delete(Application.persistentDataPath + "/playerInfo.dat");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    public void EndDay()
    {
        globals.ClearSpokenToList();
        globals.IncrementDay();
    }

    [Serializable]
    class SaveData
    {
        public string currentScene;
        public string[] peopleSpokenTo;
        public SerializableVector3 playerPos;
        public string[] characterProgress;
        public string[] inventory;
        public int dayNum;
        public string[] questsDone;
        public string[] ongoingQuests;
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

