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
        ObjSelector otherObjSelector = other.GetComponent<ObjSelector>();
        if (otherObjSelector != null)
        {

            ObjSelector parentObjSelector = gameObject.GetComponentInParent<ObjSelector>();
            if (parentObjSelector != null && otherObjSelector.player != parentObjSelector.player)
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