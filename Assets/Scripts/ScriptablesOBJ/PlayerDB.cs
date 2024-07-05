using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptablesOBJ.Stages
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Player", order = 1)]
    public class PlayerDB : ScriptableObject
    {
        public Material material;
        public Player.Numbers pNum;
        public Stage.Type startStage;
        public Castle castle;
    }
}
