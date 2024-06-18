using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/Resource", order = 1)]
public class Resource : ScriptableObject
{

    public Material material;
    public Type type;
    public enum Type
    {
        Copper,
        Iron,
        Gold
    }
}
