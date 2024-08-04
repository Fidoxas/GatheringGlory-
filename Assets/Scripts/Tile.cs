using UnityEngine;
using UnityEngine.Serialization;

public class Tile 
{
    public bool _assigned = false;
    public Type type;
    public Vector3 Position { get; private set; }
    private GameObject _marker;
    public Map.Object MapObject { get; private set; }

    public void SetMapObject(Map.Object obj)
    {
        MapObject = obj;
    } 
    public enum Type
    {
        Normal,
        Castle,
        Crystal,
        Elest,
        Rune,
        Core,
    }

    public bool IsResource()
    {
        return type == Type.Core || type == Type.Elest || type == Type.Rune || type == Type.Crystal;
    }

    public Map.Resource GetMapObjectAsResource()
    {
        return MapObject as Map.Resource;
    }

    public bool IsCastle()
    {
        return type == Type.Castle;
    }

    public void SetPosition(Vector3 position)
    {
        _assigned = true;
        this.Position = position;
    }

    // private void Start()
    // {
    //     transform.position = new Vector3(_position.x, 3, _position.y);
    //     GameObject GO = Instantiate(_marker, transform.position, Quaternion.identity);
    //     GO.transform.parent = this.transform;
    //
    //     MeshRenderer meshRenderer = GO.GetComponent<MeshRenderer>();
    //     if (meshRenderer != null)
    //     {
    //         meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
    //         meshRenderer.sharedMaterial.color = _color;
    //     }
    //     else
    //     {
    //         Debug.LogError("MeshRenderer not found on the marker GameObject.");
    //     }
    // }
}

