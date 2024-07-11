using UnityEngine;

public class StructuresCreator : MonoBehaviour
{
    public static GameObject CreateCastle(Vector2[] castleCastleCords, GameObject obj, Player.Numbers pNum,int areaLen = 45)
    {
        if (castleCastleCords.Length != 4)
        {
            Debug.LogError("castleCastleCords should have exactly 4 coordinates.");
            return null;
        }

        Vector2[] convertedCords = CoordinatesConver.ConvertCoords(castleCastleCords, areaLen);
        float centroidX = convertedCords[1].x;
        float centroidZ = convertedCords[1].y;

        Vector3 centroid = new Vector3(centroidX, 0.5f, centroidZ);

        GameObject newObject = Instantiate(obj, centroid, obj.transform.rotation);
        newObject.GetComponent<UnitId>().pNum = pNum;
        
        return newObject;
    }
}