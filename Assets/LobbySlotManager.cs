// using System.Collections.Generic;
// using TMPro;
// using Unity.Netcode;
// using UnityEngine;
//
// public class LobbySlotManager : NetworkBehaviour
// {
//     [SerializeField] public LobbySlot[] lobbySlots;
//     [SerializeField] private TextMeshProUGUI tmpInformer;
//     private LobbySlot _occupiedSlot;
//     private int _clientid;
//
//     public static event SlotTakenHandler OnSlotTaken; 
//     public delegate void SlotTakenHandler();
//
//     private void Start()
//     {
//         if (lobbySlots == null || lobbySlots.Length == 0)
//         {
//             Debug.LogError("Lobby slots are not assigned in the inspector.");
//             return;
//         }
//
//         for (int i = 0; i < lobbySlots.Length; i++)
//         {
//             int index = i; 
//             lobbySlots[i].takeSlotbutton.onClick.AddListener(() =>
//             {
//                 Debug.Log("Taking slot: " + index); 
//                 TakeSlotServerRpc(NetworkManager.Singleton.LocalClientId, index);
//             });
//         }
//     }
//
//
//     [ServerRpc(RequireOwnership = false)]
//     private void TakeSlotServerRpc(ulong clientId, int slotIndex)
//     {
//         Debug.Log($"TakeSlotServerRpc called by client {clientId} for slot {slotIndex}.");
//     
//         _occupiedSlot = GetOccupiedSlot(clientId);
//         if (_occupiedSlot != null)
//         {
//             Debug.Log($"Client {clientId} releasing occupied slot: {_occupiedSlot.gameObject.name}");
//             ReleaseSlot(_occupiedSlot);
//             Debug.Log("Released occupied slot.");
//         }
//         else
//         {
//             Debug.Log($"Client {clientId} does not have an occupied slot.");
//         }
//         
//         var _selectedSlot = lobbySlots[slotIndex];
//         Debug.Log($"Selected slot: {_selectedSlot.gameObject.name}, isFilled: {_selectedSlot.IsFilled()}");
//     
//     
//
//         if (!_selectedSlot.IsFilled())
//         {
//             Debug.Log($"Filling selected slot: {_selectedSlot.gameObject.name}");
//             FillSlot(_selectedSlot, clientId);
//             TakeSlotClientRpc(clientId, _selectedSlot.playerNum); 
//         }
//         else
//         {
//             Debug.Log($"Slot {slotIndex} is already taken by another player.");
//         }
//     }
//
//
//
//     [ClientRpc]
//     public void TakeSlotClientRpc(ulong clientId, int playerNum)
//     {
//         InformChosenPlayer(clientId, playerNum); 
//     }
//
//     private void FillSlot(LobbySlot slot, ulong clientId)
//     {
//         slot.TakeSlot(clientId); 
//     }
//
//     public void ReleaseSlot(LobbySlot slot)
//     {
//         slot.ReleaseSlot(); 
//     }
//
//     private LobbySlot GetOccupiedSlot(ulong clientId)
//     {
//         foreach (var slot in lobbySlots)
//         {
//             Debug.Log(clientId);
//             if (slot.IsOccupiedBy(clientId)) return slot; 
//         }
//         return null; 
//     }
//
//     private void InformChosenPlayer(ulong clientId, int playerNum)
//     {
//         if (clientId == NetworkManager.Singleton.LocalClientId)
//         {
//             tmpInformer.text = "You are Player " + playerNum;
//         }
//         else
//         {
//             tmpInformer.text = "Player " + playerNum + " has taken the slot.";
//         }
//     }
// }
