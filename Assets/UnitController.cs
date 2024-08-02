using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public LayerMask groundLayer;
    public LayerMask unitLayer;
    private List<Unit> _selectedUnits = new List<Unit>();
    private List<Target> _targetsForUnits = new List<Target>();

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
            yield return null;
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
                if (unit.isSelected && !_selectedUnits.Contains(unit))
                {
                    _selectedUnits.Add(unit);
                }
                else if (!unit.isSelected && _selectedUnits.Contains(unit))
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
            RemoveUnitsFromOldTargets(unit);
            unit.MoveToGround(destination);
            unit.DeSelect();
        }
        _selectedUnits.Clear();
    }

    void FollowSelectedUnits(UnitId target)
    {
        Target targetObj = _targetsForUnits.Find(t => t.TargetId == target);

        if (targetObj == null)
        {
            targetObj = new Target(target, new List<Unit>());
            _targetsForUnits.Add(targetObj);
        }

        foreach (Unit unit in _selectedUnits)
        {
            RemoveUnitsFromOldTargets(unit);
            unit.FollowUnit(target);
            targetObj.Units.Add(unit);
            unit.DeSelect();
        }

        _selectedUnits.Clear();
        target.SetAsTarget();
    }

    void RemoveUnitsFromOldTargets(Unit unit)
    {
        foreach (var target in _targetsForUnits)
        {
            if (target.Units.Contains(unit))
            {
                target.Units.Remove(unit);
                if (target.Units.Count == 0)
                {
                    target.TargetId.DeselectTarget();
                    _targetsForUnits.Remove(target);
                    break;
                }
            }
        }
    }
}
public class Target
{
    public UnitId TargetId { get; set; }
    public List<Unit> Units { get; set; }

    public Target(UnitId target, List<Unit> units)
    {
        TargetId = target;
        Units = units;
    }
}