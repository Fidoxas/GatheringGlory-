using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player.Numbers pNum;
    public Material material;

    void Awake()
    {
        if (material != null && material.shader.name == "Custom/Teamer")
        {
            int playerNumber = (int)pNum;
            material.SetInt("_PlayerView", playerNumber);
        }
        else
        {
            Debug.LogError("Material does not use the Custom/Teamer shader or the material is not assigned.");
        }
    }

    public Material GetTeamMaterial()
    {
        return material;
    }
}