using System.Collections.Generic;
using System.Linq;
using ScriptablesOBJ.Stages;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class StructuresCreator : MonoBehaviour
{
    public Terrain terrain;

    // Pomocnicza funkcja do ustalania wysokości na terenie za pomocą Raycast
    // Pomocnicza funkcja do ustalania wysokości na terenie za pomocą Raycast
    private float GetHeightAtPosition(Vector3 position)
    {
        RaycastHit hit;

        // Maskowanie warstwy "Terrain" w Raycast
        int terrainLayerMask = LayerMask.GetMask("Terrain");

        if (Physics.Raycast(new Vector3(position.x, 40f, position.z), Vector3.down, out hit, Mathf.Infinity, terrainLayerMask))
        {
            return hit.point.y;
        }
        else
        {
            Debug.LogWarning("Raycast nie trafił w teren. Ustawiono domyślną wysokość.");
            return position.y; 
        }
    }


    // Tworzenie zamku
    public GameObject CreateCastle(GameObject obj, Map.Castle mapCastle, Player player, float seed)
    {
        var castleCastleTiles = mapCastle.Tiles;

        if (castleCastleTiles.Count != 9)
        {
            Debug.LogError("castleCastleCords should have exactly 9 coordinates.");
            return null;
        }

        var startPos = mapCastle.Tiles[0].Position;
        var endPos = mapCastle.Tiles[mapCastle.Tiles.Count() - 1].Position;
        terrain.MatchFieldYToTerrain(startPos, endPos);

        float centroidX = castleCastleTiles[4].Position.x;
        float centroidZ = castleCastleTiles[4].Position.z;

        Vector3 centroid = new Vector3(centroidX + 0.5f, castleCastleTiles[4].Position.y, centroidZ + 0.5f);

        // Ustawienie wysokości z terenu za pomocą Raycast
        centroid.y = GetHeightAtPosition(centroid);

        GameObject newObject = Instantiate(obj);
        newObject.transform.position = centroid;

        newObject.GetComponent<ObjSelector>().player = player;
        newObject.GetComponent<OwnerShip>().player = player;
        player.castle = newObject;

        return player.castle;
    }

    // Tworzenie źródła surowca
    public GameObject CreateResourceSource(GameObject resourcePrefab, Map.Resource mapResource, float seed)
    {
        if (mapResource == null || mapResource.Tiles.Count == 0)
        {
            Debug.LogError("Coordinates list is null or empty.");
            return null;
        }

        terrain.MatchFieldYToTerrain(mapResource.Tiles[0].Position, mapResource.Tiles[mapResource.Tiles.Count() - 1].Position);

        Vector3 total = Vector3.zero;
        foreach (var tile in mapResource.Tiles)
        {
            total += tile.Position; 
        }

        Vector3 center = total / mapResource.Tiles.Count;
        Vector3 centerPosition = new Vector3(center.x + 0.5f, center.y, center.z + 0.5f);

        centerPosition.y = GetHeightAtPosition(centerPosition);

        GameObject resourceObject = Instantiate(resourcePrefab, centerPosition, Quaternion.identity);

        return resourceObject;
    }
}
