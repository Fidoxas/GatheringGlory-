using UnityEngine;
using UnityEngine.UI;

public class Live : MonoBehaviour
{
    public Player.Numbers pNum;
    public int healthPoints = 100;
    [SerializeField] private GameObject healthBar;
    private Camera _cam;
    private int _healthBarLive;

    void Start()
    {
        GameObject player = GameObject.Find("Player");

        if (player != null)
        {
            _cam = player.GetComponentInChildren<Camera>();
        }
        if (_cam == null)
        {
            Debug.LogError("Camera not found as a child of Player object");
        }
        Slider healthBarSlider = healthBar.GetComponent<Slider>();
        if (healthBarSlider != null)
        {
            _healthBarLive = (int)healthBarSlider.value;
        }
        else
        {
            Debug.LogError("HealthBar GameObject does not have a Slider component.");
        }
    }

    void Update()
    {
        if (healthBar != null && _cam != null)
        {
            healthBar.transform.LookAt(_cam.transform);

            if (_healthBarLive != healthPoints)
            {
                _healthBarLive = healthPoints;
                Slider healthBarSlider = healthBar.GetComponent<Slider>();
                if (healthBarSlider != null)
                {
                    healthBarSlider.value = healthPoints;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
        if (healthPoints < 0)
        {
            healthPoints = 0;
            // Handle death or other logic here
        }
    }
}