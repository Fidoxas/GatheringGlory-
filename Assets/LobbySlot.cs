using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbySlot : NetworkBehaviour
{
    public static ulong EMPTY_SLOT_CLIENT_ID = ulong.MaxValue;
    [SerializeField] public Button takeSlotbutton;
    [SerializeField] public Button releaseSlotbutton;

    public event Action<bool> IsReadyChangedEvent;
    public event Action<ulong> OccupiedByClientIdChangedEvent;

    private NetworkVariable<ulong> _occupiedByClientId = new(EMPTY_SLOT_CLIENT_ID);
    private NetworkVariable<bool> _isReady = new(false);

    public void ToggleIsReady()
    {
        _isReady.Value = !_isReady.Value;
    }

    public bool IsSlotTaken()
    {
        return _occupiedByClientId.Value != EMPTY_SLOT_CLIENT_ID;
    }

    public ulong GetClientId()
    {
        return _occupiedByClientId.Value;
    }

    public bool IsClientReady()
    {
        return _isReady.Value;
    }

    [HideInInspector] public int slotIndex;

    private void Awake()
    {
        LobbyManager lobbyManager = FindObjectOfType<LobbyManager>();
        lobbyManager.SlotReleasedEvent += OnSlotReleasedEvent;
        lobbyManager.SlotTakenEvent += OnSlotTakenEvent;

        _occupiedByClientId.OnValueChanged += OnOccuppiedClientIdChanged;
        _isReady.OnValueChanged += OnIsReadyChanged;
    }

    public void Release()
    {
        _occupiedByClientId.Value = EMPTY_SLOT_CLIENT_ID;
    }

    private void OnIsReadyChanged(bool previousvalue, bool newvalue)
    {
        IsReadyChangedEvent?.Invoke(newvalue);
    }

    private void OnOccuppiedClientIdChanged(ulong previousvalue, ulong newvalue)
    {
        OccupiedByClientIdChangedEvent?.Invoke(newvalue);
        if (NetworkManager.IsServer)
        {
            _isReady.Value = false;   
        }
        if (newvalue == EMPTY_SLOT_CLIENT_ID)
        {
            takeSlotbutton.interactable = true;
            releaseSlotbutton.gameObject.SetActive(false);
        }
        else
        {
            takeSlotbutton.interactable = false;
            if (_occupiedByClientId.Value == NetworkManager.Singleton.LocalClientId)
            {
                releaseSlotbutton.gameObject.SetActive(true);
            }
        }
    }

    private void OnSlotReleasedEvent(ulong clientId, int lobbySlotIndex)
    {
        if (lobbySlotIndex == slotIndex)
        {
            Release();
        }
    }

    private void OnSlotTakenEvent(ulong clientId, int lobbySlotIndex)
    {
        if (lobbySlotIndex == slotIndex)
        {
            _occupiedByClientId.Value = clientId;
        }
    }
}