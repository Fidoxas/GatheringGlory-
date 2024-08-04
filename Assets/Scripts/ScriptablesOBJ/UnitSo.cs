using UnityEngine;

namespace ScriptablesOBJ
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Unit", order = 1)]
    public class UnitSo : ScriptableObject
    {
        public Tier tier;
        public GameObject prefab;
        public ResourceSO resourceToMake;
        public int price;


        public enum Tier
        {
            Tier1 = 1,
            Tier2 = 2,
            Tier3 = 3,
            Tier4 = 4,
            Tier5 = 5
        }
    }
}