using System;
using System.Collections.Generic;
using UnityEngine;

public class StructureAreaChecker
{
    public static List<Vector2> TilesAround(Vector2[] structure, int areaLine)
    {
        Vector2 start = new Vector2(structure[0].x - 1, structure[0].y - 1);
        int structureAreaLen = (int)Mathf.Sqrt(structure.Length) + 2;

        List<Vector2> structureArea = new List<Vector2>();

        for (int y = 0; y < structureAreaLen; y++)
        {
            for (int x = 0; x < structureAreaLen; x++)
            {
                Vector2 tileCoords = new Vector2(start.x + x, start.y + y);
                if (FitToArena(tileCoords, areaLine))
                {
                    structureArea.Add(tileCoords);
                }
            }
        }

        return structureArea;
    }

    static bool FitToArena(Vector2 coords, int areaLine)
    {
        if (coords.x < areaLine && coords.y < areaLine && coords.x >= 0 && coords.y >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}