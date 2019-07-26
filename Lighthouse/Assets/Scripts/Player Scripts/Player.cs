using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    ACTIVE, INACTIVE, MOVING, CUTSCENE
}

public enum PlayerExpression
{
    SMILE, MEH, SURPRISED
}

public class Player : MonoBehaviour {
	private string playerName = "Luke";
    private PlayerState state = PlayerState.ACTIVE;
    public PlayerState State { get { return state; } set { Debug.Log("Setting Player state: " + value); state = value; } }

    private float camSpeedX = 0;
    private float camSpeedY = 0;

    void Awake () {
	}

    public void DeactivateCamera()
    {
        Cinemachine.CinemachineBrain brain = Camera.main.gameObject.GetComponent<Cinemachine.CinemachineBrain>();
        camSpeedX = ((Cinemachine.CinemachineFreeLook)brain.ActiveVirtualCamera).m_XAxis.m_MaxSpeed;
        camSpeedY = ((Cinemachine.CinemachineFreeLook)brain.ActiveVirtualCamera).m_YAxis.m_MaxSpeed;
        ((Cinemachine.CinemachineFreeLook)brain.ActiveVirtualCamera).m_XAxis.m_MaxSpeed = 0;
        ((Cinemachine.CinemachineFreeLook)brain.ActiveVirtualCamera).m_YAxis.m_MaxSpeed = 0;
    }

    public void ActivateCamera()
    {

        Cinemachine.CinemachineBrain brain = Camera.main.gameObject.GetComponent<Cinemachine.CinemachineBrain>();
        ((Cinemachine.CinemachineFreeLook)brain.ActiveVirtualCamera).m_XAxis.m_MaxSpeed = camSpeedX;
        ((Cinemachine.CinemachineFreeLook)brain.ActiveVirtualCamera).m_YAxis.m_MaxSpeed = camSpeedY;
    }
}
