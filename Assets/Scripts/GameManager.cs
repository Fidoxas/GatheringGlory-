using ScriptablesOBJ.Stages;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerSo[] playerSos;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public PlayerSo GetPlayerSoByIndex(int index)
    {
        if (index >= 0 && index < playerSos.Length)
        {
            return playerSos[index];
        }
        return null;
    }
}