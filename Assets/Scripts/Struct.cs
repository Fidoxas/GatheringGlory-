using UnityEngine;
using UnityEngine.UI;

public class Struct : MonoBehaviour, IHaveHp, ICanBeCaptured
{
    [SerializeField] private int maxHealthPoints;
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private int health;

    private ObjSelector _objSelector;
    private OwnerShip _ownerShip;
    private void Start()
    {
        health = maxHealthPoints;
        
        if (_objSelector == null)
        {
            _objSelector = GetComponent<ObjSelector>();
            if (_objSelector == null)
            {
                Debug.LogWarning($"{gameObject.name} is missing ObjSelector component!");
            }
        }
        ReloadHp();
    }

    public void TakeDamage(int amount, Player player)
    {
        health -= amount;
        if (health <= 0)
        {
            TakeOver(player);
        }
        else
        {
            ReloadHp();
        }
    }

    public void ReloadHp()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealthPoints;
            healthBarSlider.value = Mathf.Clamp(health, 0, maxHealthPoints);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} is missing HealthBarSlider reference!");
        }
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public void TakeOver(Player player)
    {
        health = maxHealthPoints;
        ReloadHp();
        
        Debug.Log("Structure captured by player.");

        var ownershipComponent = GetComponent<OwnerShip>();
        if (ownershipComponent != null)
        {
            ownershipComponent.AssignOwnerShip(player);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} is missing OwnerShip component!");
        }

        if (_objSelector != null)
        {
            _objSelector.Reload();
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} is missing ObjSelector component when trying to reload!");
        }

        var resourceSource = GetComponent<ResourceSource>();
        if (resourceSource != null)
        {
            resourceSource.Restart();
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} is missing ResourceSource component!");
        }

        Debug.Log($"{gameObject.name} has been taken over by {player.name} and restored to full health!");
    }
}
