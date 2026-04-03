using UnityEngine;
using Unity.Netcode;

// Simple helper to spawn a networked player prefab at join.
public class PlayerSpawner : MonoBehaviour
{
    public NetworkObject playerPrefab;
, join;

        Vector3 spawnPos = Vector3.zero + Vector3.right * (NetworkManager.Singleton.ConnectedClientsList.Count * 2);
        var obj = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        obj.SpawnAsPlayerObject(clientId);
    }
}
