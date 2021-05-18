using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraughtsSpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject playerDummyParent;
    [SerializeField] private GameObject opponentDummyParent;
    [Space]
    [SerializeField] private GameObject playerDraughtPrefab;
    [SerializeField] private GameObject opponentDraughtPrefab;
    [Space]
    [SerializeField] private Transform playerDraughtsParentT;
    [SerializeField] private Transform opponentDraughtsParentT;
    [Space]
    [SerializeField] private IntRef playerDraughtsQuantity;
    [SerializeField] private IntRef opponentDraughtsQuantity;
    [Space]
    [SerializeField] private GameEvent onDraughtsSpawned;

    void Awake()
    {
        foreach (Transform dummyT in playerDummyParent.transform)
		{
            Instantiate(playerDraughtPrefab, dummyT.position, dummyT.rotation, playerDraughtsParentT);
            playerDraughtsQuantity.Variable.ApplyChange(1);
        }
        foreach (Transform dummyT in opponentDummyParent.transform)
		{
            Instantiate(opponentDraughtPrefab, dummyT.position, dummyT.rotation, opponentDraughtsParentT);
            opponentDraughtsQuantity.Variable.ApplyChange(1);
		}

        onDraughtsSpawned.Raise();
    }
}
