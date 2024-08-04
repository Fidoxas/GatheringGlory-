using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkBehaviour
{
    [SerializeField] private LobbySlot[] lobbySlots;
    public NetworkVariable<int> countToStart = new NetworkVariable<int>(5);
    private PlayersAssign _playersAssign;
    public event Action<ulong, int> SlotReleasedEvent;
    public event Action<ulong, int> SlotTakenEvent;
    public event Action<string> StartGameFor ;
    private bool _startingGame;
    private void Awake()
    {
        for (int i = 0; i < lobbySlots.Length; i++)
        {
            int index = i;
            lobbySlots[index].slotIndex = i;

            lobbySlots[i].IsReadyChangedEvent += TryToStartGame;
            lobbySlots[index].takeSlotbutton.onClick.AddListener(() =>
            {
                TryTakeSlotServerRpc(NetworkManager.Singleton.LocalClientId, index);
            });
            lobbySlots[index].releaseSlotbutton.onClick.AddListener(() =>
            {
                ReleaseSlotServerRpc(NetworkManager.Singleton.LocalClientId);
            });
        }
    }
    
    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += ReleaseSlot;
    }


    private void TryToStartGame(bool obj)
    {
        if (!IsServer)
        {
            return;
        }
        if (AreAllPlayersReady())
        {
            Debug.Log("is starting event ");
            _startingGame = true;
            StartCoroutine(CountToStart());
        }
        else
        {
            _startingGame = true;
            countToStart.Value = 5;
        }
    }

    private IEnumerator CountToStart()
    {
        while (_startingGame )
        {
            countToStart.Value--;
            StartGameFor?.Invoke(countToStart.Value.ToString());
            UpdateCountdownClientRpc(countToStart.Value);
            yield return new WaitForSeconds(1f);
            if (countToStart.Value <= 0)
            {
                Debug.Log("StartGame");
                _startingGame = false;
                SceneManager.LoadScene("1v1Arena");
            }
        }
        StartGameFor?.Invoke(string.Empty);
    }

    [ClientRpc]
    private void UpdateCountdownClientRpc(int countdownValue)
    {
        StartGameFor?.Invoke(countdownValue.ToString());
    }
    private void ReleaseSlot(ulong clientId)
    {
        for (var i = 0; i < lobbySlots.Length; i++)
        {
            var slot = lobbySlots[i];
            if (slot.GetClientId() == clientId)
            {
                SlotReleasedEvent?.Invoke(clientId, i);    
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ReleaseSlotServerRpc(ulong clientId)
    {
        ReleaseSlot(clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void TryTakeSlotServerRpc(ulong clientId, int lobbySlotIndex)
    {
        var slot = lobbySlots[lobbySlotIndex];
        if (slot.IsSlotTaken())
        {
            throw new Exception("Slot is already taken");
        } 
        ReleaseSlot(clientId);
        TakeSlotI(clientId, lobbySlotIndex);
    }

    private void TakeSlotI(ulong clientId, int lobbySlotIndex)
    {
        SlotTakenEvent?.Invoke(clientId, lobbySlotIndex);
    }
    

    public bool IsPlayerReady(ulong clientId)
    {
        foreach (var slot in lobbySlots)
        {
            if (slot.GetClientId() == clientId)
            {
                return slot.IsClientReady();
            }       
        }

        throw new DataException("Should not check for non-existing clientId: " + clientId);
    }

    public bool AreAllPlayersReady()
    {
        foreach (var slot in lobbySlots)
        {
            if (!slot.IsClientReady())
            {
                return false;
            }
        }

        return true;
    }
}

/*


Ładowanie gry:
...lobby
- Gdy zakonczy sie odliczanie w lobby uruchom tylko na serwerze klase MultiplayerGameLoadingManager
- Przekierowanie wszystkich na "pusta" scene gry z ekranem ładowania
- Stworzenie mapy (w pamieci nie na scenie) - nie uzywając GameObject ani MonoBehaviour ani NetworkBehaviour
    - Stworz Grida z tileami oznaczonymi jako zamki (nieistotne jaki) i resourcy na podstawie seed'a
    - Stwórz teren i obiekty zamkow i resourcow z prefabow
    - Zaczekaj az klienci zaladuja pusta scene
    - Wysłanie clientom jak zbudować mapę oraz uruchomienie budowana mapy u klientow
        - Wyślij informacje o budowaniu terenu - czyli jak stworzyc samego mesha (może też być Prefab terenu z NetworkObject oraz ze skryptem ktory z plaskiego terenu na podstawie informacji o ksztalcie stworzy gorki i to powinno wtedy sie samo zsynchronizowac z graczami)
        - Na serwerze Spawnuj prefaby zamku i resourcow - zsynchronizuja sie u graczy
            GameObject castle = Instantiate(castlePrefab, spawnPosition, spawnRotation);
            castle.GetComponent<NetworkObject>().Spawn();
    - Stworzenie Adeptow i ustawienie ich transformu
- Pokazanie loadingu z widoczna mapą i info o oczekiwaniu na graczy
- Zaczekanie na wszystkich graczy az pojawia sie na scenie
- Odliczanie do startu rozgrywki
- Faktyczne rozpoczecie gry dla clientow

*/