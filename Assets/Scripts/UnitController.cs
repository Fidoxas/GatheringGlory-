using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitController : MonoBehaviour
{
    public LayerMask groundLayer;
    public LayerMask unitLayer;
    public LayerMask uiLayer;

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

    private void HandleInput()
    {
        // Sprawdzanie, czy kliknięto element UI
        if (IsPointerOverUI()) return;

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
                    IssueOrders();
                }
                break;
        }
    }

    private bool IsPointerOverUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        return results.Count > 0;
    }

    private IEnumerator SelectUnits()
    {
        while (Input.GetButton("Fire1"))
        {
            HandleSelection();
            yield return null;
        }

        _currentState = _selectedUnits.Count > 0 ? State.GivingOrders : State.Idle;
    }

    private void HandleSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, unitLayer))
        {
            Unit unit = hit.collider.GetComponent<Unit>();
            if (unit == null) return;

            if (!_selectedUnits.Contains(unit))
            {
                unit.Select();
                _selectedUnits.Add(unit);
            }
        }
    }

    private void IssueOrders()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, unitLayer))
        {
            ObjSelector targetUnit = hit.collider.GetComponent<ObjSelector>();
            if (targetUnit != null)
            {
                FollowSelectedUnits(targetUnit);
                return;
            }
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            MoveSelectedUnits(hit.point);
        }
    }

    private void MoveSelectedUnits(Vector3 destination)
    {
        foreach (Unit unit in _selectedUnits)
        {
            RemoveUnitsFromOldTargets(unit);
            unit.MoveToGround(destination);
            unit.DeSelect();
        }
        _selectedUnits.Clear();
        _currentState = State.Idle;
    }

    private void FollowSelectedUnits(ObjSelector target)
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
        _currentState = State.Idle;
    }

    private void RemoveUnitsFromOldTargets(Unit unit)
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
                }
                break;
            }
        }
    }
}

public class Target
{
    public ObjSelector TargetId { get; set; }
    public List<Unit> Units { get; set; }

    public Target(ObjSelector target, List<Unit> units)
    {
        TargetId = target;
        Units = units;
    }
}
