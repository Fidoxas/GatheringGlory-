using System;
using ScriptablesOBJ;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Player.Numbers player;
    public Type type = Type.Neutral;
    public Material mat;
    public Vector2 coords;

    public enum Type
    {
        Resource,
        Neutral,
        Castle
    }

    public static void CreateTile(Vector3[] triangle1, Vector3[] triangle2, Transform transform, Vector2 coords,
        Type type,Player.Numbers occupator, Resource resource, Material mat, Castle.Type nation)
    {
        if (mat == null)
        {
            mat = new Material(Shader.Find("Standard"));
            Debug.LogWarning("Material was null. A default standard material has been assigned.");
        }

        string tileName = $"Tile_{coords.x}_{coords.y}";
        GameObject tileObj = new GameObject(tileName);
        tileObj.transform.SetParent(transform);

        Tile tileSc;
        if (type == Type.Neutral)
        {
            tileSc = tileObj.AddComponent<Tile>();
            tileSc.player = occupator;
        }
        else if (type == Type.Castle)
        {
            var castleTileSc = tileObj.AddComponent<CastleTile>();
            castleTileSc.nation = nation;
            tileSc = castleTileSc;
            tileObj.AddComponent<UnitId>().pNum = occupator;

        }
        else if (type == Type.Resource)
        {
            var resourceTileSc = tileObj.AddComponent<ResourceTile>();
            resourceTileSc.mat = resource.material;
            tileSc = resourceTileSc;
            tileSc.player = occupator;
        }
        else
        {
            throw new ArgumentOutOfRangeException();
        }

        tileSc.mat = mat;
        tileSc.coords = coords;
        tileSc.type = type;

        CreateTriangle(tileObj.transform, triangle1[0], triangle1[1], triangle1[2], tileSc.mat);
        CreateTriangle(tileObj.transform, triangle2[0], triangle2[1], triangle2[2], tileSc.mat);

        Debug.Log($"Created tile of type {type} at coordinates {coords} with material {mat.name}");
    }

    public static void CreateTriangle(Transform parentTransform, Vector3 v0, Vector3 v1, Vector3 v2, Material material)
    {
        GameObject triangleObj = new GameObject("Triangle");
        triangleObj.transform.SetParent(parentTransform);
        triangleObj.transform.localPosition = Vector3.zero;
        triangleObj.transform.localRotation = Quaternion.identity;

        Mesh mesh = new Mesh();
        mesh.vertices = new[] { v0, v1, v2 };
        mesh.triangles = new[] { 0, 1, 2 };
        mesh.RecalculateNormals();

        MeshFilter meshFilter = triangleObj.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer renderer = triangleObj.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = material;

        triangleObj.AddComponent<MeshCollider>();

        int terrainLayer = LayerMask.NameToLayer("Terrain");
        if (terrainLayer == -1)
        {
            Debug.LogError("Layer 'Terrain' does not exist. Please add it in the Layers settings.");
        }
        else
        {
            triangleObj.layer = terrainLayer;
        }

        Debug.Log($"Created triangle with vertices {v0}, {v1}, {v2} and material {material.name}");
    }
}
