using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float speed = 5f;
    [SerializeField] private GameObject checker;
    [SerializeField] private Material Circle;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private float range;
    private Vector3 _targetPosition;
    private Transform _targetUnit;
    public bool isSelected = false;

    private enum State
    {
        Idle,
        Moving,
        Following,
        Auto
    }

    private State _currentState = State.Idle;
    private Coroutine _currentCoroutine;

    void Awake()
    {
        attackArea.transform.localScale = new Vector3(2 * range, attackArea.transform.localScale.y, 2 * range);
    }

    public void MoveToGround(Vector3 destination)
    {
        _targetPosition = destination;
        _targetUnit = null;
        _currentState = State.Moving;

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(MoveToGroundTarget());
    }

    public void FollowUnit(UnitId target)
    {
        Debug.Log("Follow unit");
        attackArea.GetComponent<UnitRangeContr>()._currentTarget = target;
        attackArea.GetComponent<UnitRangeContr>().inTargetRange=false;
        _targetUnit = target.transform;
        _currentState = State.Following;

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(FollowUnitTarget());
    }

    private IEnumerator MoveToGroundTarget()
    {
        while (_currentState == State.Moving)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);
            float distanceToTarget = Vector3.Distance(transform.position, _targetPosition);

            if (distanceToTarget < 0.5f) 
            {
                StopMoving();
            }
            yield return null;
        }
    }

    private IEnumerator FollowUnitTarget()
    {
        while (_currentState == State.Following)
        {
            if (_targetUnit != null)
            {
                _targetPosition = _targetUnit.position;

                if (!attackArea.GetComponent<UnitRangeContr>().inTargetRange)
                {
                    float step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);
                }
            }

            yield return null;
        }
    }

    private bool isMoving()
    {
        return _currentState == State.Moving || _currentState == State.Following;
    }

    public void StopMoving()
    {
        _currentState = State.Idle;
    }

    public void ToggleSelect()
    {
        if (!isSelected)
        {
            isSelected = true;
            checker.SetActive(isSelected);
        }
    }

    public void DeSelect()
    {
        if (isSelected)
        {
            isSelected = false;
            checker.SetActive(isSelected);
        }
    }
}
