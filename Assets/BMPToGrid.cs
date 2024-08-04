using System;
using System.Collections.Generic;
using System.Drawing;
using OpenCover.Framework.Model;
using ScriptablesOBJ;
using Unity.VisualScripting;
using UnityEngine;
using Color = UnityEngine.Color;

public class BMPToGrid : MonoBehaviour
{
    public Texture2D bmpTexture;
    public Map map;
    public GameObject marker;

    public Dictionary<Tile.Type, int> StructNumerator = new Dictionary<Tile.Type, int>();
    [SerializeField] private ResourcesDB resSo;
    private Color _pixelColor;

    public Map GenerateGridFromBmp()
    {
        if (bmpTexture == null)
        {
            Debug.LogError("BMP texture is not assigned!");
            return null;
        }

        int xSize = bmpTexture.width;
        int zSize = bmpTexture.height;

        Debug.Log($"{xSize} xSize");
        Debug.Log($"{zSize} zSize");

        map = new Map(xSize, zSize);

        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                if (map.GridArray[x, z] != null && map.GridArray[x, z]._assigned)
                {
                    continue;
                }

                _pixelColor = bmpTexture.GetPixel(x, z);

                if (_pixelColor == new Color32(92, 0, 255, 255))
                {
                    CreateCastleOnTheMap(x, z);
                    Debug.Log("Found a purple tile!");
                }
                else if (_pixelColor == new Color32(92, 255, 0, 255))
                {
                    CreateResourceOnTheMap(x,z,3, Tile.Type.Core,resSo.core);
                    Debug.Log("Found a green tile!");
                }
                else if (_pixelColor == new Color32(255, 0, 0, 255))
                {
                    CreateResourceOnTheMap(x,z,2, Tile.Type.Crystal,resSo.crystal);
                    Debug.Log("Found a red tile!");
                }
                else if (_pixelColor == new Color32(255, 255, 0, 255))
                {
                    CreateResourceOnTheMap(x,z,2, Tile.Type.Elest,resSo.elest);
                    Debug.Log("Found a yellow tile!");
                }
                else if (_pixelColor == new Color32(0, 0, 255, 255))
                {
                    CreateResourceOnTheMap(x,z,2, Tile.Type.Rune,resSo.rune);
                    Debug.Log("Found a blue tile!");
                }
                else
                {
                    AssignValuesForTile(x, z, Tile.Type.Normal);
                    // Debug.Log("Found an unknown tile!");
                }
            }
        }

        Debug.Log("Grid generation complete!");
        return map;
    }

    private void AssignValuesForTile(int x, int z, Tile.Type normalType)
    {
        var tile = map.GridArray[x, z];
        tile.type = normalType;
        tile.SetMapObject(null);
        float y = GetShade(_pixelColor);
        tile.SetPosition(new Vector3(x, y, z));
    }
    void CreateResourceOnTheMap(int xPos, int zPos, int width, Tile.Type typeOnBmp, ResourceSO resourceSo)
    {
        var mapResource = new Map.Resource();
        mapResource.SetResourceSo(resourceSo);
        List<Tile> tiles = new List<Tile>();

        for (int x = xPos; x < xPos + width; x++)
        {
            for (int z = zPos; z < zPos + width; z++)
            {
                float y = 0;
                var tile = map.GridArray[x, z];
                tile.type = typeOnBmp;
                tile.SetMapObject(mapResource);
                tile.SetPosition(new Vector3(x, y, z));
                tiles.Add(tile);
            }

        mapResource.Tiles = tiles;
        if (typeOnBmp == Tile.Type.Crystal)
        {
            mapResource.SetResourceSo(this.resSo.crystal);
        }
        else if (typeOnBmp == Tile.Type.Elest)
        {
            mapResource.SetResourceSo(this.resSo.elest);
        }
        else if (typeOnBmp == Tile.Type.Rune)
        {
            mapResource.SetResourceSo(this.resSo.rune);
        }
        else if (typeOnBmp == Tile.Type.Core)
        {
            mapResource.SetResourceSo(this.resSo.core);
        }
        }
        map.mapResources.Add(mapResource);
    }
    void CreateCastleOnTheMap(int xPos, int zPos)
    {
        var mapCastle = new Map.Castle();
        const int width = 3;
        List<Tile> tiles = new List<Tile>();
        
        for (int x = xPos; x < xPos + width; x++)
        {
            for (int z = zPos; z < zPos + width; z++)
            {
                float y = 0;
                var tile = map.GridArray[x, z];
                tile.SetPosition(new Vector3(x, 0, z));
                tile.type = Tile.Type.Castle;
                tile.SetMapObject(mapCastle);
                tile.SetPosition(new Vector3(x, y, z));
                tiles.Add(tile);
            }
        }
        mapCastle.Tiles = tiles;
        map.mapCastles.Add(mapCastle);
    }
    
    float GetShade(Color pixelColor)
    {
        return (pixelColor.r + pixelColor.g + pixelColor.b) / 3f;
    }
}