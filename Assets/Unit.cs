using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float speed = 5f;

    [SerializeField] private GameObject Checker;
    private Vector3 targetPosition;
    private bool isMoving = false;
    public bool isSelected = false;
    private Color originalColor;

    void Awake()
    {
        originalColor = GetComponent<Renderer>().material.color;
    }

    public void MoveTo(Vector3 destination)
    {
        targetPosition = destination;
        Debug.Log($"MoveTo called with destination: {destination}");

        if (!isMoving)
        {
            StartCoroutine(MoveToTarget());
        }
    }

    private IEnumerator MoveToTarget()
    {
        isMoving = true;
        Debug.Log("Started moving");

        while (isMoving)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            Debug.Log($"Current position: {transform.position}, Target position: {targetPosition}, Distance: {Vector3.Distance(transform.position, targetPosition)}");

            if (Vector3.Distance(transform.position, targetPosition) < 1f)
            {
                StopMoving();
            }
            yield return null;
        }
    }

    public void StopMoving()
    {
        isMoving = false;
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