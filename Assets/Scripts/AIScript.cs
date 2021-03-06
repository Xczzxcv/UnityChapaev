using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

public class AIScript : MonoBehaviour
{
	[SerializeField] private GameObject playerDraughtsParent;
	[SerializeField] private GameObject opponentDraughtsParent;
	[Space]
	[SerializeField] private Transform downLeftBorderCorner;
	[SerializeField] private Transform upperRightBorderCorner;
	[Space]
	[SerializeField] private BoolRef isAIThinking;
	[SerializeField] private FireDraught fireDraughtScript;
	[Space]
	[SerializeField] private GameObject dirLineObj;
	[SerializeField] private FloatRef dirLineMaxLength;
	[Space]
	[SerializeField] private Material firingAIMat;
	[SerializeField] IntVar activeDraughtID;
	[SerializeField] private BoolVar IsCameraToOpponentAnimOver;
	[Header("AI Settings")]
	[SerializeField] private float delayBeforeFire;

	private FireOption bestOption;
	private List<FireOption> bestDraughtOptions;
	private Vector3 playerSideCenter;
	private int draughtsPerRaw = 4;
	private float maxRaycastRange;
	private int draughtsQuantityBySide;
	private RaycastHit[] raycastResults;
	private Vector3 draughtSize;
	private LayerMask draughtMask;
	private float draughtMaxForce;
	private Material changedDraughtMat;
	private int deactivatedTargetPenalty = 100;

	private void Start()
	{
		maxRaycastRange = Mathf.Abs(downLeftBorderCorner.position.x - upperRightBorderCorner.position.x);
		draughtsQuantityBySide = playerDraughtsParent.transform.childCount;
		raycastResults = new RaycastHit[draughtsQuantityBySide];
		bestDraughtOptions = new List<FireOption>();
		
		Transform draughtT = playerDraughtsParent.transform.GetChild(0);
		draughtSize = draughtT.GetComponent<Collider>().bounds.size;
		draughtMask = LayerMask.GetMask(LayerMask.LayerToName(draughtT.gameObject.layer));
		draughtMaxForce = draughtT.GetComponent<DraughtController>().ForceValue;

		FireOption.RateFunc = RateOption;

		// find center of the opposite side to calculate neareast draughts
		playerSideCenter = new Vector3(
			(downLeftBorderCorner.position.x + upperRightBorderCorner.position.x) / 2,
			downLeftBorderCorner.position.y,
			downLeftBorderCorner.position.z
			);
	}

	public void MakeMove()
	{
		Debug.Log($"MakeMove call. is AI thinking: {isAIThinking.Value}");
		if (!isAIThinking) return;

		var aliveDraughts = new List<GameObject>();
		foreach (Transform draughtT in opponentDraughtsParent.transform)
		{
			if (draughtT.GetComponent<DraughtController>().isActive)
			{
				aliveDraughts.Add(draughtT.gameObject);
			}
		}
		if (aliveDraughts.Count == 0) return;

		aliveDraughts.Sort(CompareByDistToOpponentSide);

		if (playerDraughtsParent.transform.childCount > 0) SetBestAliveOption(aliveDraughts);
		else SetRandomOption(aliveDraughts);

		StartCoroutine(ActuallyFireDraughtAfterCondition());
	}

	private IEnumerator ActuallyFireDraughtAfterCondition()
	{
		yield return new WaitUntil(() => IsCameraToOpponentAnimOver.Value);
		yield return new WaitForSeconds(delayBeforeFire);

		IsCameraToOpponentAnimOver.Value = false;

		var draughtMeshRenderer = bestOption.Draught.GetComponent<MeshRenderer>();
		changedDraughtMat = draughtMeshRenderer.material;
		draughtMeshRenderer.material = firingAIMat;
/*		Debug.DrawLine(
			bestOption.Draught.transform.position,
			bestOption.Draught.transform.position 
				+ bestOption.ForceDir * bestOption.ForceValue,
			Color.red,
			3
			);
*/
		yield return new WaitForSeconds(1.5f);

		dirLineObj.transform.position = bestOption.Draught.transform.position;
		dirLineObj.GetComponent<LineRenderer>().SetPosition(1, bestOption.ForceDir * dirLineMaxLength * (bestOption.ForceValue / draughtMaxForce));

		yield return new WaitForSeconds(1);
		
		draughtMeshRenderer.material = changedDraughtMat;

		Debug.Log("FIRE!!");
		fireDraughtScript.Fire(bestOption.Draught, bestOption.ForceDir, bestOption.ForceValue, this);
		dirLineObj.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
	}

