using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using ScriptablesOBJ;
using UnityEngine;
using UnityEngine.Serialization;

public class ResourcesGen : MonoBehaviour
{
    private int _stageNum;
    private int _spacing;
    private int _stageRows;
    public ResourceDB resourceDB;
    public Stack<Terrain.TerrainResource> CreateResourcesForStage(List<Vector2> occupedTillesCords, int currentStageNum, int spacing, int stageRows)
    {
        _stageNum = currentStageNum;
        _spacing = spacing;
        _stageRows = stageRows;
        
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
            return  Generate2WeakResources(occupedTillesCords, spacing,stageRows);
        }
    }

    private Stack<Terrain.TerrainResource> Generate2WeakResources(List<Vector2> occupedTillesCords, int spacing, int stageRows)
    {
        Stack<Terrain.TerrainResource> terrainResources = new Stack<Terrain.TerrainResource>();
     
        var copperCoords = GenerateUniqueCoordinate(occupedTillesCords, spacing);
        terrainResources.Push(new Terrain.TerrainResource(copperCoords, resourceDB.copper));
        var copperArea = StructureAreaChecker.TilesAround(new Vector2[] { copperCoords }, stageRows * spacing).ToArray();
            
        var ironCoords = GenerateUniqueCoordinate(occupedTillesCords, spacing, copperArea);
        terrainResources.Push(new Terrain.TerrainResource(ironCoords, resourceDB.iron));

        return terrainResources;
    }

    private Stack<Terrain.TerrainResource> GenerateGoodResource(List<Vector2> occupedTillesCords, int spacing)
    {
        Stack<Terrain.TerrainResource> terrainResources = new Stack<Terrain.TerrainResource>();
       var goldCoords = GenerateUniqueCoordinate(occupedTillesCords, spacing);
       terrainResources.Push(new Terrain.TerrainResource(goldCoords, resourceDB.gold));
       return terrainResources;
    }

    private Vector2 GenerateUniqueCoordinate(List<Vector2> occupedTillesCords, int spacing, [CanBeNull] Vector2[] existingCoord = null)
    {
        Vector2 newCoord;
        do
        {
            newCoord = new Vector2(Random.Range(_stageNum%_stageRows* spacing,(_stageNum%_stageRows+1)* spacing - 1), Random.Range(_stageNum/_stageRows* spacing, (_stageNum/_stageRows+1)* spacing - 1));
        } while (occupedTillesCords.Contains(newCoord) || (existingCoord != null && existingCoord.Contains(newCoord)));

        return newCoord;
    }
    
}
