using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    public ClientManager PM;

    [SerializeField] private List<ResourceCountGUI> resources = new();

    [Serializable]
    public class ResourceCountGUI
    {
        public ResourceSO resourceSo;
        public TextMeshProUGUI textGUI;
        public int amount;
    }
    
    public class ResourceProducedEventArgs: EventArgs
    {
        public int amount;
        public ResourceSO resourceSo;
    }
    
    public void OnResourceProduced(ResourceSO resourceSo, int amountProduced)
    {
        var entry = resources.Find(resource => resource.resourceSo == resourceSo);
        entry.amount += amountProduced;
        entry.textGUI.text = entry.amount.ToString();
    }
}