	private int CompareByDistToOpponentSide(GameObject go1, GameObject go2)
	{
		return Mathf.RoundToInt(
			Vector3.Distance(go1.transform.position, playerSideCenter)
			- Vector3.Distance(go2.transform.position, playerSideCenter)
			); 
	}

	private void SetBestAliveOption(List<GameObject> aliveDraughts)
	{
		var bestOptions = new List<FireOption>();

		for (int i = 0; i < Mathf.Min(draughtsPerRaw, aliveDraughts.Count); ++i)
		{
			GameObject currDraught = aliveDraughts[i];
			GetBestDraughtOptions(currDraught);
			
			if (bestOptions.Count == 0) bestOptions.AddRange(bestDraughtOptions);
			else if (bestDraughtOptions[0].Rating == bestOptions[0].Rating) bestOptions.AddRange(bestDraughtOptions);
			else if (bestDraughtOptions[0].Rating > bestOptions[0].Rating)
			{
				bestOptions.Clear();
				bestOptions.AddRange(bestDraughtOptions);
			}
		}

		bestOption = bestOptions[UnityEngine.Random.Range(0, bestOptions.Count - 1)];
		activeDraughtID.Value = bestOption.Draught.GetInstanceID();
	}

	private void SetRandomOption(List<GameObject> aliveDraughts)
	{
		bestOption = new FireOption(
			aliveDraughts[UnityEngine.Random.Range(0, aliveDraughts.Count - 1)],
			Vector3.zero,
			0
			);
		activeDraughtID.Value = bestOption.Draught.GetInstanceID();
	}

	private void GetBestDraughtOptions(GameObject draught)
	{
		bestDraughtOptions.Clear();
		bestDraughtOptions.Add(FireOption.BadOption);
		foreach (Transform draughtT in playerDraughtsParent.transform)
		{		
			Vector3 currVec = draughtT.position - draught.transform.position;
			var currOption = new FireOption(draught, currVec, draughtMaxForce);

			if (!draughtT.GetComponent<DraughtController>().isActive) 
			{
				currOption.Rating -= deactivatedTargetPenalty;
			}

			if (currOption.Rating == bestDraughtOptions[0].Rating) bestDraughtOptions.Add(currOption);
			else if (currOption.Rating > bestDraughtOptions[0].Rating)
			{
				bestDraughtOptions.Clear();
				bestDraughtOptions.Add(currOption);
			}
		}
	}

	private int RateOption(GameObject draught, Vector3 force)
	{
		Ray testRay = new Ray(draught.transform.position, force);
		int hitedNum = Physics.SphereCastNonAlloc(testRay, draughtSize.x, raycastResults, maxRaycastRange, draughtMask);

		for (int i = 0; i < hitedNum; ++i)
		{
			// if hited ally draught
			if (raycastResults[i].collider.gameObject
				.transform.parent == opponentDraughtsParent) return -100;
		}

		if (hitedNum > 2) return 0;
		else if (hitedNum == 2) return 100;
		else if (hitedNum == 1) return 200;
		else return 0;
	}

	[Serializable]
	struct FireOption
	{
		public GameObject Draught { get; set; }
		public Vector3 ForceDir { get; set; }
		public float ForceValue { get; set; }
		private int _rating;
		public int Rating
		{
			get { return _rating; }
			set { _rating = value; }
		}
		public static int DefaultRating = -666;
		public static Func<GameObject, Vector3, int> RateFunc { get; set; }

		public static FireOption BadOption
		{
			get
			{
				return new FireOption
				{
					Draught = null,
					ForceDir = Vector3.zero,
					ForceValue = 0,
					_rating = DefaultRating
				};
			}
		}

		public FireOption(GameObject draught, Vector3 forceDir, float forceValue)
		{
			Draught = draught;
			ForceDir = forceDir.normalized;
			ForceValue = forceValue;
			_rating = RateFunc(draught, forceDir);
		}
	}
}
