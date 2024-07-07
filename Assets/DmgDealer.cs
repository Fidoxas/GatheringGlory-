using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgDealer : MonoBehaviour
{
    public Player.Numbers pNum;
    [SerializeField] private int _damage = 10; 

    private void OnTriggerEnter(Collider other)
    {
        Live live = other.GetComponent<Live>();
        if (live != null && live.pNum != pNum)
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