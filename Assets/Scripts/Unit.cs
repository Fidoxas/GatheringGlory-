using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour, IHaveHp, ICanBeKilled
{
    private Player _owner;
    public float speed = 5f;

    [SerializeField] private GameObject attackArea;
    [SerializeField] private float range;
    [SerializeField] public ObjSelector objSelector;
    private int _health = 100;

    private Vector3 _targetPosition;
    private Transform _targetUnit;
    public bool isSelected = false;

    private enum State
    {
        Rest,
        Idle,
        Moving,
        Following,
        Auto
    }

    private State _currentState = State.Idle;
    private Coroutine _currentCoroutine;
    private DmgDealer _dmgDealer;
    private Slider _healthBarSlider;

    void Awake()
    {
        attackArea.transform.localScale = new Vector3(2 * range, attackArea.transform.localScale.y, 2 * range);
    }

    private void Start()
    {
        _dmgDealer = GetComponent<DmgDealer>();
        _owner = GetComponent<OwnerShip>().player;
        _healthBarSlider = GetComponentInChildren<Slider>();
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

    public void FollowUnit(ObjSelector target)
    {
        if (target == null) return;

        if (target.GetComponent<OwnerShip>().player != _owner)
        {
            FollowEnemyTarget(target);
        }
        else
        {
            FollowAllyTarget(target);
        }
    }

    public void FollowEnemyTarget(ObjSelector target)
    {
        Debug.Log("Following enemy unit");
        _targetUnit = target.transform;
        _currentState = State.Following;

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(FollowEnemyCoroutine());
    }

    public void FollowAllyTarget(ObjSelector target)
    {
        Debug.Log("Following ally unit");
        _targetUnit = target.transform;
        _currentState = State.Following;

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(FollowAllyCoroutine());
    }

    private IEnumerator MoveToGroundTarget()
    {
        bool waitingForTargetToMoveAway = false;

        while (true)
        {
            float distanceToTarget = Vector3.Distance(transform.position, _targetPosition);

            if (!waitingForTargetToMoveAway && distanceToTarget < 0.5f)
            {
                StopMoving();
                waitingForTargetToMoveAway = true;
            }
            else if (waitingForTargetToMoveAway && distanceToTarget >= 4f)
            {
                _currentState = State.Moving;
                waitingForTargetToMoveAway = false;
            }

            if (!waitingForTargetToMoveAway && _currentState == State.Moving)
            {
                Vector3 direction = (_targetPosition - transform.position).normalized;

                if (direction != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 5f);
                }

                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);
            }

            yield return null;
        }
    }

    private IEnumerator FollowEnemyCoroutine()
    {
        while (_currentState == State.Following && _targetUnit != null)
        {
            _targetPosition = _targetUnit.position;
            float distanceToTarget = Vector3.Distance(transform.position, _targetPosition);

            if (distanceToTarget > range)
            {
                Vector3 direction = (_targetPosition - transform.position).normalized;

                if (direction != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 5f);
                }

                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);
            }
            else if (distanceToTarget <= range && _targetUnit.GetComponent<OwnerShip>().player != _owner)
            {
                _dmgDealer.Hit(_targetUnit.gameObject); // Attack enemy within range
            }

            if (_targetUnit == null)
            {
                StopMoving();
                yield break;
            }

            yield return null;
        }

        StopMoving();
    }

    private IEnumerator FollowAllyCoroutine()
    {
        float extendedRange = range * 1.5f;
        bool waitingForAllyToMove = false;

        while (_targetUnit != null)
        {
            _targetPosition = _targetUnit.position;
            float distanceToTarget = Vector3.Distance(transform.position, _targetPosition);

            if (distanceToTarget > range && !waitingForAllyToMove)
            {
                Vector3 direction = (_targetPosition - transform.position).normalized;

                if (direction != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 5f);
                }

                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);
            }
            else if (distanceToTarget > extendedRange)
            {
                _currentState = State.Following;
                waitingForAllyToMove = false;
            }
            else if (distanceToTarget <= range)
            {
                waitingForAllyToMove = true;
                StopMoving();
            }
            yield return null;
        }

        StopMoving();
    }

    public void StopMoving()
    {
        _currentState = State.Idle;
    }

    public void TakeDamage(int amount, Player player)
    {
        _health -= amount;
        if (_health <= 0)
        {
            Die();
        }
        if (_healthBarSlider != null)
        {
            ReloadHp();
        }
    }

    public void ReloadHp()
    {
        _healthBarSlider.value = _health;
    }

    public void Heal(int amount)
    {
        _health += amount;
    }

    public bool IsAlive()
    {
        return _health > 0;
    }

    public void Die()
    {
        Debug.Log("Unit has died.");
        Destroy(gameObject);
    }

    public void Select()
    {
        objSelector.Select();
        isSelected = true;
    }

    public void DeSelect()
    {
        objSelector.DeSelect();
        isSelected = false;
    }
}
