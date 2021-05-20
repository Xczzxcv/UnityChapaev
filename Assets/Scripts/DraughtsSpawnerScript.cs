using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraughtsSpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject playerDummyParent;
    [SerializeField] private GameObject opponentDummyParent;
    [Space]
    [SerializeField] private GameObject whiteDraughtsPrefab;
    [SerializeField] private GameObject blackDraughtsPrefab;
    [SerializeField] private IntRef playerDraughtsColor;
    [Space]
    [SerializeField] private Transform playerDraughtsParentT;
    [SerializeField] private Transform opponentDraughtsParentT;
    [Space]
    [SerializeField] private IntRef playerDraughtsQuantity;
    [SerializeField] private IntRef opponentDraughtsQuantity;
    [Space]
    [SerializeField] private GameEvent onDraughtsSpawned;

    private void Awake()
    {
        SpawnDraughts();
    }

    public void SpawnDraughts()
	{
        GameObject playerDraughtsPrefab;
        GameObject opponentDraughtsPrefab;

        if (playerDraughtsColor.Value == 0)
        {
            playerDraughtsPrefab = whiteDraughtsPrefab;
            opponentDraughtsPrefab = blackDraughtsPrefab;

        }
		else
		{
            playerDraughtsPrefab = blackDraughtsPrefab;
            opponentDraughtsPrefab = whiteDraughtsPrefab;
        }
        foreach (Transform dummyT in playerDummyParent.transform)
        {
            Instantiate(playerDraughtsPrefab, dummyT.position, dummyT.rotation, playerDraughtsParentT);
            playerDraughtsQuantity.Variable.ApplyChange(1);
        }
        foreach (Transform dummyT in opponentDummyParent.transform)
        {
            Instantiate(opponentDraughtsPrefab, dummyT.position, dummyT.rotation, opponentDraughtsParentT);
            opponentDraughtsQuantity.Variable.ApplyChange(1);
        }

        onDraughtsSpawned.Raise();
    }
}
