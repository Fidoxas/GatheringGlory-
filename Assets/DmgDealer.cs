using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgDealer : MonoBehaviour
{
    private Player.Numbers pNum;
    [SerializeField] private int _damage = 10;

    private void Start()
    {
        pNum = GetComponent<UnitId>().pNum;
    }

    private void OnTriggerEnter(Collider other)
    {
        Live live = other.GetComponent<Live>();
        UnitId id = other.GetComponent<UnitId>();
        if (live != null && id != null && id.pNum != pNum)
        {
            Debug.Log("dmg to " + other.name);
            DealDamage(live);
        }
    }

    public void DealDamage(Live live)
    {
        live.TakeDamage(_damage);
    }
}