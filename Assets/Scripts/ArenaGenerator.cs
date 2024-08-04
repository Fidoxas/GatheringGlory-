using System;
using System.Collections.Generic;
using System.Linq;
using ScriptablesOBJ;
using ScriptablesOBJ.Stages;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ArenaGenerator : MonoBehaviour
{
    [SerializeField] PlayersAssign playersAssign;
    [SerializeField] float _seed;
    [SerializeField] [Range(0, 100)] int resolution;

    int xTiles = 30;
    int zTiles = 30;
    private float seed;
    // [SerializeField] private ResourcesGen resourcesGen;
    [SerializeField] private StructuresCreator structuresCreator;

    private Map _map;
    [SerializeField] private BMPToGrid bmpToGrid;
    [SerializeField] private TerrainGen ter;

    [ContextMenu("Generate Arena")]
    public void GenerateArenaFromEditor()
    {
        seed = Random.Range(0.1f, 0.5f);
        TerrainGen.PerlinNoiseGenerator.SetSeed(seed);
        ClearArena();
        GenerateArena();
    }

    private void ClearArena()
    {
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

    }

    void GenerateArena()
    {
        // playersAssign.reAssign();
        ClearArena();
        CreateGrid();
        // PrepareStructuresCords();
        var terrain = ter.CreateTerrain(seed, resolution,_map);
        CreateStructures(terrain);
    }

    private void CreateGrid()
    {
        _map = bmpToGrid.GenerateGridFromBmp();
    }

    private void CreateTerrain()
    {

        // Terrain ter = Terrain.CreateTerrainObj(stage).GetComponent<Terrain>();
        // StartCoroutine(ter.CreateTerrain(stageRows, spacing, seed,players)); 
        ter.CreateTerrain(seed, resolution,_map);
    }

    // void PrepareStructuresCords()
    // {
    //     var castleStages = new List<Stage.Type>();
    //
    //     foreach (var player in playersAssign.players)
    //     {
    //         if (player == null)
    //         {
    //             Debug.LogError("Player is null!");
    //             continue;
    //         }
    //
    //         if (player == null)
    //         {
    //             Debug.LogError($"PlayerSo is null for player: {player.name}");
    //             continue;
    //         }
    //
    //         if (player.playerSo.castleSo == null)
    //         {
    //             Debug.LogError($"CastleSo is null for player: {player.name}");
    //             continue;
    //         }
    //
    //         castleStages.Add(player.playerSo.castleSo.startStage);
    //     }
    //
    //     _terrainResources.Clear();
    //     for (int currentStage = 0; currentStage < 9; currentStage++)
    //     {
    //         var currentStageType = (Stage.Type)currentStage;
    //         if (currentStageType != Stage.Type.Special)
    //         {
    //             if (castleStages.Contains(currentStageType))
    //             {
    //                 var player = PlayerSo.CheckCurrentPByStage(currentStageType, playersAssign.db);
    //                 player.castleSo.mat = playersAssign.db.material;
    //                 player.castleSo.castleCords =
    //                     CastleGenerator.DrawCastlePlace(xTiles, currentStage);
    //                 List<Vector2> castleAreaCords =
    //                     StructureAreaChecker.TilesAround(player.castleSo.castleCords.ToList(), xTiles);
    //
    //                 var castleArea = new TerrainGen.OccupiedField();
    //                 castleArea.OccupiedTiles = castleAreaCords;
    //                 occupedFields.Add(castleArea);
    //             }
    //
    //             var resourcesForStage =
    //                 resourcesGen.CreateResourcesForStage(occupedFields, currentStage, xTiles);
    //
    //             foreach (var resource in resourcesForStage)
    //             {
    //                 _terrainResources.Push(resource);
    //                 Vector2[] resourceCoords = resource.Coords.ToArray();
    //                 Array.Reverse(resourceCoords);
    //                 List<Vector2> resourceAreaCords =
    //                     StructureAreaChecker.TilesAround(resourceCoords.ToList(), xTiles);
    //
    //                 var resourceArea = new TerrainGen.OccupiedField();
    //                 resourceArea.OccupiedTiles = resourceAreaCords;
    //                 occupedFields.Add(resourceArea);
    //             }
    //         }
    //         else if ((Stage.Type)currentStage == Stage.Type.Special)
    //         {
    //             var specialResource = resourcesGen.GenerateBestResource(occupedFields, currentStage, xTiles);
    //             _terrainResources.Push(specialResource);
    //             Vector2[] specialResourceCoords = specialResource.Coords.ToArray();
    //             Array.Reverse(specialResourceCoords);
    //             List<Vector2> specialResourceAreaCords =
    //                 StructureAreaChecker.TilesAround(specialResourceCoords.ToList(), xTiles);
    //
    //             var specialResourceArea = new TerrainGen.OccupiedField();
    //             specialResourceArea.OccupiedTiles = specialResourceAreaCords;
    //             occupedFields.Add(specialResourceArea);
    //         }
    //     }
    // }

    private void CreateStructures(Terrain terrain)
    {
        GameObject structuresParent = new GameObject("Structures");
        structuresParent.transform.parent = this.transform;
        structuresCreator.terrain = terrain;
        if (playersAssign.players.Count != _map.mapCastles.Count)
        {
            Debug.LogError("Players and castles " + playersAssign.players.Count + "  " + _map.mapCastles.Count);
            throw new NotImplementedException();
        }
        for (int i = 0; i < playersAssign.players.Count; i++)
        {
            var player = playersAssign.players[i];
            if (player.playerSo.castleSo.nation.castlePrefab == null)
            {
                Debug.LogError("nie ma obiektu");
            }
        
            GameObject castleObject = structuresCreator.CreateCastle(
                player.playerSo.castleSo.nation.castlePrefab,_map.mapCastles[i], player, seed);
        
            castleObject.transform.parent = structuresParent.transform;
        }
        
        foreach (var res in _map.mapResources)
        {
            GameObject resObj =
                structuresCreator.CreateResourceSource(res.resourceSo.resourceSourcePref, res, seed);
            resObj.transform.transform.parent = structuresParent.transform;
        }
    }

    bool IsTileInCastleCoords(Vector2 newTileCoords)
    {
        foreach (var player in playersAssign.db.playerDbs)
        {
            if (player.castleSo != null && player.castleSo.castleCords.Contains(newTileCoords))
            {
                return true;
            }
        }

        return false;
    }
}