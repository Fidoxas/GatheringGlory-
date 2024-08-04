
using System.Collections.Generic;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;

public class Map 
{
    private int _x;
    private int _z;
    public readonly Tile[,] GridArray;
    public List<Resource> mapResources = new List<Resource>();
    public List<Castle> mapCastles = new List<Castle>();

    public int GetSizeX()
    {
        return _x;
    }

    public int GetSizeZ()
    {
        return _z;
    }
    
    public Map(int x, int z)
    {
        _x = x;
        _z = z;
        GridArray = new Tile[_x, _z];
        for (int xPos = 0; xPos < x; xPos++)
        {
            for (int zPos = 0; zPos < z; zPos++)
            {
                GridArray[xPos, zPos] = new Tile();
            }
        }
    }

    public class Object {}

    public class Resource: Map.Object
    {
        public List<Tile> Tiles;
        public ResourceSO resourceSo { get; private set; }
        
        public void SetResourceSo(ResourceSO so)
        {
            resourceSo = so;
        }
    }

    public class Castle: Map.Object
    {
        public List<Tile> Tiles;
    }
}

