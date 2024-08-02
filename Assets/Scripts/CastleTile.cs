using UnityEngine;
using UnityEngine.Serialization;

public class CastleTile : Tile
{
    public Castle.Type nation;
    
    // public static bool CanGenerate(Vector2 newCastleCoords, Vector2 castleCoords1, Vector2 castleCoords2, int minSpace)
    // {
    //     Debug.Log($"Border Castle 1 Coords: {castleCoords1}");
    //     Debug.Log($"Border Castle 2 Coords: {castleCoords2}");
    //     if (castleCoords1 == Vector2.zero && castleCoords2 == Vector2.zero)
    //     {
    //         Debug.Log("Both border castles are zero. Can generate.");
    //         return true;
    //     }
    //     else if (castleCoords2 == Vector2.zero)
    //     {
    //         float distance = Mathf.Abs(newCastleCoords.x - castleCoords1.x);
    //         Debug.Log($"Horizontal Distance to Castle 1: {distance}");
    //         bool canGenerate = distance >= minSpace;
    //         Debug.Log($"Can generate: {canGenerate}");
    //         return canGenerate;
    //     }
    //     else if (castleCoords1 == Vector2.zero)
    //     {
    //         float distance = Mathf.Abs(newCastleCoords.y - castleCoords2.y);
    //         Debug.Log($"Vertical Distance to Castle 2: {distance}");
    //         bool canGenerate = distance >= minSpace;
    //         Debug.Log($"Can generate: {canGenerate}");
    //         return canGenerate;
    //     }
    //     else
    //     {
    //         float distance1 = Mathf.Abs(newCastleCoords.x - castleCoords1.x);
    //         float distance2 = Mathf.Abs(newCastleCoords.y - castleCoords2.y);
    //         Debug.Log($"Horizontal Distance to Castle 1: {distance1}");
    //         Debug.Log($"Vertical Distance to Castle 2: {distance2}");
    //         bool canGenerate = distance1 >= minSpace && distance2 >= minSpace;
    //         Debug.Log($"Can generate: {canGenerate}");
    //         return canGenerate;
    //     }
    // }
}