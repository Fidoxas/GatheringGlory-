using System.Collections.Generic;
using ScriptablesOBJ;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayersAssign : MonoBehaviour
{
    public List<Player> players = new List<Player>();
    [SerializeField] public PlayersDB db;

    private void Awake()
    {
        // DontDestroyOnLoad(gameObject);
        // Assign();
    }
    [Button]
    public void reAssign()
    {
        Clear();
        for (int i = 0; i < db.playerDbs.Length; i++)
        {

            var playerObj = new GameObject("Player " + (i+1));
            playerObj.transform.parent = this.transform;
            var player = playerObj.AddComponent<Player>();
            players.Add(player);
            players[i].playerSo = db.playerDbs[i];
        }
    }

    private void Clear()
    {
        if (this.transform.childCount <=0)
        {
            return;
        }
        for (int i = this.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = this.transform.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
        players.Clear(); 
    }

}