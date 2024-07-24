using System;
using UnityEngine;

public class UnitRangeContr : MonoBehaviour
{
    private UnitId __currentTarget;
    public UnitId _currentTarget
    {
        get => __currentTarget;
        set
        {
            __currentTarget = value;
            inTargetRange = false;
        }
    }

    public bool inTargetRange;

    private void OnTriggerStay(Collider other)
    {
        UnitId unitId = other.GetComponent<UnitId>();
        if (unitId != null)
        {
            if (unitId == _currentTarget)
            {
                inTargetRange = true;
                Debug.Log("Target stay range: " + unitId.name);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        UnitId unitId = other.GetComponent<UnitId>();
        if (unitId != null)
        {
            if (unitId == _currentTarget)
            {
                inTargetRange = true;
                Debug.Log("Target entered range: " + unitId.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UnitId unitId = other.GetComponent<UnitId>();
        if (unitId != null)
        {
            if (unitId == _currentTarget)
            {
                inTargetRange = false;
                Debug.Log("Target exited range: " + unitId.name);
            }
        }
    }

    public void RefreshTarget()
    {
        throw new System.NotImplementedException();
    }
}