using UnityEngine;

namespace ScriptablesOBJ
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ResourceDB", order = 1)]
    public class ResourceDB : ScriptableObject
    {
        public Resource gold;
        public Resource copper;
        public Resource iron;
        public Resource diam;
    }
}
