using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/Resource", order = 1)]
public class ResourceSO : ScriptableObject
{
    public Material material;
    public Resource.Type type;
    public GameObject resourceSourcePref;
}
