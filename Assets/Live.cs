using UnityEngine;
using UnityEngine.UI;

public class Live : MonoBehaviour
{
    // public Player.Numbers pNum; // Zmienić na odpowiedni typ enum lub int, jeśli Player.Numbers nie istnieje.
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

        Slider healthBarSlider = healthBar.GetComponent<Slider>();
        if (healthBarSlider != null)
        {
            _healthBarLive = (int)healthBarSlider.value;
        }

        LoadPnumToShader();
    }

    private void LoadPnumToShader()
    {
       
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
        if (healthPoints <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}