using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // Referencja do obiektu gracza
    public float distance = 5.0f; // Odległość kamery od gracza
    public float height = 2.0f; // Wysokość kamery nad graczem
    public float damping = 5.0f; // Szybkość wygładzania ruchu kamery

    void LateUpdate()
    {
        if (!target)
            return;

        Vector3 wantedPosition = target.position + target.up * height - target.forward * distance;
        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

        transform.LookAt(target.position + target.up * height);
    }
}