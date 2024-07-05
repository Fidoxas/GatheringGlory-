using ScriptablesOBJ;
using ScriptablesOBJ.Stages;
using UnityEngine;
using UnityEngine.Serialization;
[CreateAssetMenu(menuName = "ScriptableObjects/Castle", order = 1)]
public class Castle : ScriptableObject
{
    public Vector2[] castleCords;
    public Nation nation;
    public GameObject castleObj;
    public Material mat;
    public enum Type
    {
        None = -1,
        Stellar,
        Inferno,
        Fae,
    }
}