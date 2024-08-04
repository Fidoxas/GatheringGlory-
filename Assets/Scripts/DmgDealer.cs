using System.Collections;
using UnityEngine;

public class DmgDealer : MonoBehaviour
{
    private Player _owner;
    [SerializeField] private int dmg = 10;
    [SerializeField] private float speedAttack = 1f; 
    private bool _hitting;

    private void Start()
    {
        _owner = GetComponent<OwnerShip>().player;
    }

    public void Hit(GameObject targetUnitGameObject)
    {
        if (!_hitting)
        {
            IHaveHp targetLive = targetUnitGameObject.GetComponent<IHaveHp>();
            if (targetLive != null && targetLive.IsAlive()) 
            {
                StartCoroutine(Attack(targetLive));
            }
            else
            {
                Debug.Log("no target or target no hp" +
                          "");
            }
        }
    }

    private IEnumerator Attack(IHaveHp targetLive)
    {
        Debug.Log("attacking");
        _hitting = true;
        targetLive.TakeDamage(dmg,_owner); 
        yield return new WaitForSeconds(speedAttack);
        _hitting = false;
    }
}