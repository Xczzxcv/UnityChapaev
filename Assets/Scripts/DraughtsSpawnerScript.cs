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

    private void Awake()
    {
        SpawnDraughts();
    }

    public void SpawnDraughts()
	{
        Debug.Log("call spawndr");
        Debug.Log($"call spawndr {playerDummyParent.name} {opponentDummyParent.name}");
        Debug.Log($"call spawndr {playerDraughtPrefab.name} {opponentDraughtPrefab.name}");
        Debug.Log($"call spawndr {playerDraughtsParentT.name} {opponentDraughtsParentT.name}");
        Debug.Log($"call spawndr {playerDraughtsQuantity.Value} {opponentDraughtsQuantity.Value}");
        Debug.Log("end call info");

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
