using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FireDraught : ScriptableObject
{
	[SerializeField] private BoolRef isMoveDone;
	[SerializeField] private FloatRef minActiveDraughtVelocity;

	private static float randTorqueVal = 0.4f;
	private GameObject lastMovedDraught;

	public void Fire(GameObject draught, Vector3 forceDir, float forceValue, MonoBehaviour scriptExecutor)
	{
		draught.GetComponent<Rigidbody>().AddForce(forceDir * forceValue, ForceMode.Impulse);
		draught.GetComponent<Rigidbody>().AddTorque(Random.onUnitSphere * randTorqueVal, ForceMode.Impulse);

		draught.GetComponent<DraughtController>().isActive = false;
		lastMovedDraught = draught;

		scriptExecutor.StartCoroutine(SetMoveDone());
	}

	IEnumerator SetMoveDone()
	{
		var lastMovedDraughtRigidbody = lastMovedDraught.GetComponent<Rigidbody>();
		var lastMovedDraughtController = lastMovedDraught.GetComponent<DraughtController>();

		yield return new WaitUntil(
			() => lastMovedDraughtRigidbody.velocity.magnitude > minActiveDraughtVelocity.Value
		);
		isMoveDone.Variable.SetValue(true);
		lastMovedDraughtController.CheckDraught(
			lastMovedDraughtController.WaitUntilStopAndChangeMaterial);
	}
}
