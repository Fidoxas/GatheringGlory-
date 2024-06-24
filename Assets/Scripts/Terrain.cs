using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptablesOBJ;
using ScriptablesOBJ.Stages;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    [SerializeField] private Stack<TerrainResource> _terrainResources = new Stack<TerrainResource>();
    public List<Vector2> occupedTillesCords = new List<Vector2>();
    public Vector2[][] CastleCoords = new Vector2[8][];
    public PlayersDB playersDB;
    [SerializeField] ResourcesGen resourcesGen;

    public IEnumerator CreateTerrain(int stageRows, int spacing, float seed)
    {
        Vector3 arenaPos = gameObject.transform.localPosition;
        PrepareStructures(spacing, stageRows);

        var flatVertices = FindFlatVertices(0.5f, spacing, stageRows);

        for (int y = 0; y < spacing * stageRows; y++)
        {
            int yI = spacing * stageRows - 1 - y;
            for (int x = 0; x < spacing * stageRows; x++)
            {
                var newTileCoords = new Vector2(x, y);
                Vector3[] vertices = GenerateVertices(x, yI, flatVertices, arenaPos, seed);

                var currentStage = StageChecker.CheckCurrentStage(newTileCoords, spacing, stageRows);
                PlayerDB currentPlayerDB = Player.CheckCurrentPByStage(currentStage, playersDB);

                if (currentStage != Stage.Type.Special && IsTileInCastleCoords(newTileCoords))
                {
                    Tile.CreateTile(new[] { vertices[0], vertices[2], vertices[1] },
                        new[] { vertices[1], vertices[2], vertices[3] }, this.transform, new Vector2(x, y),
                        Tile.Type.Castle, null, currentPlayerDB.material);
                }
                else if (currentStage != Stage.Type.Special &&
                         IsTileInResourceCoords(newTileCoords, out TerrainResource resource))
                {
                    Tile.CreateTile(new[] { vertices[0], vertices[2], vertices[1] },
                        new[] { vertices[1], vertices[2], vertices[3] }, this.transform, new Vector2(x, y),
                        Tile.Type.Resource, resource.Resource, resource.Resource.material);
                    RemoveResource(resource);
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

    private List<Vector2> FindFlatVertices(float flatHeight, int spacing, int stageRows)
    {
        var _flatVerts = new List<Vector2>();
        for (int y = 0; y < spacing * stageRows; y++)
        {
            int yI = spacing * stageRows - 1 - y;
            for (int x = 0; x < spacing * stageRows; x++)
            {
                var newTileCoords = new Vector2(x, y);
                Vector3[] vertices = new Vector3[4];
                if (occupedTillesCords.Contains(newTileCoords))
                {
                    vertices[0] = new Vector3(x, flatHeight, yI);
                    vertices[1] = new Vector3(x + 1, flatHeight, yI);
                    vertices[2] = new Vector3(x, flatHeight, yI + 1);
                    vertices[3] = new Vector3(x + 1, flatHeight, yI + 1);

                    foreach (Vector3 vert in vertices)
                    {
                        _flatVerts.Add(new Vector2(vert.x, vert.z));
                    }
                }
            }
        }
        return _flatVerts;
    }

    private Vector3[] GenerateVertices(int x, int yI, List<Vector2> flatVertices, Vector3 arenaPos, float seed)
    {
        Vector3[] vertices = new Vector3[4];
        float flatHeight = 0.5f;

        vertices[0] = new Vector3(x,
            flatVertices.Contains(new Vector2(x, yI))
                ? flatHeight
                : Mathf.PerlinNoise((x + arenaPos.x) * seed, (yI + arenaPos.z) * seed), yI);
        vertices[1] = new Vector3(x + 1,
            flatVertices.Contains(new Vector2(x + 1, yI))
                ? flatHeight
                : Mathf.PerlinNoise((x + 1 + arenaPos.x) * seed, (yI + arenaPos.z) * seed), yI);
        vertices[2] = new Vector3(x,
            flatVertices.Contains(new Vector2(x, yI + 1))
                ? flatHeight
                : Mathf.PerlinNoise((x + arenaPos.x) * seed, (yI + 1 + arenaPos.z) * seed), yI + 1);
        vertices[3] = new Vector3(x + 1,
            flatVertices.Contains(new Vector2(x + 1, yI + 1))
                ? flatHeight
                : Mathf.PerlinNoise((x + 1 + arenaPos.x) * seed, (yI + 1 + arenaPos.z) * seed), yI + 1);

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] += arenaPos;
        }

        return vertices;
    }

    void PrepareStructures(int spacing, int stageRows)
    {
        _terrainResources.Clear();
        for (int currentStage = 0; currentStage < 9; currentStage++)
        {
            if ((Stage.Type)currentStage != Stage.Type.Special)
            {
                int adjustedStage = currentStage < 4 ? currentStage : currentStage - 1;
                CastleCoords[adjustedStage] = CastleGenerator.DrawCastlePlace(spacing, currentStage, stageRows);
                List<Vector2> CastleArea =
                    StructureAreaChecker.TilesAround(CastleCoords[adjustedStage], spacing * stageRows);
                occupedTillesCords.AddRange(CastleArea);
                var resourcesForStage =
                    resourcesGen.CreateResourcesForStage(occupedTillesCords, currentStage, spacing, stageRows);

                foreach (var resource in resourcesForStage)
                {
                    _terrainResources.Push(resource);
                    List<Vector2> resourceCoords = new List<Vector2> { resource.Coords };
                    List<Vector2> resourceArea =
                        StructureAreaChecker.TilesAround(resourceCoords.ToArray(), spacing * stageRows);
                    occupedTillesCords.AddRange(resourceArea);
                }
            }
        }

        Debug.Log(_terrainResources.Count + " terrain resources");
    }

    bool IsTileInCastleCoords(Vector2 newTileCoords)
    {
        foreach (var castle in CastleCoords)
        {
            if (castle != null && castle.Contains(newTileCoords))
            {
                return true;
            }
        }

        return false;
    }

    bool IsTileInResourceCoords(Vector2 newTileCoords, out TerrainResource foundResource)
    {
        foreach (var resource in _terrainResources)
        {
            if (resource.Coords == newTileCoords)
            {
                foundResource = resource;
                return true;
            }
        }

        foundResource = null;
        return false;
    }



    void RemoveResource(TerrainResource resource)
    {
        _terrainResources = new Stack<TerrainResource>(_terrainResources.Where(r => r != resource));
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