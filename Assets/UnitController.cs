using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public LayerMask groundLayer;
    public LayerMask unitLayer;
    private HashSet<Unit> _selectedUnits = new HashSet<Unit>();

    private enum State
    {
        Idle,
        Selecting,
        GivingOrders
    }

    private State _currentState = State.Idle;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        switch (_currentState)
        {
            case State.Idle:
                if (Input.GetButtonDown("Fire1"))
                {
                    _currentState = State.Selecting;
                    StartCoroutine(SelectUnits());
                }
                break;

            case State.GivingOrders:
                if (Input.GetButtonDown("Fire1"))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
                    {
                        UnitId targetUnit = hit.collider.GetComponent<UnitId>();
                        if (targetUnit != null)
                        {
                            FollowSelectedUnits(targetUnit);
                            _currentState = State.Idle;
                        }
                    }
                    else if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                    {
                        MoveSelectedUnits(hit.point);
                        _currentState = State.Idle;
                    }
                }
                break;
        }
    }

    IEnumerator SelectUnits()
    {
        while (Input.GetButton("Fire1"))
        {
            HandleSelection();
            yield return null; // wait until the next frame
        }

        _currentState = _selectedUnits.Count > 0 ? State.GivingOrders : State.Idle;
    }

    void HandleSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, unitLayer))
        {
            Unit unit = hit.collider.GetComponent<Unit>();
            if (unit != null)
            {
                unit.ToggleSelect();
                if (unit.isSelected)
                {
                    _selectedUnits.Add(unit);
                }
                else
                {
                    _selectedUnits.Remove(unit);
                }
            }
        }
    }

    void MoveSelectedUnits(Vector3 destination)
    {
        foreach (Unit unit in _selectedUnits)
        {
            unit.MoveToGround(destination);
            unit.DeSelect();
        }
        _selectedUnits.Clear();
    }

    void FollowSelectedUnits(UnitId target)
    {
        foreach (Unit unit in _selectedUnits)
        {
            unit.FollowUnit(target);
            unit.DeSelect();
        }
        _selectedUnits.Clear();
    }
}
