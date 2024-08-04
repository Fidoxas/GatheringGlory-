using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnnovirmentGen : MonoBehaviour
{
    [SerializeField, Range(0, 100)] int frequency = 50;
    
    public List<Vector3> spots = new List<Vector3>();
    public List<Vector3> _activeSpots = new List<Vector3>();
    [SerializeField] public GameObject prefab;
    void ActivateSpots()
    {
        _activeSpots.Clear();
        foreach (var spot in spots)
        {
            int r = Random.Range(0, 100);
            // Debug.Log(r.ToString());
            if (r > frequency)
            {
                Debug.Log("lol");
                _activeSpots.Add(spot);
            }
        }
    }
    [Button]
    public void GenerateEnvironment()
    {
        ActivateSpots();
        GameObject parent = new GameObject("Ennovirment");

        parent.transform.SetParent(this.transform.parent.transform);  

        foreach (var spot in _activeSpots)
        {
            GameObject obj = Instantiate(prefab, spot, quaternion.identity);
            obj.transform.SetParent(parent.transform);  
        }
    }
}
