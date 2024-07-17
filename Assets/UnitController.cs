using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public LayerMask groundLayer;
    public LayerMask unitLayer;
    private List<Unit> _selectedUnits = new List<Unit>();
    private bool Selecting = false;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Selecting = true;
            
        }
        if (Input.GetButtonUp("Fire1"))
        {
            Selecting = false;
        }

        if (Selecting)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
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
        if (_selectedUnits.Count != 0 && Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                MoveSelectedUnits(hit.point);
            }
        }
    }

    void MoveSelectedUnits(Vector3 destination)
    {
        List<Unit> unitsToDeselect = new List<Unit>();

        foreach (Unit unit in _selectedUnits)
        {
            unit.MoveTo(destination);
            unitsToDeselect.Add(unit);
        }

        foreach (Unit unit in unitsToDeselect)
        {
            unit.DeSelect();
            _selectedUnits.Remove(unit);
        }
    }
}
