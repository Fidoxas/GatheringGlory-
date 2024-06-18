using UnityEngine;

public class Tile : MonoBehaviour
{
    public Player.PlayersNum player = Player.PlayersNum.None;
    public Type type = Type.Neutral;
    public Material mat;
    public Vector2 coords;

    public enum Type
    {
        Resource,
        Neutral,
        Castle
    }
   public static void CreateTile(Vector3[] triangle1, Vector3[] triangle2, GameObject terrainObj, Vector2 coords,
        Type type, Resource resource)
    {
        GameObject tileObj = new GameObject("Tile");
        Stage stage = terrainObj.GetComponent<Terrain>().stage;
        tileObj.transform.SetParent(terrainObj.transform);

        Tile tileSc;
        if (type == Type.Neutral)
        {
            tileSc = tileObj.AddComponent<Tile>();
            tileSc.mat = stage.material;
            tileSc.coords = coords;
        }
        else if (type == Type.Castle)
        {
            tileSc = tileObj.AddComponent<CastleTile>();
            tileSc.mat = new Material(Shader.Find($"Standard"));
            tileSc.coords = coords;
        }
        else if(type == Type.Resource)
        {
            tileSc = tileObj.AddComponent<ResourceTile>();
            tileSc.mat = resource.material;
            
        }

        CreateTriangle(tileObj, triangle1[0], triangle1[1], triangle1[2]);
        CreateTriangle(tileObj, triangle2[0], triangle2[1], triangle2[2]);

        foreach (Transform triangle in tileObj.transform)
        {
            MeshRenderer renderer = triangle.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = tileObj.GetComponent<Tile>().mat;
        }
    }

    public static void CreateTriangle(GameObject terrain, Vector3 v0, Vector3 v1, Vector3 v2)
    {
        GameObject triangleObj = new GameObject("Triangle");
        triangleObj.transform.SetParent(terrain.transform);
        triangleObj.transform.localPosition = Vector3.zero;
        triangleObj.transform.localRotation = Quaternion.identity;

        Mesh mesh = new Mesh();
        mesh.vertices = new[] { v0, v1, v2 };
        mesh.triangles = new[] { 0, 1, 2 };
        mesh.RecalculateNormals();

        MeshFilter meshFilter = triangleObj.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        triangleObj.AddComponent<MeshRenderer>();
    }
}