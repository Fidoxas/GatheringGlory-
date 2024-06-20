using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CastleGenerator : MonoBehaviour
{
    public static Vector2[] DrawCastlePlace(int spacing, int currentStageNum,
        int stageRows)
    {
        var (castleAreaCords, rangeXCastleArea, rangeYCastleArea) = SetCastleArea(currentStageNum, stageRows, spacing);
        castleAreaCords = DrawCastleGenArea(rangeXCastleArea, rangeYCastleArea, castleAreaCords);
        
        int startX = Random.Range(rangeXCastleArea.Start.Value, rangeXCastleArea.End.Value + 1);
        int startY = Random.Range(rangeYCastleArea.Start.Value, rangeYCastleArea.End.Value + 1);
        
        List<Vector2> castleCoords = new List<Vector2>();
        for (int y = startY; y < startY + 2; y++)
        {
            for (int x = startX; x < startX + 2; x++)
            {
                castleCoords.Add(new Vector2(x, y));
            }
        }
        // return castleAreaCords.ToArray();
        return castleCoords.ToArray();
    }
     private static (Vector2[], Range, Range) SetCastleArea(int stage, int stageRows, int spacing, int borderSpace = 3)
    {

        int leftUpCorner = 0;
        int rightUpCorner = stageRows - 1;
        int leftDownCorner = stageRows * (stageRows - 1);
        int rightDownCorner = stageRows * stageRows - 1;

        int[] upEdge = GetEdgeIndices(stageRows, 0);
        int[] downEdge = GetEdgeIndices(stageRows, stageRows * (stageRows - 1));
        int[] leftEdge = GetEdgeIndices(stageRows, 0, stageRows);
        int[] rightEdge = GetEdgeIndices(stageRows, stageRows - 1, stageRows);

        if (stage == leftUpCorner)
        {
            var coords = new Vector2[(spacing - borderSpace - 2) * (spacing - borderSpace - 2)];
            var x = new Range(1,spacing-borderSpace-2);
            var y = new Range(1,spacing- borderSpace-2);
            return (coords, x, y);
        }
        else if (stage == rightUpCorner)
        {
            var coords = new Vector2[(spacing - borderSpace - 1) * (spacing - borderSpace - 3) + 1];
            var x = new Range(stage*spacing+borderSpace, (stage+1)*spacing- 3);
            var y = new Range(1, spacing - borderSpace - 2);
            return (coords, x, y);
        }
        else if (stage == leftDownCorner)
        {
            var coords = new Vector2[(spacing - borderSpace - 2) * (spacing - borderSpace - 2)];
            var x = new Range(1, spacing - borderSpace - 2);
            var y = new Range(stage/stageRows*spacing + borderSpace, (stage/stageRows+1)*spacing-2);
            return (coords, x, y);
        }
        else if (stage == rightDownCorner)
        {
            var coords = new Vector2[(spacing - borderSpace - 2) * (spacing - borderSpace - 2)];
            var x = new Range(stage%stageRows*spacing+borderSpace, (stage%stageRows+1)*spacing- 3);
            var y = new Range(stage/stageRows*spacing + borderSpace, (stage/stageRows+1)*spacing-3);
            return (coords, x, y);
        }
        else if (upEdge.Contains(stage))
        {
            var coords = new Vector2[(spacing - 2 * borderSpace - 1) * (spacing - borderSpace - 2)];
            var x = new Range(stage*spacing+ borderSpace, (stage+ 1)*spacing - borderSpace - 2);
            var y = new Range(1, spacing - borderSpace - 2);
            return (coords, x, y);
        }
        else if (downEdge.Contains(stage))
        {
            var coords = new Vector2[(spacing - borderSpace -borderSpace-1) * (spacing - borderSpace-2)];
            var x = new Range(stage%stageRows*spacing+ borderSpace, (stage%stageRows+ 1)*spacing - borderSpace - 2);
            var y = new Range(stage/stageRows*spacing + borderSpace, ((stage/stageRows+1))*spacing-3);
            return (coords, x, y);
        }
        else if (leftEdge.Contains(stage))
        {
            var coords = new Vector2[(spacing - 2 * borderSpace - 1) * (spacing - borderSpace - 2)];
            var x = new Range(1, spacing - borderSpace - 2);
            var y = new Range(stage/stageRows*spacing+ borderSpace, (stage/stageRows+1)*spacing+ borderSpace);
            return (coords, x, y);
        }
        else if (rightEdge.Contains(stage))
        {
            var coords = new Vector2[((spacing - 2 * borderSpace) - 1) * (spacing - borderSpace - 2)];
            var x = new Range(stage%stageRows*spacing+borderSpace, (stage%stageRows+1)*spacing - 3);
            var y = new Range(stage/stageRows*spacing+ borderSpace, (stage/stageRows+1)*spacing - borderSpace - 2);
            return (coords, x, y);
        }
        else
        {
            throw new NotSupportedException();
        }
    }



    private static Vector2[]  DrawCastleGenArea(Range rangeXCastleArea, Range rangeYCastleArea,
        Vector2[] castleGenerateArea)
    {
        int index = 0;

        for (int y = rangeYCastleArea.Start.Value; y <= rangeYCastleArea.End.Value; y++)
        {
            for (int x = rangeXCastleArea.Start.Value; x <= rangeXCastleArea.End.Value; x++)
            {
                if (index < castleGenerateArea.Length)
                {
                    castleGenerateArea[index] = new Vector2(x, y);
                    index++;
                }
            }
        }


        return castleGenerateArea.ToArray();
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