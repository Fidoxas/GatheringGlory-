using System;
using UnityEngine;

public class UnitRangeContr : MonoBehaviour
{
    private ObjSelector _currentTarget;
    public ObjSelector CurrentTarget
    {
        get => _currentTarget;
        set
        {
            _currentTarget = value;
            inTargetRange = false;
        }
    }

    public bool inTargetRange;

    private void OnTriggerStay(Collider other)
    {
        ObjSelector objSelector = other.GetComponent<ObjSelector>();
        if (objSelector != null)
        {
            if (objSelector == CurrentTarget)
            {
                inTargetRange = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ObjSelector objSelector = other.GetComponent<ObjSelector>();
        if (objSelector != null)
        {
            if (objSelector == CurrentTarget)
            {
                inTargetRange = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ObjSelector objSelector = other.GetComponent<ObjSelector>();
        if (objSelector != null)
        {
            if (objSelector == CurrentTarget)
            {
                inTargetRange = false;
                Debug.Log("Target exited range: " + objSelector.name);
            }
        }
    }

    public void RefreshTarget()
    {
        throw new System.NotImplementedException();
    }
}