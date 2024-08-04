using ScriptablesOBJ.Stages;
using Sirenix.OdinInspector;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public Player MyPlayer;
    public Material material; 
    public int number;

    public delegate void MatAssignEventHandler();
    public event MatAssignEventHandler OnPlayerMatAssigned;

    private void Awake()
    {
        PlayerSelection playerSelection = FindObjectOfType<PlayerSelection>();
        if (playerSelection != null)
        {
            playerSelection.OnPlayerAssigned += HandlePlayerAssigned;
        }
    }

    private void HandlePlayerAssigned(int playerNumber)
    {
        SetSideMat(playerNumber);
    }

    public void AssignPlayer(Player player)
    {
        MyPlayer = player;
    }

    void SetSideMat(int playerNumber)
    {
        if (material != null && material.shader.name == "Custom/Teamer")
        {
            // Tworzenie nowej instancji materiału zanim dokonasz zmian
            material = new Material(material);
            Debug.Log("Setting _PlayerView for material instance to: " + playerNumber);
            material.SetInt("_PlayerView", playerNumber);
            material.SetInt("_PlayerNumber",playerNumber);
            
            OnPlayerMatAssigned?.Invoke();
        }
        else
        {
            Debug.LogError("Material is null or does not use the Custom/Teamer shader.");
        }
    }

    [Button]
    void ChangePlayerViewForInstance(int playerNumber)
    {
        if (material != null && material.shader.name == "Custom/Teamer")
        {
            Debug.Log("Setting _PlayerView for material instance to: " + playerNumber);
            material.SetInt("_PlayerView", playerNumber);
        }
    }

    public Material GetTeamMaterial()
    {
        return material;
    }
}

public class MatAssignEventArgs : System.EventArgs
{
    public int PlayerNumber { get; }

    public MatAssignEventArgs(int playerNumber)
    {
        PlayerNumber = playerNumber;
    }
}
