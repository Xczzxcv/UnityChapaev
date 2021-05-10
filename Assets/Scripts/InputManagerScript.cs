using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManagerScript : MonoBehaviour
{
	[SerializeField] private GameObject board;
	[SerializeField] private LayerMask draughtLayer;
	[SerializeField] private float maxAnchorDistance;
	[SerializeField] private float maxDirLineDistance;
	[SerializeField] private GameObject draughtAnchor;
	[SerializeField] private GameObject dirLineObj;

	private Camera mainCamera;
	private Image anchorImg;
	private GameObject activeDraught = null;
	private LineRenderer dirLine = null;
	private Vector3 lastAnchorShiftNormWorld;
	private float dirLineCoeff;
	private float dirLineCoeffMin = 0.1f;
	private bool leftBtnPressed = false;
	private LayerMask boardLayerMask;
	private Plane boardPlane;
	private List<GameObject> usedDraughts = new List<GameObject>();
	private float randTorqueVal = 0.4f;

	private void Start()
	{
		mainCamera = Camera.main;
		anchorImg = draughtAnchor.GetComponent<Image>();
		boardLayerMask = LayerMask.GetMask(LayerMask.LayerToName(board.layer));

		Vector3 highPoint = board.transform.position;
		highPoint.y = 10;

		dirLine = dirLineObj.GetComponent<LineRenderer>();
		dirLine.SetPosition(1, Vector3.zero);

		Ray testRay = new Ray(highPoint, Vector3.down);
		Debug.DrawRay(testRay.origin, testRay.direction, Color.red, 10);

		if (Physics.Raycast(testRay, out RaycastHit hitBoard, 100, boardLayerMask))
		{
			Debug.Log($"Plane dot: {hitBoard.point}");
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

	private void Update()
	{
		if (Input.GetMouseButtonDown(1) && leftBtnPressed)
		{
			Debug.Log("GetMouseButtonDown(1)");
			leftBtnPressed = false;
			ClearDraught();
		}
		else if (Input.GetMouseButtonDown(0))
		{
			leftBtnPressed = true;

			GameObject draught = GetClickPosOnDraught();
			if (draught != null && !usedDraughts.Contains(draught))
			{
				activeDraught = draught;
/*				dirLine = activeDraught.transform.GetChild(0).GetComponent<LineRenderer>();
*/				
				Vector3 dirLinePos = draught.transform.position;
				dirLine.transform.position = dirLinePos;
				anchorImg.enabled = true;
			}
		}
		else if (Input.GetMouseButton(0) && anchorImg.enabled)
		{
			Vector3 draughtScrPos = mainCamera.WorldToScreenPoint(activeDraught.transform.position);
			draughtScrPos.z = 0;
			Vector3 clickBoardPos = GetClickPosOnBoard();
			if (clickBoardPos == Vector3.zero) return;
			clickBoardPos.y = activeDraught.transform.position.y;

			Vector3 anchorShiftScreen = Input.mousePosition - draughtScrPos;
			Vector3 anchorShiftWorld = clickBoardPos - activeDraught.transform.position;

			if (anchorShiftScreen.magnitude > maxAnchorDistance)
			{
				draughtAnchor.transform.position = draughtScrPos + anchorShiftScreen.normalized * maxAnchorDistance;
			}
			else draughtAnchor.transform.position = Input.mousePosition;

			dirLineCoeff =  Mathf.Min(anchorShiftScreen.magnitude / maxAnchorDistance, 1);
			Vector3 dirLineEndPos = -anchorShiftWorld.normalized * dirLineCoeff * maxDirLineDistance;
			dirLine.SetPosition(1, dirLineEndPos);
			lastAnchorShiftNormWorld = anchorShiftWorld.normalized;
		}
		else if (Input.GetMouseButtonUp(0) && activeDraught && leftBtnPressed)
		{
			leftBtnPressed = false;

			if (dirLineCoeff > dirLineCoeffMin)
			{
				Vector3 forceVector = -lastAnchorShiftNormWorld;
				activeDraught.GetComponent<Rigidbody>().AddForce(
					forceVector * activeDraught.GetComponent<DraughtController>().ForceValue * dirLineCoeff, 
					ForceMode.Impulse
				);
				activeDraught.GetComponent<Rigidbody>().AddTorque(Random.onUnitSphere * randTorqueVal, ForceMode.Impulse);

				Material newMaterial = activeDraught.GetComponent<DraughtController>().DeactivatedMaterial;
				activeDraught.GetComponent<MeshRenderer>().material = newMaterial;
				usedDraughts.Add(activeDraught);
			}

			ClearDraught();
		}
	}

	private void ClearDraught()
	{
		anchorImg.enabled = false;
		activeDraught = null;
		if (dirLine != null)
		{
			dirLine.SetPosition(1, Vector3.zero);
/*			dirLine = null;
*/		}
	}

	private GameObject GetClickPosOnDraught()
	{
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
}
