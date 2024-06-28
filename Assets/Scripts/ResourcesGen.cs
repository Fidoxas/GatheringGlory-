using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using ScriptablesOBJ;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class ResourcesGen : MonoBehaviour
{
    private int _stageNum;
    private int _spacing;
    private int _stageRows;
    public ResourceDB resourceDB;

    public Stack<Terrain.TerrainResource> CreateResourcesForStage(List<Vector2> occupiedTilesCoords, int currentStageNum, int spacing, int stageRows)
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
            return GenerateGoodResource(occupiedTilesCoords, spacing);
        }
        else
        {
            return Generate2WeakResources(occupiedTilesCoords, spacing, stageRows);
        }
    }

    private Stack<Terrain.TerrainResource> Generate2WeakResources(List<Vector2> occupiedTilesCoords, int spacing, int stageRows)
    {
        Stack<Terrain.TerrainResource> terrainResources = new Stack<Terrain.TerrainResource>();

        Vector2 copperCoord = GenerateUniqueCoordinate(occupiedTilesCoords, spacing);
        Stack<Vector2> copperCoords = new Stack<Vector2>();
        copperCoords.Push(copperCoord);
        terrainResources.Push(new Terrain.TerrainResource(copperCoords, resourceDB.copper));
        var copperArea = StructureAreaChecker.TilesAround(copperCoords.ToArray(), stageRows * spacing).ToArray();

        Vector2 ironCoord = GenerateUniqueCoordinate(occupiedTilesCoords, spacing, copperArea);
        Stack<Vector2> ironCoords = new Stack<Vector2>();
        ironCoords.Push(ironCoord);
        terrainResources.Push(new Terrain.TerrainResource(ironCoords, resourceDB.iron));

        return terrainResources;
    }

    private Stack<Terrain.TerrainResource> GenerateGoodResource(List<Vector2> occupiedTilesCoords, int spacing)
    {
        Stack<Terrain.TerrainResource> terrainResources = new Stack<Terrain.TerrainResource>();
        Vector2 goldCoord = GenerateUniqueCoordinate(occupiedTilesCoords, spacing);
        Stack<Vector2> goldCoords = new Stack<Vector2>();
        goldCoords.Push(goldCoord);
        terrainResources.Push(new Terrain.TerrainResource(goldCoords, resourceDB.gold));
        return terrainResources;
    }
    
    public Terrain.TerrainResource GenerateBestResource(List<Vector2> occupiedTilesCoords, int currentStageNum, int spacing)
    {
        _stageNum = currentStageNum;
        Vector2 diamondCoordsStart = GenerateUniqueSpCoordinate(occupiedTilesCoords, spacing);
        Stack<Vector2> diamondCoords = new Stack<Vector2>();
        
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                diamondCoords.Push(new Vector2(diamondCoordsStart.x + i, diamondCoordsStart.y + j));
            }
        }

        string coordsLog = "Diamond Coords: ";
        foreach (var coord in diamondCoords)
        {
            coordsLog += "(" + coord.x + ", " + coord.y + ") "+"stagenum :" + _stageNum;
        }
        Debug.Log(coordsLog);

        Terrain.TerrainResource resource = new Terrain.TerrainResource(diamondCoords, resourceDB.diam); 

        return resource;
    }

    private Vector2 GenerateUniqueCoordinate(List<Vector2> occupiedTilesCoords, int spacing, [CanBeNull] Vector2[] existingCoord = null)
    {
        Vector2 newCoord;
        do
        {
            newCoord = new Vector2(
                Random.Range(_stageNum % _stageRows * spacing, (_stageNum % _stageRows + 1) * spacing - 1),
                Random.Range(_stageNum / _stageRows * spacing, (_stageNum / _stageRows + 1) * spacing - 1)
            );
        } while (occupiedTilesCoords.Contains(newCoord) || (existingCoord != null && existingCoord.Contains(newCoord)));

        return newCoord;
    }
    
    private Vector2 GenerateUniqueSpCoordinate(List<Vector2> occupiedTilesCoords, int spacing, [CanBeNull] Vector2[] existingCoord = null)
    {
        Vector2 newCoord;
        int definer = spacing / 2; 
        do
        {
            newCoord = new Vector2(
                Random.Range((_stageNum % _stageRows * spacing) + definer, ((_stageNum % _stageRows + 1) * spacing - 1) - definer - 1),
                Random.Range((_stageNum / _stageRows * spacing) + definer, ((_stageNum / _stageRows + 1) * spacing - 1) - definer - 1)
            );
        } while (occupiedTilesCoords.Contains(newCoord) || (existingCoord != null && existingCoord.Contains(newCoord)));

        return newCoord;
    }
}
