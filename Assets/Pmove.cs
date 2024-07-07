using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pmove : MonoBehaviour
{
    public float speed = 6.0f; // Prędkość ruchu

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;
        move *= speed * Time.deltaTime;

        rb.MovePosition(rb.position + move);
    }
}