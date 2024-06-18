using System.Collections.Generic;
using UnityEngine;

public class ResourcesGen : MonoBehaviour
{
    public ScriptableObjects scriptableObjects;
    public Stack<Terrain.TerrainResource> CreateResourcesForStage(List<Vector2> occupedTillesCords, int currentStageNum, int spacing, int stageRows)
    {
        List<int> corners = new List<int>()
        {
            0,
            stageRows - 1,
            stageRows * (stageRows - 1),
            stageRows * stageRows - 1
        };


        if (corners.Contains(currentStageNum))
        {
            return GenerateGoodResource(occupedTillesCords, spacing);
        }
        else
        {
            return  Generate2WeakResources(occupedTillesCords, spacing);
        }
    }

    private Stack<Terrain.TerrainResource> Generate2WeakResources(List<Vector2> occupedTillesCords, int spacing)
    {
        Stack<Terrain.TerrainResource> terrainResources = new Stack<Terrain.TerrainResource>();
     
        var copperCoords = GenerateUniqueCoordinate(occupedTillesCords, spacing);
        terrainResources.Push(new Terrain.TerrainResource(copperCoords, scriptableObjects.copper));
        
        var ironCoords = GenerateUniqueCoordinate(occupedTillesCords, spacing, copperCoords);
        terrainResources.Push(new Terrain.TerrainResource(ironCoords, scriptableObjects.iron));

        return terrainResources;
    }

    private Stack<Terrain.TerrainResource> GenerateGoodResource(List<Vector2> occupedTillesCords, int spacing)
    {
        Stack<Terrain.TerrainResource> terrainResources = new Stack<Terrain.TerrainResource>();
       var goldCoords = GenerateUniqueCoordinate(occupedTillesCords, spacing);
       terrainResources.Push(new Terrain.TerrainResource(goldCoords, scriptableObjects.gold));
       return terrainResources;
    }

    private static Vector2 GenerateUniqueCoordinate(List<Vector2> occupedTillesCords, int spacing, Vector2? existingCoord = null)
    {
        Vector2 newCoord;
        do
        {
            newCoord = new Vector2(Random.Range(0, spacing - 1), Random.Range(0, spacing - 1));
        } while (occupedTillesCords.Contains(newCoord) || (existingCoord.HasValue && newCoord == existingCoord.Value));

        return newCoord;
    }
    
}
