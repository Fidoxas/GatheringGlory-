using UnityEngine;
public class Player : MonoBehaviour
{
    public string pname;
    public PlayersNum playersNum;
    public Castles.Type type;

    public enum PlayersNum
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
}