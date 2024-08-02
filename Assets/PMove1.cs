using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pmove1 : MonoBehaviour
{
    [SerializeField] private Joystick _joystick; 
    [SerializeField] private Camera _camera; 
    public float moveSpeed = 5f;
    public float turnSpeed = 200f; 
    private Rigidbody rb;
    private Vector3 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float moveX = _joystick.Horizontal;
        float moveZ = _joystick.Vertical;

        if (moveX != 0 || moveZ != 0)
        {
            Vector3 forward = _camera.transform.forward;
            Vector3 right = _camera.transform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            Vector3 direction = forward * moveZ + right * moveX;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            movement = direction * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }
}