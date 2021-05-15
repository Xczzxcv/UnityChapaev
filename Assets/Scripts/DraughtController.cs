using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DraughtController : MonoBehaviour
{
	[SerializeField] private FloatRef forceValue;
	[SerializeField] GameEvent draughtDeathEvent;
	[SerializeField] Material deactivatedMaterial;
	[SerializeField] private FloatRef minVelocity;
	[SerializeField] IntRef activeDraughtID;
	
	[System.NonSerialized] public bool isActive = true;
	private float destructionLevel = -15;
	private IEnumerator checkingCoroutine;

	private void FixedUpdate()
	{
		if (transform.position.y <= destructionLevel)
		{
			gameObject.transform.parent = null;
			if (checkingCoroutine != null) StopCoroutine(checkingCoroutine);
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

	private void OnCollisionEnter(Collision collision)
	{
		// if collide with another draught — change material
		if (collision.collider.gameObject.layer == gameObject.layer
			&& activeDraughtID.Value == gameObject.GetInstanceID())
		{
			SetDeactivatedMaterial();
		} 
	}

	public void CheckDraught(Func<IEnumerator> coroutine)
	{
		if (checkingCoroutine != null) StopCoroutine(checkingCoroutine);

		checkingCoroutine = coroutine();
		StartCoroutine(checkingCoroutine);
	}
	public IEnumerator WaitUntilStopAndChangeMaterial()
	{
		yield return new WaitUntil(
			() => GetComponent<Rigidbody>().velocity.magnitude < minVelocity.Value);
		SetDeactivatedMaterial();
	}

	public void SetDeactivatedMaterial()
	{
		GetComponent<MeshRenderer>().material = deactivatedMaterial;
	}
}
