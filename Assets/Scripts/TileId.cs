using System;
using System.Collections;
using System.Collections.Generic;
using ScriptablesOBJ.Stages;
using UnityEngine;

public class TileId : MonoBehaviour
{
    public PlayerSo.Numbers pNum;
    private ClientManager _clientManager;
    // public Tile.Type _type;
    public Material teamMaterial;
    private void Awake()
    {
        _clientManager = FindObjectOfType<ClientManager>();
        if (_clientManager != null)
        {
            _clientManager.OnPlayerMatAssigned += HandlePlayerAssigned;
        }
    }

    private void HandlePlayerAssigned()
    {
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        // if (_type == Tile.Type.Castle && pNum == _clientManager.MyPlayer.playerSo.pNum)
        {
            
            teamMaterial = _clientManager.material;
            foreach (Transform child in this.transform)
            {
                GameObject triangle = child.gameObject;
                triangle.GetComponent<MeshRenderer>().material = teamMaterial;
            }
        }
    }
}