using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Quest { DOG_TREAT }

public enum GlobalData { GLOW_RADIUS, CHARACTER_COUNTS, DOOR_ACTIVE, SPOKEN_TO_LIST, QUESTS_DONE, ONGOING_QUESTS, CURRENT_DAY }

public class Globals : MonoBehaviour {

    private Dictionary<GlobalData, object> dictionary;
    public Dictionary<GlobalData, object> Dictionary { get { return dictionary; } }
    public delegate void finishLerp();

    private void Awake()
    {
        dictionary = new Dictionary<GlobalData, object>();

        dictionary.Add(GlobalData.GLOW_RADIUS, 0.0f);
        dictionary.Add(GlobalData.CHARACTER_COUNTS, new Dictionary<Character, int>());
        dictionary.Add(GlobalData.DOOR_ACTIVE, false);
        dictionary.Add(GlobalData.SPOKEN_TO_LIST, new List<Character>());
        dictionary.Add(GlobalData.QUESTS_DONE, new List<Quest>());
        dictionary.Add(GlobalData.ONGOING_QUESTS, new Dictionary<Quest, int>());
        dictionary.Add(GlobalData.CURRENT_DAY, 0);
    }

    void Start () {
        //savedata
        GameManager.instance.EventMan.lerpGlobalValue.AddListener(StartGlobalLerp);
	}
    
    //Progresses relationship with character
    public void IncrementTalkCount(Character pCharacter)
    {
        Dictionary<Character, int> characterDict = (Dictionary<Character, int>)dictionary[GlobalData.CHARACTER_COUNTS];
        if (!characterDict.ContainsKey(pCharacter))
        {
            characterDict.Add(pCharacter, 0);
        }
        characterDict[pCharacter] += 1;
        dictionary[GlobalData.CHARACTER_COUNTS] = characterDict;
        ((List<Character>)dictionary[GlobalData.SPOKEN_TO_LIST]).Add(pCharacter);
    }

    //Adds another count to the day
    public void IncrementDay()
    {
        dictionary[GlobalData.CURRENT_DAY] = ((int)dictionary[GlobalData.CURRENT_DAY]) + 1;
    }

    public void AddCompletedQuest(Quest pQuest)
    {
        ((List<Quest>)dictionary[GlobalData.QUESTS_DONE]).Add(pQuest);
    }

    //Furthers a questline
    public void IncrementQuestStep(Quest pQuest)
    {
        Dictionary<Quest, int> questDict = (Dictionary<Quest, int>)dictionary[GlobalData.ONGOING_QUESTS];
        if (!questDict.ContainsKey(pQuest))
        {
            questDict.Add(pQuest, 0);
        }
        questDict[pQuest] += 1;

    }

    //Used when going to another day, resetting who you've talked to in a day
    public void ClearSpokenToList()
    {
        ((List<Character>)dictionary[GlobalData.SPOKEN_TO_LIST]).Clear();
    }

    public void LoadPeopleSpokenTo(string[] pArray)
    {
        List<Character> temp = new List<Character>();
        foreach(string s in pArray)
        {
            temp.Add(GetStringToCharacter(s));
        }
        dictionary[GlobalData.SPOKEN_TO_LIST] = temp;
    }

    public void LoadCharacterProgress(string[] pArray)
    {
        Dictionary<Character, int> temp = new Dictionary<Character, int>();
        foreach(string s in pArray)
        {
            string charString = s.Substring(0, s.IndexOf("_"));
            int charNum = int.Parse(s.Substring(s.IndexOf("_") + 1));
            temp.Add(GetStringToCharacter(charString), charNum);
        }
        dictionary[GlobalData.CHARACTER_COUNTS] = temp;
    }

    public void LoadDayNum(int pNum)
    {
        dictionary[GlobalData.CURRENT_DAY] = pNum;
    }

    public void LoadQuestsDone(string[] pArray)
    {
        List<Quest> temp = new List<Quest>();
        foreach(string s in pArray)
        {
            temp.Add(GetStringToQuest(s));
        }
        dictionary[GlobalData.QUESTS_DONE] = temp;
    }

