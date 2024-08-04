using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LobbyReadyMarker : NetworkBehaviour
{
    [SerializeField] private LobbySlot lobbySlot;
    [SerializeField] public Button button;
     Image image;

     private void Awake()
    {
        lobbySlot.IsReadyChangedEvent += LobbySlotOnIsReadyChangedEvent;
        lobbySlot.OccupiedByClientIdChangedEvent += LobbySlotOnOccupiedByClientIdChangedEvent;
        image = GetComponent<Image>();
        button.onClick.AddListener(() => RequestChangeReadyStatusServerRpc());
        button.interactable = false;
    }

     private void LobbySlotOnOccupiedByClientIdChangedEvent(ulong clientId)
     {
         Debug.Log("LobbySlotOnOccupiedByClientIdChangedEvent");
         Debug.Log($"ClientID: {clientId.ToString()} OwnerClientId: {OwnerClientId.ToString()} Network: {NetworkManager.Singleton.LocalClientId.ToString()}");
         if (lobbySlot.IsSlotTaken() && NetworkManager.Singleton.LocalClientId == clientId)
         {
             Debug.Log("Setting as interactable");
             button.interactable = true;
         }
         else
         {
             Debug.Log("Setting as non interactable");
             button.interactable = false;
         }
     }

     private void LobbySlotOnIsReadyChangedEvent(bool isReady)
     {
         if (isReady)
         {
             image.color = Color.green;
         }
         else
         {
             image.color = Color.red;
         }
     }

     [ServerRpc(RequireOwnership = false)]
     public void RequestChangeReadyStatusServerRpc(ServerRpcParams rpcParams = default)
     {
         lobbySlot.ToggleIsReady();
     }

}
