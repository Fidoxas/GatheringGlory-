using System.Collections;
using ScriptablesOBJ.Stages;
using UnityEngine;

public class AutoFollower : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float viewRange;
    [SerializeField] private float moveSpeed = 5f; // Speed at which the object will move towards the target
    [SerializeField] private GameObject attackArea;
    private Player _player;
    public IHaveHp target; 

    private void Start()
    {
        attackArea.transform.localScale = new Vector3(range, attackArea.transform.localScale.y, range);
        _player = GetComponent<ObjSelector>().player;
    }

    private IHaveHp FindNearestLivingObject()
    {
        IHaveHp nearestLiving = null;
        float nearestDistance = Mathf.Infinity;
        Collider[] colliders = Physics.OverlapSphere(transform.position, viewRange);

        foreach (Collider collider in colliders)
        {
            IHaveHp living = collider.GetComponent<IHaveHp>();
            ObjSelector id = collider.GetComponent<ObjSelector>();
            if (living != null && id != null && id.player != _player)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestLiving = living;
                }
            }
        }

        return nearestLiving;
    }

    private void Update()
    {
        if (target == null || !((MonoBehaviour)target).gameObject.activeSelf)
        {
            target = FindNearestLivingObject();
        }
        else if (target != null)
        {
            GetComponentInChildren<AutoAttacker>().SetTarget(target);
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, ((MonoBehaviour)target).transform.position);
        if (distanceToTarget > range - 2)
        {
            Vector3 direction = (((MonoBehaviour)target).transform.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
}
