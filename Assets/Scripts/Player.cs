using System;
using System.Collections.Generic;
using ScriptablesOBJ.Stages;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] public PlayerSo playerSo;
    public event Action<Player,ResourceSO, int> OnValueReloaded;
    public Dictionary<ResourceSO, int> resources = new Dictionary<ResourceSO, int>();
    public GameObject castle;
    public GameObject Adept;

    public void Awake()
    {
        var pAssign =FindObjectOfType<PlayersAssign>().gameObject.transform;
        // this.transform.SetParent(pAssign);
        // Debug.Log("assigned");
    }

    public void CreateT1Unit()
    {
        if (resources.ContainsKey(playerSo.unitT1.resourceToMake) && 
            resources[playerSo.unitT1.resourceToMake] >= playerSo.unitT1.price)
        {
            resources[playerSo.unitT1.resourceToMake] -= playerSo.unitT1.price;
            ReloadValue(playerSo.unitT1.resourceToMake);
            if (castle!= null)
            {
                castle.GetComponent<Castle>().CreateUnit(playerSo.unitT1.prefab);
            }
        }
        else
        {
            Debug.Log("Not enough resources to create the unit.");
        }
    }

    public void ReloadValue(ResourceSO resource)
    {
        OnValueReloaded?.Invoke(this,resource,resources[resource]);
    }
}