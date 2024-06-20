using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptablesOBJ;
using ScriptablesOBJ.Stages;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class Terrain : MonoBehaviour
{
    public Stack<TerrainResource> terrainResources;
    public List<Vector2> occupedTillesCords = new List<Vector2>(); // Initialize here
    public Vector2[][] castleCoords = new Vector2[8][]; // Initialize with appropriate size
    public PlayersDB playersDB;
    private ResourcesGen resourcesGen;
    
    public IEnumerator CreateTerrain(int stageRows, int spacing, float seed)
    {
        Vector3 ArenaPos = gameObject.transform.localPosition;
        PrepareStructures(spacing, stageRows);

        for (int y = 0; y < spacing * stageRows; y++)
        {
            int yI = spacing * stageRows - 1 - y;
            for (int x = 0; x < spacing * stageRows; x++)
            {
                Vector3[] vertices = new Vector3[4];
                vertices[0] = new Vector3(x, Mathf.PerlinNoise((x + ArenaPos.x) * seed, (yI + ArenaPos.z) * seed), yI);
                vertices[1] = new Vector3(x + 1, Mathf.PerlinNoise((x + 1 + ArenaPos.x) * seed, (yI + ArenaPos.z) * seed), yI);
                vertices[2] = new Vector3(x, Mathf.PerlinNoise((x + ArenaPos.x) * seed, (yI + 1 + ArenaPos.z) * seed), yI + 1);
                vertices[3] = new Vector3(x + 1, Mathf.PerlinNoise((x + 1 + ArenaPos.x) * seed, (yI + 1 + ArenaPos.z) * seed), yI + 1);

                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] += ArenaPos;
                }

                var newTileCoords = new Vector2(x, y);
                var currentStage = StageChecker.CheckCurrentStage(newTileCoords, spacing, stageRows);
                PlayerDB currentPlayerDB = Player.CheckCurrentPByStage(currentStage, playersDB);       
                
                if (currentStage != Stage.Type.Special && IsTileInCastleCoords(newTileCoords))
                {
                    Tile.CreateTile(new[] { vertices[0], vertices[2], vertices[1] },
                        new[] { vertices[1], vertices[2], vertices[3] }, this.transform, new Vector2(x, y),
                        Tile.Type.Castle,null, currentPlayerDB.material);
                }
                //else if (currentStage != Stage.Type.Special && terrainResources.Any(tr => tr.Coords == newTileCoords))
                //{
                //    var resource = terrainResources.First(tr => tr.Coords == newTileCoords).Resource;
                //    Tile.CreateTile(new[] { vertices[0], vertices[2], vertices[1] },
                //        new[] { vertices[1], vertices[2], vertices[3] }, _terrainObj, new Vector2(x, y),
                //        Tile.Type.Resource, resource);
                //    terrainResources.Pop();
                //}
                else
                {
                    Tile.CreateTile(new[] { vertices[0], vertices[2], vertices[1] },
                        new[] { vertices[1], vertices[2], vertices[3] }, this.transform, new Vector2(x, y),
                        Tile.Type.Neutral,null, null);
                }

                yield return null;
            }
        }
    }
    
    void PrepareStructures(int spacing, int stageRows)
    {
        for (int currentStage = 0; currentStage < 9; currentStage++)
        {

            if ((Stage.Type)currentStage != Stage.Type.Special)
            {
                if (currentStage <4)
                {
                    castleCoords[currentStage] = CastleGenerator.DrawCastlePlace(spacing, currentStage, stageRows);
                    occupedTillesCords.AddRange(castleCoords[currentStage]);
                    // terrainResources = resourcesGen.CreateResourcesForStage(occupedTillesCords, (int)currentStage,spacing, stageRows);
                }
                else
                {
                    castleCoords[currentStage-1] = CastleGenerator.DrawCastlePlace(spacing, currentStage, stageRows);
                    occupedTillesCords.AddRange(castleCoords[currentStage-1]);
                    // terrainResources = resourcesGen.CreateResourcesForStage(occupedTillesCords, (int)currentStage,spacing, stageRows);
                }
                
            // terrainResources = resourcesGen.CreateResourcesForStage(occupedTillesCords, (int)currentStage,spacing, stageRows);
            }
        }
    }

    public bool IsTileInCastleCoords(Vector2 newTileCoords)
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
