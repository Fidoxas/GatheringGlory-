using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Sirenix.OdinInspector;

public class PlayerSelection : MonoBehaviour
{
    [SerializeField] private PlayersAssign playersAssign;
    public Button hostButton;
    public Button clientButton;
    public Button startServerButton;

    public delegate void PlayerAssignedEventHandler(int playerNumber);
    public event PlayerAssignedEventHandler OnPlayerAssigned;

    void Start()
    {
        Debug.Log("Player Selection Initialized.");
        hostButton.onClick.AddListener(() => StartHost());
        clientButton.onClick.AddListener(() => StartClient());
        startServerButton.onClick.AddListener(() => StartServer());
    }

    public void SelectPlayer(int playerNumber)
    {
        if (playerNumber == 1)
        {
            StartHost();
        }
        else if (playerNumber == 2)
        {
            StartClient();
        }
    }

    private void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        this.gameObject.SetActive(false);
        AssignPlayer(1);
    }

    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        this.gameObject.SetActive(false);
        AssignPlayer(1);
    }

    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();        
        this.gameObject.SetActive(false);
        AssignPlayer(1);
    }
    [Button]
    private void AssignPlayer(int playerNumber)
    {
        Debug.LogError("JD");
        OnPlayerAssigned?.Invoke(playerNumber);
    }
}