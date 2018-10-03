using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    public static DialogueManager instance = null;

    private Dictionary<string, string> lines = new Dictionary<string, string>();
    private DialogueSystem originalDialog;
    private Dictionary<string, DialogueObject[]> packs = new Dictionary<string, DialogueObject[]>();
    private GameManager gm;
    private bool doneLoading = false;
    public bool DoneLoading { get { return doneLoading; } }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        //string dialoguePath = Application.dataPath + "/Scripts/NPC Scripts/CharacterDialogue/dialogue.txt";
        string dialoguePath = Path.Combine(Application.streamingAssetsPath, "dialogue.json");
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
            originalDialog = JsonUtility.FromJson<DialogueSystem>(jsonText);
        }
        catch (IOException e)
        {
            print(e.Message);
        }

        for (int i = 0; i < originalDialog.packs.Length; i++)
        {
            packs.Add(originalDialog.packs[i].name, new DialogueObject[originalDialog.packs[i].objects.Length]);
            for (int j = 0; j < originalDialog.packs[i].objects.Length; j++)
            {
                packs[originalDialog.packs[i].name][j] = originalDialog.packs[i].objects[j];
                lines.Add(originalDialog.packs[i].objects[j].name, originalDialog.packs[i].objects[j].text);
            }
        }
        doneLoading = true;
    }

    void Start () {
    }

    public DialogueObject[] getPack(string pPackName)
    {
        if (packs.ContainsKey(pPackName))
        {
            return packs[pPackName];
        }
        else
        {
            return null;
        }
    }

    public string getLine(string pLineName)
    {
        if (lines.ContainsKey(pLineName))
        {
            return lines[pLineName];
        }
        else
        {
            return "Line not found";
        }
    }
}
