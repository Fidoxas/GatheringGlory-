using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewArea : MonoBehaviour
{
    public List<GameObject> SeenUnits = new List<GameObject>();
    private Collider coll;

    private void Start()
    {
        coll = GetComponent<Collider>();
        if (coll == null || !coll.isTrigger)
        {
            throw new InvalidOperationException("Collider must be assigned and set as a Trigger.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        UnitId otherUnitId = other.GetComponent<UnitId>();
        if (otherUnitId != null)
        {

            UnitId parentUnitId = gameObject.GetComponentInParent<UnitId>();
            if (parentUnitId != null && otherUnitId.pNum != parentUnitId.pNum)
            {
                Debug.Log("iSee YOu");
                if (!SeenUnits.Contains(other.gameObject))
                {
                    SeenUnits.Add(other.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (SeenUnits.Contains(other.gameObject))
        {
            SeenUnits.Remove(other.gameObject);
        }
    }
}