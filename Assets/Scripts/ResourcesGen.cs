// using System.Collections.Generic;
// using System.Linq;
// using JetBrains.Annotations;
// using ScriptablesOBJ;
// using UnityEngine;
// using Random = UnityEngine.Random;
//
// public class ResourcesGen : MonoBehaviour
// {
//     private List<Vector2> occupiedResTil = new List<Vector2>();
//     private int _stageNum;
//     private int _spacing;
//     private int _stageRows;
//     public ResourcesDB resourceDB;
//     [SerializeField] private GameObject markerPrefab;
//
//     public Stack<TerrainResource> CreateResourcesForStage(List<TerrainGen.OccupiedField> occupedFields, int currentStageNum, int xTiles)
//     {
//         _stageNum = currentStageNum;
//         _spacing = xTiles / 3;
//         _stageRows = 3;
//
//         List<int> corners = new List<int>
//         {
//             0,
//             _stageRows - 1,
//             _stageRows * (_stageRows - 1),
//             _stageRows * _stageRows - 1
//         };
//
//         if (corners.Contains(currentStageNum))
//         {
//             return GenerateGoodResource(occupedFields, _spacing);
//         }
//         else
//         {
//             return Generate2WeakResources(occupedFields, _spacing, _stageRows);
//         }
//     }
//
//     private Stack<TerrainResource> Generate2WeakResources(List<TerrainGen.OccupiedField> occupedFields, int spacing, int stageRows)
//     {
//         Stack<TerrainResource> terrainResources = new Stack<TerrainResource>();
//
//         (Vector2 copperCoord, List<Vector2> AroundCopper) = GenerateUniqueCoordinate(occupedFields, spacing);
//         Stack<Vector2> copperCoords = GenerateResourceCoords(copperCoord, 2, 2);
//         terrainResources.Push(new TerrainResource(copperCoords, resourceDB.crystal, AroundCopper));
//
//         (Vector2 ironCoord, List<Vector2> AroundIron) = GenerateUniqueCoordinate(occupedFields, spacing);
//         Stack<Vector2> ironCoords = GenerateResourceCoords(ironCoord, 2, 2);
//         terrainResources.Push(new TerrainResource(ironCoords, resourceDB.elest, AroundIron));
//
//         return terrainResources;
//     }
//
//     private Stack<TerrainResource> GenerateGoodResource(List<TerrainGen.OccupiedField> occupedFields, int spacing)
//     {
//         Stack<TerrainResource> terrainResources = new Stack<TerrainResource>();
//
//         (Vector2 goldCoord, List<Vector2> AroundGold) = GenerateUniqueCoordinate(occupedFields, spacing);
//         Stack<Vector2> goldCoords = GenerateResourceCoords(goldCoord, 2, 2); 
//         terrainResources.Push(new TerrainResource(goldCoords, resourceDB.rune, AroundGold));
//
//         return terrainResources;
//     }
//
//
//     public TerrainResource GenerateBestResource(List<TerrainGen.OccupiedField> occupedFields, int currentStageNum, int xTiles)
//     {
//         _stageNum = currentStageNum;
//     
//         (Vector2 diamondCoordsStart, List<Vector2> aroundDiamond) = GenerateUniqueSpCoordinate(occupedFields, xTiles);
//     
//         Stack<Vector2> diamondCoords = GenerateResourceCoords(diamondCoordsStart, 3, 3);
//         string coordsLog = "Diamond Coords: ";
//         foreach (var coord in diamondCoords)
//         {
//             coordsLog += "(" + coord.x + ", " + coord.y + ") " + "stagenum: " + _stageNum + " ";
//         }
//         // Debug.Log(coordsLog);
//
//         TerrainResource resource = new TerrainResource(diamondCoords, resourceDB.core, aroundDiamond);
//
//         return resource;
//     }
//
//
//     private Stack<Vector2> GenerateResourceCoords(Vector2 startCoord, int width, int height)
//     {
//         Stack<Vector2> coords = new Stack<Vector2>();
//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 coords.Push(new Vector2(startCoord.x + x, startCoord.y - y));
//             }
//         }
//         return coords;
//     }
//
//     private (Vector2 newCoord, List<Vector2> newCoordArea) GenerateUniqueCoordinate(List<TerrainGen.OccupiedField> occupedFields, int spacing)
//     {
//         Vector2 newCoord;
//         List<Vector2> newCoordArea = new List<Vector2>();
//         bool isValid = false;
//
//         do
//         {
//             // Generate a random coordinate within the stage bounds
//             newCoord = new Vector2(
//                 Random.Range((_stageNum % _stageRows * spacing) + 1, ((_stageNum % _stageRows + 1) * spacing) - 2),
//                 Random.Range((_stageNum / _stageRows * spacing) + 2, ((_stageNum / _stageRows + 1) * spacing) - 1)
//             );
//
//             newCoordArea.Clear();
//
//             Vector2 start = new Vector2(newCoord.x - 1, newCoord.y + 1);
//             for (int y = (int)start.y; y > start.y - 4; y--)
//             {
//                 for (int x = (int)start.x; x < start.x + 4; x++)
//                 {
//                     newCoordArea.Add(new Vector2(x, y));
//                 }
//             }
//
//             foreach (var field in occupedFields)
//             {
//                 var fieldCords = field.OccupiedTiles;
//                 isValid = !fieldCords.Intersect(newCoordArea).Any() &&
//                           !occupiedResTil.Intersect(newCoordArea).Any();
//             }
//
//         } while (!isValid);
//
//         // Genmarker(newCoordArea);
//         occupiedResTil.AddRange(newCoordArea);
//         return (newCoord, newCoordArea);
//     }
//
//     private (Vector2, List<Vector2>) GenerateUniqueSpCoordinate(List<TerrainGen.OccupiedField> occupedFields, int spacing, [CanBeNull] Vector2[] existingCoord = null)
//     {
//         Vector2 newCoord;
//         List<Vector2> newCoordArea = new List<Vector2>();
//         int definer = spacing / 2;
//         bool isValid = false;
//
//         do
//         {
//             newCoord = new Vector2(
//                 Random.Range((_stageNum % _stageRows * spacing) + definer, ((_stageNum % _stageRows + 1) * spacing - 1) - definer - 2),
//                 Random.Range((_stageNum / _stageRows * spacing) + definer+2, ((_stageNum / _stageRows + 1) * spacing - 1) - definer )
//             );
//
//             newCoordArea.Clear();
//             Vector2 start = new Vector2(newCoord.x - 1, newCoord.y + 1);
//             for (int y = (int)start.y; y > start.y - 3; y--)
//             {
//                 for (int x = (int)start.x; x < start.x + 3; x++)
//                 {
//                     newCoordArea.Add(new Vector2(x, y));
//                 }
//             }
//
//             foreach (var field in occupedFields)
//             {
//                 var fieldCords = field.OccupiedTiles;
//                 isValid = !fieldCords.Intersect(newCoordArea).Any() &&
//                           (existingCoord == null || !existingCoord.Contains(newCoord));
//             }
//
//         } while (!isValid);
//
//         return (newCoord, newCoordArea);
//     }
//
//
//     // void Genmarker(List<Vector2> positions)
//     // {
//     //     GameObject parentObject = new GameObject("ResourceMarkers");
//     //     parentObject.transform.SetParent(transform);
//     //
//     //     foreach (Vector2 position in positions)
//     //     {
//     //         Vector3 position3D = new Vector3(position.x + 0.5f, 1, position.y + 0.5f);
//     //
//     //         GameObject marker = Instantiate(markerPrefab, position3D, Quaternion.identity);
//     //         marker.transform.SetParent(parentObject.transform);
//     //     }
//     // }
//     public class TerrainResource
//     {
//         public Stack<Vector2> Coords { get; set; }
//         public List<Vector2> TilesAround { get; set;}
//         public ResourceSO ResourceSo { get; set; }
//         
//
//         public TerrainResource(Stack<Vector2> coords, ResourceSO resourceSo, List<Vector2> tilesAround)
//         {
//             TilesAround = tilesAround;
//             Coords = coords;
//             ResourceSo = resourceSo;
//         }
//
//        
//     }
// }
