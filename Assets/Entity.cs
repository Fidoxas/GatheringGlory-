using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Entity : MonoBehaviour,IHaveHp,ICanBeKilled
{
    [SerializeField] int health;
    [SerializeField] Slider healthBarSlider;

    public void TakeDamage(int amount, Player player)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
        if (healthBarSlider != null)
        {
            ReloadHp();
        }
    }

    public void ReloadHp()
    {
        healthBarSlider.value = health;
    }

    public void Heal(int amount)
    {
        health += amount;
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public void Die()
    {
        Debug.Log("Unit has died.");
        Destroy(this.gameObject);
    }
}
