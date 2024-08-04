using System.Collections;
using ScriptablesOBJ.Stages;
using UnityEngine;

public class AutoAttacker : MonoBehaviour
{
    private Player _owner;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackSpeed = 1.0f;
    private Coroutine _damageCoroutine;
    private IHaveHp _currentTarget; 
    private AutoFollower _autoFollower;

    private void Start()
    {
        _autoFollower = GetComponent<AutoFollower>();
        _owner = GetComponentInParent<OwnerShip>().player;
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
        IHaveHp living = other.GetComponent<IHaveHp>();
        ObjSelector objSelect = other.GetComponent<ObjSelector>();
        if (living != null && objSelect != null && objSelect.player != _owner)
        {
            if (living == _currentTarget && _damageCoroutine == null)
            {
                _damageCoroutine = StartCoroutine(DealDamageOverTime(living));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IHaveHp living = other.GetComponent<IHaveHp>();
        ObjSelector objSelect = other.GetComponent<ObjSelector>();
        if (living != null && objSelect != null && objSelect.player != _owner)
        {
            if (living == _currentTarget)
            {
                if (_damageCoroutine != null)
                {
                    StopCoroutine(_damageCoroutine);
                    _damageCoroutine = null;
                }
            }
        }
    }

    private IEnumerator DealDamageOverTime(IHaveHp living)
    {
        while (true)
        {
            DealDamage(living);
            yield return new WaitForSeconds(_attackSpeed);
        }
    }

    public void DealDamage(IHaveHp living)
    {
        living.TakeDamage(_damage, _owner);
    }

    public void SetTarget(IHaveHp target)
    {
        if (target != null && target != _currentTarget)
        {
            if (_damageCoroutine != null)
            {
                StopCoroutine(_damageCoroutine);
                _damageCoroutine = null;
            }
            _currentTarget = target;
        }
    }
}
