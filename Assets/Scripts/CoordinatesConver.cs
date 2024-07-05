using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinatesConver : MonoBehaviour
{
    public static Vector2[] ConvertCoords(Vector2[] coords, int AreaLen = 45)
    {
        for (int i = 0; i < coords.Length; i++)
        {
            coords[i].y = AreaLen - 1 - coords[i].y;
        }
        return coords;
    }
}