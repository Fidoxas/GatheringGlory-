using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptablesOBJ;
using ScriptablesOBJ.Stages;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    Stack<List<TerrainResource>> _terrainResources = new Stack<List<TerrainResource>>();
    public List<Vector2> occupedTillesCords = new List<Vector2>(); 
    public Vector2[][] castleCoords = new Vector2[8][]; 
    public PlayersDB playersDB;
    [SerializeField] ResourcesGen resourcesGen;
    
    public IEnumerator CreateTerrain(int stageRows, int spacing, float seed)
    {
        Vector3 arenaPos = gameObject.transform.localPosition;
        PrepareStructures(spacing, stageRows);

        for (int y = 0; y < spacing * stageRows; y++)
        {
            int yI = spacing * stageRows - 1 - y;
            for (int x = 0; x < spacing * stageRows; x++)
            {
                Vector3[] vertices = new Vector3[4];
                vertices[0] = new Vector3(x, Mathf.PerlinNoise((x + arenaPos.x) * seed, (yI + arenaPos.z) * seed), yI);
                vertices[1] = new Vector3(x + 1, Mathf.PerlinNoise((x + 1 + arenaPos.x) * seed, (yI + arenaPos.z) * seed), yI);
                vertices[2] = new Vector3(x, Mathf.PerlinNoise((x + arenaPos.x) * seed, (yI + 1 + arenaPos.z) * seed), yI + 1);
                vertices[3] = new Vector3(x + 1, Mathf.PerlinNoise((x + 1 + arenaPos.x) * seed, (yI + 1 + arenaPos.z) * seed), yI + 1);

                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] += arenaPos;
                }

                var newTileCoords = new Vector2(x, y);
                var currentStage = StageChecker.CheckCurrentStage(newTileCoords, spacing, stageRows);
                PlayerDB currentPlayerDB = Player.CheckCurrentPByStage(currentStage, playersDB);       
                
                if (currentStage != Stage.Type.Special && IsTileInCastleCoords(newTileCoords))
                {
                    Tile.CreateTile(new[] { vertices[0], vertices[2], vertices[1] },
                        new[] { vertices[1], vertices[2], vertices[3] }, this.transform, new Vector2(x, y),
                        Tile.Type.Castle, null, currentPlayerDB.material);
                }
                else if (currentStage != Stage.Type.Special && IsTileInResourceCoords(newTileCoords))
                {
                    var resourceList = _terrainResources.FirstOrDefault(trList => trList.Any(tr => tr.Coords == newTileCoords));
                    if (resourceList != null)
                    {
                        var resource = resourceList.First(tr => tr.Coords == newTileCoords);
                        Tile.CreateTile(new[] { vertices[0], vertices[2], vertices[1] },
                            new[] { vertices[1], vertices[2], vertices[3] }, this.transform, new Vector2(x, y),
                            Tile.Type.Resource, resource.Resource, resource.Resource.material);
                        resourceList.Remove(resource); // Usunięcie zasobu z listy
                        if (resourceList.Count == 0)
                        {
                            _terrainResources.Pop(); // Usunięcie pustej listy z listy stosu
                        }
                    }
                }
                else
                {
                    Tile.CreateTile(new[] { vertices[0], vertices[2], vertices[1] },
                        new[] { vertices[1], vertices[2], vertices[3] }, this.transform, new Vector2(x, y),
                        Tile.Type.Neutral, null, null);
                }

                yield return null;
            }
        }
    }

    void PrepareStructures(int spacing, int stageRows)
    {
        _terrainResources.Clear();
        for (int currentStage = 0; currentStage < 9; currentStage++)
        {
            if ((Stage.Type)currentStage != Stage.Type.Special)
            {
                int adjustedStage = currentStage < 4 ? currentStage : currentStage - 1;
                castleCoords[adjustedStage] = CastleGenerator.DrawCastlePlace(spacing, currentStage, stageRows);
                occupedTillesCords.AddRange(castleCoords[adjustedStage]);
                var resourcesForStage = resourcesGen.CreateResourcesForStage(occupedTillesCords, currentStage, spacing, stageRows);
                _terrainResources.Push(resourcesForStage.ToList());
            }
        }
        Debug.Log(_terrainResources + " terrain resources");
    }

    bool IsTileInCastleCoords(Vector2 newTileCoords)
    {
        foreach (var castle in castleCoords)
        {
            if (castle != null && castle.Contains(newTileCoords))
            {
                return true;
            }
        }
        return false;
    }

    bool IsTileInResourceCoords(Vector2 newTileCoords)
    {
        return _terrainResources.Any(trList => trList.Any(tr => tr.Coords == newTileCoords));
    }

    public class TerrainResource
    {
        public Vector2 Coords { get; set; }
        public Resource Resource { get; set; }

        public TerrainResource(Vector2 coords, Resource resource)
        {
            Coords = coords;
            Resource = resource;
        }
    }
}
