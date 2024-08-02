using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObObjectPositionUpdater  : MonoBehaviour
{
    public Material terrainMaterial;
    public Transform targetObject;

    void Update()
    {
        if (terrainMaterial && targetObject)
        {
            Vector4 objectPosition = new Vector4(targetObject.position.x, targetObject.position.y, targetObject.position.z, 1.0f);
            terrainMaterial.SetVector("_ObjectPosition", objectPosition);
        }
    }
}
