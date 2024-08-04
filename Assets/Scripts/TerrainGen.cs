using System;
using System.Collections.Generic;
using ScriptablesOBJ;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

public class TerrainGen : MonoBehaviour
{
    private List<Stage.Type> _castleStages;
    private List<Player> _players;
    private Map _map;

    private int _resolution;
    private int _sizeZ;
    private int _sizeX;

    public Terrain CreateTerrain(float seed, int resolution, Map map)
    {
        _resolution = resolution;
        _map = map;
        List<Vector3> verticesList = new List<Vector3>();

        var vertices = CreateTerrainVertices();
        var triangles = CreateTerrainTriangles();

        for (int x = 0; x < _sizeX; x++)
        {
            for (int z = 0; z < _sizeZ; z++)
            {
                verticesList.Add(vertices[x, z]);
            }
        }
        // Debug.Log($"Vertices count: {verticesList.Count}");
        // Debug.Log($"Triangles count: {triangles.Count}");
        var terrain =SetTerrain(verticesList, triangles);

        // GenerateMesh(verticesList, triangles);
        return terrain;
    }

    private Terrain SetTerrain(List<Vector3> vertices, List<int> triangles)
    {
        var terrain = new GameObject("Terrain");
        terrain.layer = LayerMask.NameToLayer("Terrain");
        terrain.transform.parent = this.transform;

        var terrainSc = terrain.AddComponent<Terrain>();
        terrainSc.SetTerrain(vertices, triangles, _resolution, _map,_sizeX,_sizeZ);
        return terrainSc;
    }


    private Vector3[,] CreateTerrainVertices()
    {
        Vector3 arenaPos = gameObject.transform.localPosition;
        List<OccupiedField> occupiedFields = GetFlatFields();

        _sizeX = _map.GetSizeX() * _resolution;
        _sizeZ = _map.GetSizeZ() * _resolution;

        Vector3[,] vertices = new Vector3[_sizeZ, _sizeX];

        Debug.Log("sizeX: " + _sizeX + " sizeZ: " + _sizeZ);

        List<Vector3> verticesList = new List<Vector3>();

        for (int x = 0; x < _sizeX; x++)
        {
            for (int z = 0; z < _sizeZ; z++)
            {
                var xPos = x / (float)_resolution;
                var zPos = z / (float)_resolution;
                int xTile = x / _resolution;
                int zTile = z / _resolution;

                Vector3 vert = new Vector3(xPos, 0, zPos);
                var occupiedField = VerticeOccupied(occupiedFields, vert);
                float height = 0;
                //     height = occupiedField.AvrgHighInPerlin;
                // height = PerlinNoiseGenerator.GetY(x, map.GridArray[xTile, zTile].Position.y, z);
                height = _map.GridArray[xTile, zTile].Position.y * 8;

                // Debug.Log(height + " wysokosc");
                // Mathf.PerlinNoise(x * 0.1f + seed, z * 0.1f + seed) * 5f; // Przykład generowania wysokości

                vertices[z, x] = new Vector3(xPos, height, zPos);
            }
        }

        if (_resolution
            > 1)
        {
            GaussianBlur(vertices);
        }

        return vertices;
    }

    private List<int> CreateTerrainTriangles()
    {
        List<int> triangles = new List<int>();

        for (int x = 0; x < _sizeX - 1; x++)
        {
            for (int z = 0; z < _sizeZ - 1; z++)
            {
                int indexBottomLeft = x + z * _sizeX;
                int indexBottomRight = indexBottomLeft + 1;
                int indexTopLeft = indexBottomLeft + _sizeX;
                int indexTopRight = indexTopLeft + 1;

                // Pierwszy trójkąt (górny lewy) - porządek CCW
                triangles.Add(indexBottomLeft);
                triangles.Add(indexTopLeft);
                triangles.Add(indexBottomRight);

                // Drugi trójkąt (dolny prawy) - porządek CCW
                triangles.Add(indexTopLeft);
                triangles.Add(indexTopRight);
                triangles.Add(indexBottomRight);
            }
        }

        return triangles;
    }


