using UnityEngine;
using Unity.Netcode;

// Simple server-authoritative movement example using Netcode for GameObjects (NGO).
public class NetworkPlayer : NetworkBehaviour
{
    public float moveSpeed = 5f;

    // Server-authoritative replicated position
    private NetworkVariable<Vector3> netPosition = new NetworkVariable<Vector3>(
        writePerm: NetworkVariableWritePermission.Server);

    private void Start()
    {
        if (IsOwner)
        {
            // Enable local camera / input handler on owner if needed
        }
    }

    private void Update()
    {
        if (IsOwner && IsClient)
        {
            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (input.sqrMagnitude > 0f)
            {
                SubmitInputServerRpc(input);
            }
        }

        // Non-owners interpolate toward replicated server position
        if (IsClient && !IsOwner)
        {
            transform.position = Vector3.Lerp(transform.position, netPosition.Value, Time.deltaTime * 10f);
        }
    }

    [ServerRpc]
    private void SubmitInputServerRpc(Vector2 input, ServerRpcParams rpcParams = default)
    {
        // Server executes movement and writes authoritative position
        Vector3 newPos = transform.position + new Vector3(input.x, 0f, input.y) * moveSpeed * Time.deltaTime;
        transform.position = newPos;
        netPosition.Value = newPos;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            netPosition.Value = transform.position;
        }
    }
}
