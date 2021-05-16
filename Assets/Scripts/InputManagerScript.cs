using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InputManagerScript : MonoBehaviour
{
	[SerializeField] private GameObject board;
	[SerializeField] private LayerMask draughtLayer;
	[Space]
	[SerializeField] private float maxAnchorDistance;
	[SerializeField] private float maxDirLineDistance;
	[Space]
	[SerializeField] private GameObject draughtAnchor;
	[SerializeField] private GameObject dirLineObj;
	[Space]
	[SerializeField] private BoolRef isPLayerTurn;
	[SerializeField] private BoolRef isMoveDone;
	[SerializeField] private BoolRef isAIThinking;
	[Space]
	[SerializeField] private GameObject playerParent;
	[SerializeField] private GameObject opponentParent;
	[Space]
	[SerializeField] private FireDraught fireDraughtScript;
	[Space]
	[SerializeField] private string menuSceneName;
	[Space]
	private GameObject ad;
	[SerializeField] private IntVar activeDraughtID;
	public GameObject ActiveDraught
	{
		get
		{
			return ad;
		}

		private set
		{
			ad = value;
			if (ad != null) activeDraughtID.Value = ad .GetInstanceID();
		}
	}

	private Camera mainCamera;
	private Image anchorImg;
	private LineRenderer dirLine = null;
	private Vector3 lastAnchorShiftNormWorld;
	private float dirLineCoeff;
	private float dirLineCoeffMin = 0.1f;
	private bool leftBtnPressed = false;
	private LayerMask boardLayerMask;
	private Plane boardPlane;
	private GameObject lastMovedDraught;

	private void Start()
	{
		mainCamera = Camera.main;
		anchorImg = draughtAnchor.GetComponent<Image>();
		boardLayerMask = LayerMask.GetMask(LayerMask.LayerToName(board.layer));

		SetupBoardPlane();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(1) && leftBtnPressed)
		{
			CancelDrag();
		}
		else if (Input.GetMouseButtonDown(0))
		{
			ClickDraught();
		}
		else if (Input.GetMouseButton(0) && anchorImg.enabled)
		{
			DragAnchor();
		}
		else if (Input.GetMouseButtonUp(0) && ActiveDraught && leftBtnPressed)
		{
			FireDraughtFunc();
		}
		else if (Input.GetKey(KeyCode.Escape))
		{
			SceneManager.LoadScene(menuSceneName);
		}
	}

	private void ClearDraught()
	{
		anchorImg.enabled = false;
		ActiveDraught = null;

		if (dirLine != null)
		{
			dirLine.SetPosition(1, Vector3.zero);
/*			dirLine = null;
*/		}
	}

	private GameObject GetClickPosOnDraught()
	{
		if (EventSystem.current.IsPointerOverGameObject()) return null;

		Ray clickRay = mainCamera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(clickRay, out RaycastHit hitDraught, mainCamera.transform.position.y * 10, draughtLayer))
		{
			return hitDraught.collider.gameObject;
		}
		else return null;
	}

	private Vector3 GetClickPosOnBoard()
	{
		Ray clickRay = mainCamera.ScreenPointToRay(Input.mousePosition);
		if (boardPlane.Raycast(clickRay, out float distanceToPlane))
		{
			return clickRay.GetPoint(distanceToPlane);
		}
		else return Vector3.zero;
	}

	private void DragAnchor()
	{
		Vector3 draughtScrPos = mainCamera.WorldToScreenPoint(ActiveDraught.transform.position);
		draughtScrPos.z = 0;
		Vector3 clickBoardPos = GetClickPosOnBoard();
		if (clickBoardPos == Vector3.zero) return;
		clickBoardPos.y = ActiveDraught.transform.position.y;

		Vector3 anchorShiftScreen = Input.mousePosition - draughtScrPos;
		Vector3 anchorShiftWorld = clickBoardPos - ActiveDraught.transform.position;

		if (anchorShiftScreen.magnitude > maxAnchorDistance)
		{
			draughtAnchor.transform.position = draughtScrPos + anchorShiftScreen.normalized * maxAnchorDistance;
		}
		else draughtAnchor.transform.position = Input.mousePosition;

		dirLineCoeff = Mathf.Min(anchorShiftScreen.magnitude / maxAnchorDistance, 1);
		Vector3 dirLineEndPos = -anchorShiftWorld.normalized * dirLineCoeff * maxDirLineDistance;
		dirLine.SetPosition(1, dirLineEndPos);
		lastAnchorShiftNormWorld = anchorShiftWorld.normalized;
	}

	private void ClickDraught()
	{
		leftBtnPressed = true;

		GameObject draught = GetClickPosOnDraught();
		if (draught != null && !isMoveDone && !isAIThinking 
			&& IsDraughtAvailableInTurn(draught) 
			&& draught.GetComponent<DraughtController>().isActive)
		{
			ActiveDraught = draught;
			Vector3 dirLinePos = draught.transform.position;
			dirLine.transform.position = dirLinePos;
			anchorImg.enabled = true;
		}
	}

	private void FireDraughtFunc()
	{
		leftBtnPressed = false;

		if (dirLineCoeff > dirLineCoeffMin)
		{
			Vector3 forceVector = -lastAnchorShiftNormWorld;
			fireDraughtScript.Fire(ActiveDraught, forceVector * dirLineCoeff, this);

/*			ActiveDraught.GetComponent<Rigidbody>().AddForce(
				forceVector * ActiveDraught.GetComponent<DraughtController>().ForceValue * dirLineCoeff,
				ForceMode.Impulse
			);
			ActiveDraught.GetComponent<Rigidbody>().AddTorque(UnityEngine.Random.onUnitSphere * randTorqueVal, ForceMode.Impulse);
			ActiveDraught.GetComponent<DraughtController>().isActive = false;
			lastMovedDraught = ActiveDraught;

			StartCoroutine(SetMoveDone());
*/
		}

		ClearDraught();
	}

/*	IEnumerator SetMoveDone()
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
*/
	private void SetupBoardPlane()
	{
		Vector3 highPoint = board.transform.position;
		highPoint.y = 10;

		dirLine = dirLineObj.GetComponent<LineRenderer>();
		dirLine.SetPosition(1, Vector3.zero);

		Ray testRay = new Ray(highPoint, Vector3.down);

		if (Physics.Raycast(testRay, out RaycastHit hitBoard, 100, boardLayerMask))
		{
			boardPlane = new Plane(Vector3.up, hitBoard.point);
		}
		else
		{
			UnityException e = new UnityException("WRONG MATH");
			throw e;
		};
		// fallback for boardPlane
		// float y = 0.9f;
		// boardPlane = new Plane(new Vector3(-1, y, 1), new Vector3(0, y, 2), new Vector3(-2, y, 3));

	}
	
	public void CancelDrag()
	{
		leftBtnPressed = false;
		ClearDraught();
	}

	private bool IsDraughtAvailableInTurn(GameObject draught)
	{
		return (isPLayerTurn.Value && draught.transform.parent.gameObject == playerParent)
			|| (!isPLayerTurn.Value && draught.transform.parent.gameObject == opponentParent);
	}

/*	private void SetActiveDraught(GameObject newObj)
	{
		activeDraught = newObj;
		activeDraughtID.Value = activeDraught.GetInstanceID();
	}
*/}
