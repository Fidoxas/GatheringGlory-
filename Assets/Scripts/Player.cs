using ScriptablesOBJ;
using ScriptablesOBJ.Stages;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public string pname;
    [FormerlySerializedAs("playersNum")] public Numbers numbers;
    public Castles.Type type;

    public enum Numbers
    {
        None = -1,
        P1,
        P2,
        P3,
        P4,
        P5,
        P6,
        P7,
        P8
    }

    public static PlayerDB CheckCurrentPByStage(Stage.Type currentStage, PlayersDB playersDB)
    {
        PlayerDB currentPdb = null;
        foreach (var i in playersDB.playerDbs)
        {
            if (i.startStage == currentStage)
                currentPdb = i;
        }
        return currentPdb;

    }
}