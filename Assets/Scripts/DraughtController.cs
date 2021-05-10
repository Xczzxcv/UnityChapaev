using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraughtController : MonoBehaviour
{
	[SerializeField] private FloatRef forceValue;
	[SerializeField] GameEvent draughtDeathEvent;
	[SerializeField] Material deactivatedMaterial;
	private float destructionLevel = -10;

	public float ForceValue
	{
		get { return forceValue; }
	}

	public Material DeactivatedMaterial
	{
		get { return deactivatedMaterial; }
	}

	private void FixedUpdate()
	{
		if (transform.position.y <= destructionLevel)
		{
			gameObject.transform.parent = null;
			Destroy(gameObject);
			draughtDeathEvent.Raise();
		}
	}
}