    private float[,] GenerateGaussianKernel(int size, float sigma)
    {
        if (size % 2 == 0)
        {
            throw new ArgumentException("Kernel size must be odd.");
        }

        int halfSize = size / 2;
        float[,] kernel = new float[size, size];
        float sum = 0f;

        float twoSigmaSquared = 2 * sigma * sigma;
        float normalizationFactor = 1 / (Mathf.PI * twoSigmaSquared);


        for (int y = -halfSize; y <= halfSize; y++)
        {
            for (int x = -halfSize; x <= halfSize; x++)
            {
                float exponent = -(x * x + y * y) / twoSigmaSquared;
                kernel[y + halfSize, x + halfSize] = normalizationFactor * Mathf.Exp(exponent);
                sum += kernel[y + halfSize, x + halfSize];
            }
        }


        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                kernel[y, x] /= sum;
            }
        }

        return kernel;
    }

    private void GaussianBlur(Vector3[,] vertices)
    {
        float[,] kernel = GenerateGaussianKernel(3, 4f);

        float[,] newHeights = new float[_sizeZ, _sizeX];

        for (int z = 1; z < _sizeZ - 1; z++)
        {
            for (int x = 1; x < _sizeX - 1; x++)
            {
                float newHeight = 0f;

                for (int kernelZ = -1; kernelZ <= 1; kernelZ++)
                {
                    for (int kernelX = -1; kernelX <= 1; kernelX++)
                    {
                        int sampleX = x + kernelX;
                        int sampleZ = z + kernelZ;
                        float kernelValue = kernel[kernelZ + 1, kernelX + 1];
                        newHeight += vertices[sampleZ, sampleX].y * kernelValue;
                    }
                }

                newHeights[z, x] = newHeight;
            }
        }

        for (int z = 1; z < _sizeZ - 1; z++)
        {
            for (int x = 1; x < _sizeX - 1; x++)
            {
                vertices[z, x].y = newHeights[z, x] + PerlinNoiseGenerator.GetY(x, z);
                // vertices[z, x].y = newHeights[z, x];
                if (x == 50 && z == 50)
                {
                    Debug.Log("------------------" + newHeights[z, x] + " --- " + PerlinNoiseGenerator.GetY(x, z));
                }
            }
        }
    }


    private void GenerateMesh(List<Vector3> vertices, List<int> triangles)
    {
        Mesh mesh = new Mesh
        {
            indexFormat = IndexFormat.UInt32,
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray()
        };
        mesh.RecalculateNormals();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        Material standardMaterial = new Material(Shader.Find("Standard"));
        meshRenderer.material = standardMaterial;
    }

    private OccupiedField VerticeOccupied(List<OccupiedField> occupedFields, Vector3 vertice)
    {
        var cords = new Vector2(vertice.x, vertice.z);


        foreach (var field in occupedFields)
        {
            var occupedFieldsCords = field.OccupiedTiles;
            if (occupedFieldsCords.Count <= 0)
            {
                continue;
            }

            float minX = occupedFieldsCords[0].x;
            float maxX = occupedFieldsCords[occupedFieldsCords.Count - 1].x;
            float minY = occupedFieldsCords[0].z;
            float maxY = occupedFieldsCords[occupedFieldsCords.Count - 1].z;

            bool isWithinRangeX = cords.x >= minX && cords.x <= maxX;
            bool isWithinRangeY = cords.y >= minY && cords.y <= maxY;

            if (isWithinRangeX && isWithinRangeY)
            {
                return field;
            }
        }

        return null;
    }


    private List<OccupiedField> GetFlatFields()
    {
        var flatFields = new List<OccupiedField>();

        foreach (var resource in _map.mapResources)
        {
            var flatField = new OccupiedField(); // Tworzenie nowego obiektu dla każdego zamku
            var wide = Mathf.CeilToInt(Mathf.Sqrt(resource.Tiles.Count));

            for (int x = (int)resource.Tiles[0].Position.x - 1; x <= (int)resource.Tiles[0].Position.x + wide; x++)
            {
                for (int z = (int)resource.Tiles[0].Position.z - 1; z <= (int)resource.Tiles[0].Position.z + wide; z++)
                {
                    var y = _map.GridArray[x, z].Position.y;
                    Vector3[] vertices = new Vector3[4];
                    vertices[0] = new Vector3(x, y, z);
                    vertices[1] = new Vector3(x + 1, y, z);
                    vertices[2] = new Vector3(x, y, z + 1);
                    vertices[3] = new Vector3(x + 1, y, z + 1);

                    foreach (var vert in vertices)
                    {
                        flatField.OccupiedTiles.Add(vert);
                    }
                }
            }

            flatField.AvrgHighInPerlin = PerlinNoiseGenerator.GetAvrgY(flatField.OccupiedTiles);
            flatFields.Add(flatField);
        }

        foreach (var castle in _map.mapCastles)
        {
            var flatField = new OccupiedField(); // Tworzenie nowego obiektu dla każdego zamku
            var wide = Mathf.CeilToInt(Mathf.Sqrt(castle.Tiles.Count));

            for (int x = (int)castle.Tiles[0].Position.x - 1; x <= (int)castle.Tiles[0].Position.x + wide; x++)
            {
                for (int z = (int)castle.Tiles[0].Position.z - 1; z <= (int)castle.Tiles[0].Position.z + wide; z++)
                {
                    var y = _map.GridArray[x, z].Position.y;
                    Vector3[] vertices = new Vector3[4];
                    vertices[0] = new Vector3(x, y, z);
                    vertices[1] = new Vector3(x + 1, y, z);
                    vertices[2] = new Vector3(x, y, z + 1);
                    vertices[3] = new Vector3(x + 1, y, z + 1);

                    foreach (var vert in vertices)
                    {
                        flatField.OccupiedTiles.Add(vert);
                    }
                }
            }

            flatField.AvrgHighInPerlin = PerlinNoiseGenerator.GetAvrgY(flatField.OccupiedTiles);
            flatFields.Add(flatField);
        }

        return flatFields; // Zwrócenie listy pól
    }


    public class OccupiedField
    {
        public List<Vector3> OccupiedTiles = new List<Vector3>();
        public float AvrgHighInPerlin = 0;
    }


    public static class PerlinNoiseGenerator
    {
        public static float Seed { get; private set; }

        public static void SetSeed(float seed)
        {
            Seed = seed;
        }

        public static float GetY(float x, float y, float z)
        {
            return GetY(x, z) + (y * 3);
        }

        public static float GetY(float x, float z)
        {
            var details = 0.172f; //0.65 - resolution=2; 0.3f - resolution=3
            return Mathf.PerlinNoise(x * details * Seed, z * details * Seed);
        }

        public static float GetAvrgY(List<Vector3> cords)
        {
            float totalHeight = 0;
            foreach (var tile in cords)
            {
                float height = GetY(tile.x, tile.y, tile.z);
                totalHeight += height;
            }

            float avrgHighInPerlin = totalHeight / cords.Count;
            return avrgHighInPerlin;
        }
    }
}