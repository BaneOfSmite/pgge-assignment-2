using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoints : MonoBehaviour {
    [SerializeField]
    List<Transform> SpawnPoints = new List<Transform>();

    public Transform GetSpawnPoint() {
        int index = Random.Range(0, SpawnPoints.Count);
        return SpawnPoints[index];
    }

    public Vector3 GetSpawnPointPosition() {
        return GetSpawnPoint().position;
    }

    public Quaternion GetSpawnPointRotation() {
        return GetSpawnPoint().rotation;
    }
}
