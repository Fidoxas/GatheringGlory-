using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitId : MonoBehaviour
{
    public Player.Numbers pNum;
    public GameObject playerCircle;
    public GameObject actionCircle;
    public GameObject targetCircle;
    private bool _materialUpdated = false;
    private PlayerManager _playerManager;
    private List<GameObject> _focussed = new List<GameObject>();
    public void SetSide(Player.Numbers id)
    {
        pNum = id;
        _materialUpdated = false;
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        if (_materialUpdated) return;

        _playerManager = FindObjectOfType<PlayerManager>();
        if (playerCircle != null && playerCircle.GetComponent<Renderer>() != null)
        {
            Material material = playerCircle.GetComponent<SpriteRenderer>().material;
            if (material.shader.name == "Custom/Teamer")
            {
                material.SetInt("_PlayerNumber", (int)pNum);
            }
        }

        _materialUpdated = true;
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
        UpdateMaterial();
    }
    
}