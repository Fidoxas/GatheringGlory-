using System;
using System.Collections;
using System.Collections.Generic;
using ScriptablesOBJ.Stages;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ResourceAmountReader : MonoBehaviour
{
    [FormerlySerializedAs("playerMan")] public ClientManager clientMan;
    public ResourceSO resource;
    public int amount;
    [SerializeField] private TextMeshProUGUI tmp;

    private void OnEnable()
    {
        if (tmp == null)
        {
            tmp = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }

        clientMan = FindObjectOfType<ClientManager>();
        clientMan.MyPlayer.OnValueReloaded += UpdateTextMesh;
        
    }

    private void OnDisable()
    {
        clientMan.MyPlayer.OnValueReloaded -= UpdateTextMesh;
    }

    private void UpdateTextMesh(Player updatedPlayer, ResourceSO updatedResource, int updatedAmount)
    {
        if (updatedPlayer == clientMan.MyPlayer && updatedResource == resource && tmp.text != updatedAmount.ToString())
        {
            tmp.text = updatedAmount.ToString();
        }
    }
}
