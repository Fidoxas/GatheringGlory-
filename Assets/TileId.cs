using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileId : MonoBehaviour
{
    public Player.Numbers pNum;
    private PlayerManager _playerManager;
    private bool materialUpdated;
    private Material teamMaterial;

    private void UpdateMaterial()
    {
        if (materialUpdated) return;

        MeshRenderer[] meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in meshRenderers)
        {
            Material[] materials = renderer.materials; // Pobranie kopii tablicy materiałów
            for (int i = 0; i < materials.Length; i++)
            {
                Material material = materials[i];
                if (material.shader.name == "Standard")
                {
                    if (teamMaterial == null)
                    {
                        if (_playerManager == null)
                        {
                            _playerManager = FindObjectOfType<PlayerManager>();
                        }
                        if (_playerManager != null)
                        {
                            teamMaterial = _playerManager.GetTeamMaterial();
                        }
                    }

                    if (teamMaterial != null)
                    {
                        materials[i] = teamMaterial;
                    }
                }

                if (materials[i].shader.name == "Custom/Teamer")
                {
                    materials[i].SetInt("_PlayerNumber", (int)pNum);
                }
            }
            renderer.materials = materials;
        }

        materialUpdated = true; 
    }

    private void Start()
    {
        UpdateMaterial();
    }
}