    public void LoadOngoingQuests(string[] pArray)
    {
        Dictionary<Quest, int> temp = new Dictionary<Quest, int>();
        foreach (string s in pArray)
        {
            string questString = s.Substring(0, s.IndexOf("_"));
            int questNum = int.Parse(s.Substring(s.IndexOf("_") + 1));
            temp.Add(GetStringToQuest(questString), questNum);
        }
        dictionary[GlobalData.ONGOING_QUESTS] = temp;
    }

    //Returns the step in the relationship with that character
    public int GetCharacterCount(Character pCharacter)
    {
        Dictionary<Character, int> characterDict = (Dictionary<Character, int>)dictionary[GlobalData.CHARACTER_COUNTS];
        if (characterDict.ContainsKey(pCharacter))
        {
            return characterDict[pCharacter];
        }
        return 0;
    }

    //Primarily for saving data
    public string[] GetPeopleSpokenTo()
    {
        List<string> temp = new List<string>();
        List<Character> characterList = (List<Character>)dictionary[GlobalData.SPOKEN_TO_LIST];
        foreach(Character c in characterList)
        {
            temp.Add(GetCharacterToString(c));
        }
        return temp.ToArray();
    }

    //Returns string array with strings that end in numbers indicating relationship progress
    public string[] GetCharacterProgress()
    {
        List<string> temp = new List<string>();
        Dictionary<Character, int> characterDict = (Dictionary<Character, int>)dictionary[GlobalData.CHARACTER_COUNTS];
        foreach(KeyValuePair<Character, int> k in characterDict)
        {
            temp.Add(GetCharacterToString(k.Key) + "_" + k.Value);
        }
        return temp.ToArray();
    }

    //Returns string array of quest enums done as strings
    public string[] GetQuestsDone()
    {
        List<string> temp = new List<string>();
        List<Quest> quests = (List<Quest>)dictionary[GlobalData.QUESTS_DONE];
        foreach(Quest q in quests)
        {
            temp.Add(GetQuestToString(q));
        }
        return temp.ToArray();
    }

    //Returns ongoing quests as queststring_#
    public string[] GetOngoingQuests()
    {
        List<string> temp = new List<string>();
        Dictionary<Quest, int> questDict = (Dictionary<Quest, int>)dictionary[GlobalData.ONGOING_QUESTS];
        foreach(KeyValuePair<Quest, int> k in questDict)
        {
            temp.Add(GetQuestToString(k.Key) + "_" + k.Value);
        }
        return temp.ToArray();
    }

    public string GetQuestToString(Quest pQuest)
    {
        switch (pQuest)
        {
            case Quest.DOG_TREAT:
                return "dogtreat";
            default:
                return "none";
        }
    }

    public Quest GetStringToQuest(string pString)
    {
        switch (pString)
        {
            case "dogtreat":
                return Quest.DOG_TREAT;
            default:
                return Quest.DOG_TREAT;
        }
    }
    
    public string GetCharacterToString(Character pCharacter)
    {
        switch (pCharacter)
        {
            case Character.MATEO:
                return "mateo";
            case Character.SHAY:
                return "shay";
            case Character.JACE:
                return "jace";
            case Character.LUKE:
                return "luke";
            default:
                return "none";
        }
    }
    
    public Character GetStringToCharacter(string pString)
    {
        switch (pString)
        {
            case "mateo":
                return Character.MATEO;
            case "shay":
                return Character.SHAY;
            case "jace":
                return Character.JACE;
            case "luke":
                return Character.LUKE;
            default:
                return Character.MATEO;
        }
    }

    public void StartGlobalLerp(GlobalData dataName, float endLerp, float duration)
    {
        StartCoroutine(LerpGlobal(dataName, endLerp, duration));
    }

    IEnumerator LerpGlobal(GlobalData lerpData, float endLerp, float duration)
    {
        float elapsedTime = 0.0f;
        float alpha = 0;
        //if(dictionary[lerpString])//check it's the right type
        float startLerp = (float)dictionary[lerpData];
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            alpha = elapsedTime / duration;
            dictionary[lerpData] = Mathf.Lerp(startLerp, endLerp, alpha);
            yield return new WaitForEndOfFrame();
        }
    }
}
