using System.Collections;
using UnityEngine;

public class AutoAttacker : MonoBehaviour
{
    private Player.Numbers pNum;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackSpeed = 1.0f;
    private Coroutine _damageCoroutine;
    private Live _currentTarget; 
    private AutoFollower _autoFollower;

    private void Start()
    {
        _autoFollower = GetComponent<AutoFollower>();
        pNum = GetComponent<UnitId>().pNum;
    }

    private void Update()
    {
        if (_autoFollower != null && _autoFollower.target != null && _autoFollower.target != _currentTarget)
        {
            SetTarget(_autoFollower.target);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Live live = other.GetComponent<Live>();
        UnitId id = other.GetComponent<UnitId>();
        if (live != null && id != null && id.pNum != pNum)
        {
            if (live == _currentTarget)
            {
                Debug.Log("Starting damage to " + other.name);
                if (_damageCoroutine == null)
                {
                    _damageCoroutine = StartCoroutine(DealDamageOverTime(live));
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Live live = other.GetComponent<Live>();
        UnitId id = other.GetComponent<UnitId>();
        if (live != null && id != null && id.pNum != pNum)
        {
            if (live == _currentTarget)
            {
                Debug.Log("Stopping damage to " + other.name);
                if (_damageCoroutine != null)
                {
                    StopCoroutine(_damageCoroutine);
                    _damageCoroutine = null;
                }
            }
        }
    }

    private IEnumerator DealDamageOverTime(Live live)
    {
        while (true)
        {
            DealDamage(live);
            yield return new WaitForSeconds(_attackSpeed);
        }
    }

    public void DealDamage(Live live)
    {
        live.TakeDamage(_damage);
    }

    public void SetTarget(Live target)
    {
        if (target != null && target != _currentTarget)
        {
            if (_damageCoroutine != null)
            {
                StopCoroutine(_damageCoroutine);
                _damageCoroutine = null;
            }
            _currentTarget = target;
            Debug.Log("New target set: " + target.name);
        }
    }
}
