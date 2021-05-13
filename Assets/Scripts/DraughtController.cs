using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DraughtController : MonoBehaviour
{
	[SerializeField] private FloatRef forceValue;
	[SerializeField] GameEvent draughtDeathEvent;
	[SerializeField] Material deactivatedMaterial;
	public bool isActive = true;
	private float destructionLevel = -15;

	private void FixedUpdate()
	{
		if (transform.position.y <= destructionLevel)
		{
			gameObject.transform.parent = null;
			Destroy(gameObject);
			draughtDeathEvent.Raise();
		}
	}

	public float ForceValue
	{
		get { return forceValue; }
	}

	public Material DeactivatedMaterial
	{
		get { return deactivatedMaterial; }
	}


}
