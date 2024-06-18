using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class Terrain : MonoBehaviour
{
    public Stage stage;
    public GameObject _terrainObj;
    public Stack<TerrainResource> terrainResources;
    public List<Vector2> occupedTillesCords;
    public Vector2[] castleCoords;
    [SerializeField] private ResourcesGen resourcesGen;

    // private void Start()
    // {
    //     _terrainObj = gameObject;
    // }

    // ReSharper disable Unity.PerformanceAnalysis
    public IEnumerator CreateTerrain(int stageRows, int spacing, float seed)
    {
        Vector3 stagePos = stage.gameObject.transform.localPosition;
        Stage stageSc = stage.GetComponent<Stage>();
        occupedTillesCords = new List<Vector2>();
        var currentStage = stageSc.type;
        if(currentStage != Stage.StageType.Special)
            castleCoords = CastleGenerator.DrawCastlePlace(spacing, (int)currentStage, stageRows);
        occupedTillesCords.AddRange(castleCoords);
        
        terrainResources = resourcesGen.CreateResourcesForStage(occupedTillesCords, (int)currentStage,spacing, stageRows);

        for (int y = 0; y < spacing; y++)
        {
            int yI = spacing - 1 - y;
            for (int x = 0; x < spacing; x++)
            {
                Vector3[] vertices = new Vector3[4];
                vertices[0] = new Vector3(x, Mathf.PerlinNoise((x + stagePos.x) * seed, (yI + stagePos.z) * seed),
                    yI);
                vertices[1] = new Vector3(x + 1,
                    Mathf.PerlinNoise((x + 1 + stagePos.x) * seed, (yI + stagePos.z) * seed), yI);
                vertices[2] = new Vector3(x, Mathf.PerlinNoise((x + stagePos.x) * seed, (yI + 1 + stagePos.z) * seed),
                    yI + 1);
                vertices[3] = new Vector3(x + 1,
                    Mathf.PerlinNoise((x + 1 + stagePos.x) * seed, (yI + 1 + stagePos.z) * seed), yI + 1);

                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] += stagePos;
                }

                var newTileCoords = new Vector2(x, y);
                if (currentStage != Stage.StageType.Special && castleCoords.Contains(newTileCoords))
                {
                    // CreateCastleTile
                    Tile.CreateTile(new[] { vertices[0], vertices[2], vertices[1] },
                        new[] { vertices[1], vertices[2], vertices[3] }, _terrainObj, new Vector2(x, y),
                        Tile.Type.Castle, null);
                }
                else if (currentStage != Stage.StageType.Special && terrainResources.FirstOrDefault((tr) => tr.Coords.Equals(newTileCoords)) != null)
                {
                    // CreateResoureTile
                    Tile.CreateTile(new[] { vertices[0], vertices[2], vertices[1] },
                        new[] { vertices[1], vertices[2], vertices[3] }, _terrainObj, new Vector2(x, y),
                        Tile.Type.Resource,terrainResources.Peek().Resource);
                    terrainResources.Pop();
                }
                else
                {
                    Tile.CreateTile(new[] { vertices[0], vertices[2], vertices[1] },
                        new[] { vertices[1], vertices[2], vertices[3] }, _terrainObj, new Vector2(x, y),
                        Tile.Type.Neutral, null);
                }

                yield return null;
            }
        }
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