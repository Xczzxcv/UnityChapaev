using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player Camera")]
    [SerializeField] private Vector3 playerCameraPos;
    [SerializeField] private Quaternion playerCameraRotation;
    [Header("Opponent Camera")]
    [SerializeField] private Vector3 opponentCameraPos;
    [SerializeField] private Quaternion opponentCameraRotation;
    [Space]
    [SerializeField] BoolRef isPlayerTurn;
    [SerializeField] BoolVar IsCameraToOpponentAnimOver;
    [SerializeField] string playerTurnTrigger;
    [SerializeField] string opponentTurnTrigger;

	public void SetCameraForFirstTurn()
	{
        if (isPlayerTurn.Value)
        {
            transform.position = playerCameraPos;
            transform.rotation = playerCameraRotation;
        }
        else
        {
            transform.position = opponentCameraPos;
            transform.rotation = opponentCameraRotation;
        }
    }

	public void MoveCameraForNextTurn()
	{
        GetComponent<Animator>().applyRootMotion = false;

        if (isPlayerTurn.Value) GetComponent<Animator>().SetTrigger(playerTurnTrigger);
        else GetComponent<Animator>().SetTrigger(opponentTurnTrigger);
	}

    public void CameraToOpponentAnimOver()
	{
        IsCameraToOpponentAnimOver.Value = true;
    }
}
