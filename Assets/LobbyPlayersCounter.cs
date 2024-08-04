using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class LobbyPlayersCounter : NetworkBehaviour
{
    private int _playersInLobby = 0;
    private TextMeshProUGUI tmp;

    private void Start()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager.Singleton is null. Make sure a NetworkManager component is present in the scene.");
            return; 
        }

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        tmp = GetComponent<TextMeshProUGUI>();
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (IsServer)
        {
            _playersInLobby--;
            ReloadPlayersCounterClientRpc(_playersInLobby);
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (IsServer)
        {
            _playersInLobby++;
            ReloadPlayersCounterClientRpc(_playersInLobby);
            tmp.text = _playersInLobby + " players";
        }
    }

    [ClientRpc]
    void ReloadPlayersCounterClientRpc(int playerCount)
    {
        tmp.text = playerCount + " players";
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }
}