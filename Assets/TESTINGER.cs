using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTINGER : MonoBehaviour
{
    private Material teamMaterial;
    [SerializeField] private int pNum;
    private bool materialUpdated = false;

    private void UpdateMaterial()
    {
        if (materialUpdated) return;

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null && meshRenderer.materials.Length > 0)
        {
            Material[] materials = meshRenderer.materials;

            // Sprawdzanie, czy materiał ma standardowy shader
            if (materials[0].shader.name == "Standard")
            {
                Debug.Log("Standard shader detected");

                // Pobieranie materiału z PlayerManager
                PlayerManager playerManager = FindObjectOfType<PlayerManager>();
                if (playerManager != null)
                {
                    teamMaterial = playerManager.GetTeamMaterial();
                    Debug.Log("Team material retrieved from PlayerManager");

                    // Zamiana standardowego materiału na teamMaterial
                    if (teamMaterial != null)
                    {
                        materials[0] = teamMaterial;
                        meshRenderer.materials = materials;  // Przypisanie zaktualizowanej tablicy materiałów z powrotem do renderera
                        Debug.Log("Material replaced with teamMaterial");
                    }
                    else
                    {
                        Debug.LogWarning("Team material is null");
                    }
                }
                else
                {
                    Debug.LogWarning("PlayerManager not found");
                }
            }
            else
            {
                Debug.Log("Non-standard shader detected: " + materials[0].shader.name);
            }

            materialUpdated = true;  // Zapobiega wielokrotnemu ustawianiu materiału
        }
        else
        {
            Debug.LogWarning("MeshRenderer not found or has no materials");
        }
    }

    void Start()
    {
        UpdateMaterial();
    }
}
