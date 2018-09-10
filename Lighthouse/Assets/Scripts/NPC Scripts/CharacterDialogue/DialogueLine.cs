using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLine : ScriptableObject {

    public string line;
    public delegate void LineStartDelegate();
    public delegate void LineEndDelegate();
    public delegate void LineCompleteDelegate();
    public LineStartDelegate lineStart;
    public LineEndDelegate lineEnd;
    public LineCompleteDelegate lineComplete;
    public string speaker;

    public DialogueLine(string pLine, LineStartDelegate pLineStart = null, LineEndDelegate pLineEnd = null, LineCompleteDelegate pLineComplete = null, string pSpeaker = ""){
        line = GameManager.instance.DialogueMan.getLine(pLine);
        lineStart = pLineStart;
        lineEnd = pLineEnd;
        lineComplete = pLineComplete;
        speaker = pSpeaker;
    }

    public void doLineStart()
    {
        if(lineStart != null)
        {
            lineStart();
        }
    }

    public void doLineEnd()
    {
        if (lineEnd != null)
        {
            lineEnd();
        }
    }

    public void doLineComplete()
    {
        if(lineComplete != null)
        {
            lineComplete();
        }
    }
}
