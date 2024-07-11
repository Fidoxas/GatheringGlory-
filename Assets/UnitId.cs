using UnityEngine;

public class UnitId : MonoBehaviour
{
    public Player.Numbers pNum;
    private Material teamMaterial; // Materiał z shaderem "Custom/Teamer"
    private bool materialUpdated = false;
    private PlayerManager playerManager;

    public void SetSide(Player.Numbers id)
    {
        pNum = id;
        materialUpdated = false; // Umożliwia ponowną aktualizację materiału
        UpdateMaterial();
    }

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
                        if (playerManager == null)
                        {
                            playerManager = FindObjectOfType<PlayerManager>();
                        }
                        if (playerManager != null)
                        {
                            teamMaterial = playerManager.GetTeamMaterial();
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
            renderer.materials = materials; // Przypisanie zaktualizowanej tablicy materiałów z powrotem do renderera
        }

        materialUpdated = true; // Flaga oznaczająca, że materiały zostały zaktualizowane
    }

    void Start()
    {
        UpdateMaterial();
    }
}
