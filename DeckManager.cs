using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;

// Server-authoritative deck manager. Generates 162 letter cards (A-Z distributed evenly by default),
// optional wildcards and LoseTurn cards. Provides a ServerRpc for clients to request a draw.
public class DeckManager : NetworkBehaviour
{
    public int totalLetterCards = 162; // user-specified
    public int wildcardCount = 4;
    public int loseTurnCount = 4;

    public NetworkList<CardData> Deck;

    private System.Random rng = new System.Random();

    private void Awake()
    {
        Deck = new NetworkList<CardData>(writePerm: NetworkVariableWritePermission.Server);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            InitializeDeck();
        }
    }

    private void InitializeDeck()
    {
        Deck.Clear();

        // Distribute letter cards evenly across A-Z, assign leftovers to A,B,C... as needed
        int letters = totalLetterCards;
        int baseCount = letters / 26;
        int remainder = letters % 26;

        for (int i = 0; i < 26; i++)
        {
            int count = baseCount + (i < remainder ? 1 : 0);
            char c = (char)('A' + i);
            for (int j = 0; j < count; j++)
                Deck.Add(new CardData(CardType.Letter, c));
        }

        // Add wildcards
        for (int i = 0; i < wildcardCount; i++)
            Deck.Add(new CardData(CardType.Wildcard, '\0'));

        // Add lose-turn cards
        for (int i = 0; i < loseTurnCount; i++)
            Deck.Add(new CardData(CardType.LoseTurn, '\0'));

        ShuffleDeck();

        Debug.Log($"Deck initialized: {Deck.Count} cards (letters={totalLetterCards}, wildcards={wildcardCount}, loseTurns={loseTurnCount})");
    }

    private void ShuffleDeck()
    {
        // Copy to list, shuffle, write back (server-only)
        var tmp = Deck.ToList();
        int n = tmp.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int k = rng.Next(i + 1);
            var tmpv = tmp[k];
            tmp[k] = tmp[i];
            tmp[i] = tmpv;
        }

        Deck.Clear();
        foreach (var c in tmp) Deck.Add(c);
    }

    // ServerRpc: client requests a card draw. Server removes top card and sends it back to the requesting client.
    [ServerRpc(RequireOwnership = false)]
    public void RequestDrawServerRpc(ServerRpcParams rpcParams = default)
    {
        if (!IsServer) return;

        var sender = rpcParams.Receive.SenderClientId;
        if (Deck.Count == 0)
        {
            // No cards left - notify client with a special wildcard (could be improved)
            var empty = new CardData(CardType.Wildcard, '\0');
            var emptyParms = new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { sender } } };
            ReceiveDrawClientRpc(empty, emptyParms);
            return;
        }

        CardData top = DrawTop();

        // Try to find the player's PlayerState server-side and add the card into their NetworkList hand
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(sender, out var client))
        {
            var playerObj = client.PlayerObject;
            if (playerObj != null)
            {
                var ps = playerObj.GetComponent<PlayerState>();
                if (ps != null)
                {
                    ps.Hand.Add(top);
                }
            }
        }

        // Notify the client for immediate local UI feedback
        var parms2 = new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { sender } } };
        ReceiveDrawClientRpc(top, parms2);

        Debug.Log($"Client {sender} drew {top}");
    }

    private CardData DrawTop()
    {
        if (Deck.Count == 0) throw new InvalidOperationException("Deck empty");
        var top = Deck[0];
        Deck.RemoveAt(0);
        return top;
    }

    [ClientRpc]
    private void ReceiveDrawClientRpc(CardData card, ClientRpcParams clientRpcParams = default)
    {
        // This runs on the client that was targeted. Client should forward to its PlayerState to add to hand.
        Debug.Log($"Received card from server: {card}");
        // Client-side handling (UI update / add to local hand) to be implemented in Player UI code.
    }

    // Helper: server-side peek
    public CardData PeekTop()
    {
        if (Deck.Count == 0) throw new InvalidOperationException("Deck empty");
        return Deck[0];
    }

    public int DeckCount => Deck.Count;
}
