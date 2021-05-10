using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManagerScript : MonoBehaviour
{
	public enum Side { player, opponent };
	[SerializeField] private GameObject whiteParent;
	[SerializeField] private GameObject blackParent;
	[SerializeField] private GameEvent newTurnEvent;

	private float minVelocity = 0.01f;
	private Side turn = Side.player;

	private void Update()
	{
		if (!IsActivePhase())
		{
			turn = GetOpponent(turn);

		}
	}

	private bool IsActivePhase()
	{
		foreach (Transform draughtT in whiteParent.transform)
		{
			if (draughtT.gameObject.GetComponent<Rigidbody>().velocity.x > minVelocity
				|| draughtT.gameObject.GetComponent<Rigidbody>().velocity.y > minVelocity
				|| draughtT.gameObject.GetComponent<Rigidbody>().velocity.z > minVelocity)
			{ return true; }
		}

		foreach (Transform draughtT in blackParent.transform)
		{
			if (draughtT.gameObject.GetComponent<Rigidbody>().velocity.x > minVelocity
				|| draughtT.gameObject.GetComponent<Rigidbody>().velocity.y > minVelocity
				|| draughtT.gameObject.GetComponent<Rigidbody>().velocity.z > minVelocity)
			{ return true; }
		}

		return false;
	}

	private Side GetOpponent(Side side)
	{
		if (side == Side.player) return Side.opponent;
		else return Side.player;
	}
}
