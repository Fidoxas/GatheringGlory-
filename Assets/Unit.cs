using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float speed = 5f;
    [SerializeField] private GameObject Checker;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private float range;
    private Vector3 targetPosition;
    private Transform targetUnit;
    public bool isSelected = false;
    private Color originalColor;

    private enum State
    {
        Idle,
        Moving,
        Following,
        Auto
    }

    private State currentState = State.Idle;
    private Coroutine currentCoroutine;

    void Awake()
    {
        attackArea.transform.localScale = new Vector3(2 * range, attackArea.transform.localScale.y, 2 * range);
        originalColor = GetComponent<Renderer>().material.color;
    }

    public void MoveToGround(Vector3 destination)
    {
        targetPosition = destination;
        targetUnit = null;  // Clear any target unit
        currentState = State.Moving;
        Debug.Log($"MoveToGround called with destination: {destination}");

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(MoveToGroundTarget());
    }

    public void FollowUnit(UnitId target)
    {
        targetUnit = target.transform;
        currentState = State.Following;
        Debug.Log($"FollowUnit called with target: {target.name}");

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(FollowUnitTarget());
    }

    private IEnumerator MoveToGroundTarget()
    {
        Debug.Log("Started moving to ground target");

        while (currentState == State.Moving)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            Debug.Log($"Current position: {transform.position}, Target position: {targetPosition}, Distance: {distanceToTarget}");

            if (distanceToTarget < 0.5f) // Using a default small distance for ground movement
            {
                StopMoving();
            }
            yield return null;
        }
    }

    private IEnumerator FollowUnitTarget()
    {
        Debug.Log("Started following unit target");

        while (currentState == State.Following)
        {
            if (targetUnit != null)
            {
                targetPosition = targetUnit.position;
                float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
                Debug.Log($"Following unit. Current position: {transform.position}, Target position: {targetPosition}, Distance: {distanceToTarget}");

                if (distanceToTarget > range)
                {
                    float step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                }
            }
            else
            {
                Debug.LogWarning("Target unit is null in FollowUnitTarget coroutine.");
            }

            yield return null; 
        }
    }

    private bool isMoving()
    {
        return currentState == State.Moving || currentState == State.Following;
    }

    public void StopMoving()
    {
        currentState = State.Idle;
        Debug.Log("Stopped moving");
    }

    public void ToggleSelect()
    {
        isSelected = !isSelected;
        Checker.SetActive(isSelected);
        Debug.Log($"ToggleSelect called. isSelected: {isSelected}");
    }

    public void DeSelect()
    {
        isSelected = false;
        Checker.SetActive(isSelected);
        Debug.Log("DeSelect called");
    }
}
