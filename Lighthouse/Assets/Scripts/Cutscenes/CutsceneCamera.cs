using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCamera : MonoBehaviour {

    public void EndCutscene()
    {
        GameManager.instance.EventMan.endCutscene.Invoke();
    }

    public void ChangeFace(PlayerExpression pf) {
        switch (pf)
        {
            case PlayerExpression.SMILE:
                GameManager.instance.EventMan.changePlayerFace.Invoke(0.0f);
                break;
            case PlayerExpression.MEH:
                GameManager.instance.EventMan.changePlayerFace.Invoke(0.1f);
                break;
            case PlayerExpression.SURPRISED:
                GameManager.instance.EventMan.changePlayerFace.Invoke(0.2f);
                break;
        }
    }
}
