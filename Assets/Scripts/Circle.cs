using UnityEngine;

public class CircleFollower : MonoBehaviour
{
    public GameObject circlePrefab; // Prefab kółka
    public Transform player; // Transform gracza
    public Vector3 offset; // Przesunięcie kółka od gracza

    private GameObject circleInstance;

    void Start()
    {
        // Instancjonujemy prefab kółka
        circleInstance = Instantiate(circlePrefab, player.position + offset, Quaternion.identity);
    }

    void Update()
    {
        // Aktualizujemy położenie kółka względem gracza z przesunięciem
        if (circleInstance != null)
        {
            circleInstance.transform.position = player.position + offset;
        }
    }
}