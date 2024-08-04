using UnityEngine;

namespace ScriptablesOBJ
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ResourceDB", order = 1)]
    public class ResourcesDB : ScriptableObject
    {
        public ResourceSO crystal;
        public ResourceSO elest;
        public ResourceSO rune;
        public ResourceSO core;
    }
}
