using System.Collections;
using UnityEngine;
using Unity.Netcode;

// Holds per-player networked state
public class PlayerState : NetworkBehaviour
{
    public NetworkList<CardData> Hand;
    public NetworkVariable<int> Score = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<bool> SkipNextTurn = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        Hand = new NetworkList<CardData>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    public int HandCount => Hand.Count;
}
