using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptablesOBJ
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Nations", order = 1)]
    public class Nation : ScriptableObject
    {
        public GameObject castlePrefab;
        [FormerlySerializedAs("nation")] public Castle.Type kind;
    }
}