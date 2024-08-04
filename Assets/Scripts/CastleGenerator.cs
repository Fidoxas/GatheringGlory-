using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CastleGenerator : MonoBehaviour
{
    public static Vector2[] DrawCastlePlace(int xTiles, int currentStageNum)
    {
        var (castleAreaCords, rangeXCastleArea, rangeYCastleArea) = SetCastleArea(currentStageNum, xTiles);
        castleAreaCords = DrawCastleGenArea(rangeXCastleArea, rangeYCastleArea);

        Vector2 start = castleAreaCords[Random.Range(0, castleAreaCords.Length)];

        List<Vector2> castleCoords = new List<Vector2>();
        for (int y = (int)start.y; y > start.y - 3; y--)
        {
            for (int x = (int)start.x; x < start.x + 3; x++)
            {
                castleCoords.Add(new Vector2(x, y));
            }
        }

        // return castleAreaCords.ToArray();
        return castleCoords.ToArray();
    }
    
   private static (Vector2[], Range, Range) SetCastleArea(int stage, int xTiles, int borderSpace = 3)
   {
       int stageRows = 3;
       int spacing = xTiles / stageRows;
       
        int ldCorner = 0;
        int rdCorner = stageRows - 1;
        int luCorner = stageRows * (stageRows - 1);
        int ruCorner = stageRows * stageRows - 1;
   
        int[] dEdge = GetEdgeIndices(stageRows, 0);
        int[] uEdge = GetEdgeIndices(stageRows, stageRows * (stageRows - 1));
        int[] leftEdge = GetEdgeIndices(stageRows, 0, stageRows);
        int[] rightEdge = GetEdgeIndices(stageRows, stageRows - 1, stageRows);
   
        if (stage == ldCorner)
        {
            var coords = new Vector2[(spacing - borderSpace-1) * (spacing - borderSpace - 1)];
            var x = new Range(1,spacing-borderSpace-1);
            var y = new Range(1,spacing- borderSpace-1);
            return (coords, x, y);
        }
        else if (stage == rdCorner)
        {
            var coords = new Vector2[(spacing - borderSpace-1) * (spacing - borderSpace - 1)];

            var x = new Range(stage*spacing+borderSpace, (stage+1)*spacing- 2);
            var y = new Range(1,spacing- borderSpace-1);
            return (coords, x, y);
        }
        else if (stage == luCorner)
        {
            var coords = new Vector2[(spacing - borderSpace ) * (spacing - borderSpace - 1)];
            var x = new Range(1,spacing-borderSpace-1);
            var y = new Range(stage/stageRows*spacing + borderSpace-1, (stage/stageRows+1)*spacing-2);
            return (coords, x, y);
        }
        else if (stage == ruCorner)
        {
            var coords = new Vector2[(spacing - borderSpace - 1) * (spacing - borderSpace - 1)];
            var x = new Range(stage%stageRows*spacing+borderSpace, (stage%stageRows+1)*spacing- 2);
            var y = new Range(stage/stageRows*spacing + borderSpace-1, (stage/stageRows+1)*spacing-2);
            return (coords, x, y);
        }
        else if (dEdge.Contains(stage))
        {
            var coords = new Vector2[(spacing - 2 * borderSpace) * (spacing - borderSpace - 1)];
            var x = new Range(stage*spacing+ borderSpace, (stage+ 1)*spacing - borderSpace - 1);
            var y = new Range(1, spacing - borderSpace - 1);
            return (coords, x, y);
        }
        else if (uEdge.Contains(stage))
        {
            var coords = new Vector2[(spacing - 2 * borderSpace) * (spacing - borderSpace )];
            var x = new Range(stage%stageRows*spacing+ borderSpace, (stage%stageRows+ 1)*spacing - borderSpace - 1);
            var y = new Range(stage/stageRows*spacing + borderSpace-1, ((stage/stageRows+1))*spacing-2);
            return (coords, x, y);
        }
        else if (leftEdge.Contains(stage))
        {
            var coords = new Vector2[((spacing - 2 * borderSpace) - 1) * (spacing - borderSpace - 1)];
            var x = new Range(1,spacing-borderSpace-1);
            var y = new Range(stage/stageRows*spacing+ borderSpace, (stage/stageRows+1)*spacing -borderSpace-2);
            return (coords, x, y);
        }
        else if (rightEdge.Contains(stage))
        {
            var coords = new Vector2[((spacing - 2 * borderSpace) - 1) * (spacing - borderSpace - 1)];
            var x = new Range(stage%stageRows*spacing+borderSpace, (stage%stageRows+1)*spacing - 2);
            var y = new Range(stage/stageRows*spacing+ borderSpace, (stage/stageRows+1)*spacing - borderSpace - 2);
            return (coords, x, y);
        }
        else
        {
            throw new NotSupportedException();
        }
    }

   private static Vector2[] DrawCastleGenArea(Range rangeXCastleArea, Range rangeYCastleArea)
   {
       List<Vector2> newlist = new List<Vector2>();

       for (int y = rangeYCastleArea.Start.Value + 2; y <= rangeYCastleArea.End.Value; y++)
       {
           for (int x = rangeXCastleArea.Start.Value; x <= rangeXCastleArea.End.Value - 2; x++)
           {
               newlist.Add(new Vector2(x, y));
           }
       }

       return newlist.ToArray();
   }



    private static int[] GetEdgeIndices(int stageRows, int startIndex, int step = 1)
    {
        int[] edgeIndices = new int[stageRows];
        for (int i = 0; i < stageRows; i++)
        {
            edgeIndices[i] = startIndex + (i * step);
        }

        return edgeIndices;
    }
}