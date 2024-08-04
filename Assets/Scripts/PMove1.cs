using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class Pmove1 : NetworkBehaviour
{
    [SerializeField] public Joystick _joystick;
    [SerializeField] public Camera _camera; 
    public float moveSpeed = 5f;
    public float turnSpeed = 200f; 
    private Rigidbody rb;
    private Vector3 movement;
    public bool movingActive;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (_joystick == null)
        {
            movingActive = false;
        }

        if (_camera == null)
        {
            _camera = Camera.main;
        }
    }

    private void Update()
    {
        if (!IsOwner)
        {
            Debug.Log("not an owner");
            return;
        }
        if (movingActive)
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

                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

                movement = direction * moveSpeed * Time.deltaTime;
                Debug.Log("Calling move to server");
                MoveRigidBodyServerRpc(movement);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void MoveRigidBodyServerRpc(Vector3 movement)
    {
        Debug.Log("Moving player on server by " + movement);
        MoveRigidBodyClientRpc(movement);
    }
    [ClientRpc()]
    private void MoveRigidBodyClientRpc(Vector3 movement)
    {
        Debug.Log("Moving player on clinet by " + movement);
        rb.MovePosition(rb.position + movement);
    }

}