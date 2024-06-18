using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public string pName;
    public Material material;
    public Player.PlayersNum numberP;
    public StageType type;
    public GameObject gameObject;


    public void ApplyMaterialToTriangles()
    {
        Transform terrain = transform.Find("terrain");
        foreach (Transform tile in terrain)
        {
            // Iterujemy przez wszystkie trójkąty w kwadracie
            foreach (Transform triangle in tile)
            {
                MeshRenderer meshRenderer = triangle.GetComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = material;
            }
        }
    }

    public Stage(string pName, Material material, Player.PlayersNum numberP, StageType type, GameObject gameObject,Vector2[] castleCoords,List<Vector2> tillesCoords)
    {
        this.pName = pName;
        this.material = material;
        this.numberP = numberP;
        this.type = type;
        this.gameObject = gameObject;
    }

    public enum StageType
    {
        Special = -1,
        Stage1 = 0,
        Stage2 = 1,
        Stage3 = 2,
        Stage4 = 3,
        Stage5 = 5,
        Stage6 = 6,
        Stage7 = 7,
        Stage8 = 8
    }
    public void PrintInfo()
    {
        Debug.Log("Stage: " + pName + ", Color: " + material + ", NumberP: " + numberP + ", Type: " + type);
    }
}