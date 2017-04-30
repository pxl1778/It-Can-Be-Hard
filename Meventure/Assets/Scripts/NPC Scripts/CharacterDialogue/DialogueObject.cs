using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueObject {

    public string[] textLines;//All lines of dialogue before an option, option selection text
    public DialogueObject nextObject;//Next dialogue object for an option
    public DialogueObject[] options;//Options for the dialogue
}
