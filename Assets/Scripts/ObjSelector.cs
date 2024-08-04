using System;
using System.Collections.Generic;
using ScriptablesOBJ.Stages;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjSelector : MonoBehaviour 
{
    public Player player;
    public GameObject playerCircle;
    public GameObject actionCircle;
    public GameObject targetCircle;
    private ClientManager _clientManager;
    private List<GameObject> _focussed = new List<GameObject>();

    private void Awake()
    {
        _clientManager = FindObjectOfType<ClientManager>();
        if (_clientManager != null)
        {
            _clientManager.OnPlayerMatAssigned += HandlePlayerAssigned;
        }
    }

    private void HandlePlayerAssigned()
    {
        UpdateMaterial();
    }

    public void UpdateMaterial()
    {
        if (player!= null)
        {
            _clientManager = FindObjectOfType<ClientManager>();
            if (playerCircle != null && _clientManager != null)
            {
                if (player == _clientManager.MyPlayer)
                {
                    playerCircle.GetComponent<SpriteRenderer>().sharedMaterial = _clientManager.GetTeamMaterial();
                }
            }
        }
    }

    public void SetAsTarget()
    {
        targetCircle.SetActive(true);
    }

    public void DeselectTarget()
    {
        targetCircle.SetActive(false);
    }
    void Start()
    {
        actionCircle.SetActive(false);
        targetCircle.SetActive(false);
        if (player == null)
        {
            player = GetComponent<OwnerShip>().player;
        }
        UpdateMaterial();
    }

    [Button]
    public void Reload()
    {
        player = GetComponent<OwnerShip>().player;
        UpdateMaterial();
    }

    public void Select()
    {
        actionCircle.SetActive(true);
    }

    public void DeSelect()
    {
        actionCircle.SetActive(false);
    }
}