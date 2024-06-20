using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChecker : MonoBehaviour
{
    public static Stage.Type CheckCurrentStage(Vector2 cords, int spacing, int stageRows)
    {
        Stage.Type type;
        int x = Mathf.FloorToInt(cords.x / spacing);
        int y = Mathf.FloorToInt(cords.y / spacing);
        int stagenum = x + (y * stageRows);

        type = (Stage.Type)stagenum;
        return type;
    }
}