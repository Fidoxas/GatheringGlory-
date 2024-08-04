using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraMan : MonoBehaviour
{
    [SerializeField] private GameObject _adept;
    private ClientManager _client;
    public CinemachineFreeLook cinemachine;
    [SerializeField] private Joystick joystick;

    private void Start()
    {
        if (joystick == null)
        {
            joystick = FindObjectOfType<Joystick>();
        }
        _client = GetComponent<ClientManager>();
        
        // Subscribe to the player assigned event
        PlayerSelection playerSelection = FindObjectOfType<PlayerSelection>();
        if (playerSelection != null)
        {
            playerSelection.OnPlayerAssigned += HandlePlayerAssigned;
        }
    }

   private void HandlePlayerAssigned(int playerNumber)
    {
        Bound();
    }

    private void Bound()
    {
        _adept = _client.MyPlayer.Adept;
        if (_adept != null)
        {
            cinemachine.Follow = _adept.transform;
            cinemachine.LookAt = _adept.transform;
            _adept.GetComponent<Pmove1>()._joystick = joystick;
            _adept.GetComponent<Pmove1>().movingActive = true;
        }
        else
        {
            Debug.Log("NIE MA ADEPTA");
        }
    }
}