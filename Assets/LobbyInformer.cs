using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class LobbyInformer : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;

    private void Awake()
    {
        LobbyManager lobbyManager = FindObjectOfType<LobbyManager>();
        lobbyManager.SlotReleasedEvent += OnSlotReleased;
        lobbyManager.SlotTakenEvent += OnSlotTaken;
    }

    private void OnSlotTaken(ulong clientId, int lobbySlotIndex)
    {
        OnSlotTakenEventClientRpc(clientId, lobbySlotIndex);
    }

    private void OnSlotReleased(ulong clientId, int lobbySlotIndex)
    {
        OnSlotReleasedEventClientRpc(clientId);
    }

    [ClientRpc]
    private void OnSlotTakenEventClientRpc(ulong clientId, int lobbySlotIndex)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            tmp.text = new string("You Are Player " + (lobbySlotIndex + 1).ToString());
        }
    }

    [ClientRpc]
    private void OnSlotReleasedEventClientRpc(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            tmp.text = new string("Choose Slot ");
        }
    }
    
}