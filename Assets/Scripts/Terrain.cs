using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Terrain : MonoBehaviour
{
    private int _sizeX;
    private int _sizeZ;
    private int _resolution;
    private Map _map;
    private Mesh _mesh;

    /// <summary>
    /// Ustawia teren, tworzy siatkę na podstawie wierzchołków i trójkątów.
    /// </summary>
    public void SetTerrain(List<Vector3> vertices, List<int> triangles, int resolution, Map map, int sizeX, int sizeZ)
    {
        _resolution = resolution;
        _mesh = CreateMesh(vertices, triangles);
        _map = map;
        _sizeX = sizeX;
        _sizeZ = sizeZ;
    }

    /// <summary>
    /// Tworzy i przypisuje siatkę terenu.
    /// </summary>
    private Mesh CreateMesh(List<Vector3> vertices, List<int> triangles)
    {
        Mesh mesh = new Mesh
        {
            indexFormat = IndexFormat.UInt32,
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray()
        };
        mesh.RecalculateNormals();

        // Dodaj komponenty siatki
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        // Przypisz domyślny materiał
        Material standardMaterial = new Material(Shader.Find("Standard"));
        meshRenderer.material = standardMaterial;

        return mesh;
    }

    /// <summary>
    /// Dopasowuje wysokości wierzchołków w zadanym obszarze do średniej wysokości sąsiednich pól.
    /// </summary>
    public void MatchFieldYToTerrain(Vector3 startTile, Vector3 endTile)
    {
        if (_mesh == null)
        {
            Debug.LogError("Mesh is not initialized!");
            return;
        }

        var vertices = _mesh.vertices;
        Vector3 startBounds = startTile - new Vector3(1, 0, 1);
        Vector3 endBounds = endTile + new Vector3(2, 0, 2);

        float heightSum = 0;
        int heightCount = 0;

        for (float x = startBounds.x; x <= endBounds.x; x += 1f / _resolution)
        {
            for (float z = startBounds.z; z <= endBounds.z; z += 1f / _resolution)
            {
                if (startTile.x<=x && endTile.x+1 >=z ||startTile.z<=x && endTile.z+1 >=z )
                {
                    continue;
                }
                int vertIndex = GetVerticeIndex(x, z);
                if (IsValidVerticeIndex(vertIndex, vertices.Length))
                {
                    heightSum += vertices[vertIndex].y;
                    heightCount++;
                }
            }
        }

        float averageHeight = heightCount > 0 ? heightSum / heightCount : 0;

        // Ustaw wysokości wierzchołków na średnią
        for (float x = startBounds.x; x <= endBounds.x; x += 1f / _resolution)
        {
            for (float z = startBounds.z; z <= endBounds.z; z += 1f / _resolution)
            {
                int vertIndex = GetVerticeIndex(x, z);
                if (IsValidVerticeIndex(vertIndex, vertices.Length))
                {
                    vertices[vertIndex] = new Vector3(vertices[vertIndex].x, averageHeight, vertices[vertIndex].z);
                }
            }
        }

        _mesh.vertices = vertices;
        _mesh.RecalculateNormals();
        this.GetComponent<MeshCollider>().sharedMesh = _mesh;
    }

    /// <summary>
    /// Sprawdza, czy indeks wierzchołka jest prawidłowy.
    /// </summary>
    private bool IsValidVerticeIndex(int index, int length)
    {
        return index >= 0 && index < length;
    }

    /// <summary>
    /// Oblicza indeks wierzchołka na podstawie pozycji w siatce.
    /// </summary>
    public int GetVerticeIndex(float x, float z)
    {
        if (x < 0 || x >= _sizeX || z < 0 || z >= _sizeZ)
            return -1;
        return (int)(z * _sizeZ * _resolution + x * _resolution);
    }
}
