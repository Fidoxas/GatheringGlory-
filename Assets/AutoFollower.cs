using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFollower : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float viewRange;
    [SerializeField] private float moveSpeed = 5f; // Speed at which the object will move towards the target
    [SerializeField] private GameObject attackArea;
    private Player.Numbers pNum;
    public Live target;  // Changed to public for simplicity

    private void Start()
    {
        attackArea.transform.localScale = new Vector3(range, attackArea.transform.localScale.y, range);
        pNum = GetComponent<UnitId>().pNum;
    }

    private Live FindNearestLiveObject()
    {
        Live nearestLive = null;
        float nearestDistance = Mathf.Infinity;
        Collider[] colliders = Physics.OverlapSphere(transform.position, viewRange);

        foreach (Collider collider in colliders)
        {
            Live live = collider.GetComponent<Live>();
            UnitId id = collider.GetComponent<UnitId>();
            if (live != null && id != null && id.pNum != pNum)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestLive = live;
                }
            }
        }

        return nearestLive;
    }

    private void Update()
    {
        if (target == null|| !target.gameObject.activeSelf )
        {
            target = FindNearestLiveObject();
        }
        else if (target != null)
        {
            GetComponentInChildren<AutoAttacker>().SetTarget(target);
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToTarget > range-2)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
}