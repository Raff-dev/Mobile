using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocity = 5;
    public float rotationRate = 500f;

    private Vector3 moveDirection = new Vector3(0, 0, 1).normalized;
    private float horizontal = 0f;

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        moveDirection = new Vector3(horizontal * velocity * Time.deltaTime, 0, 1).normalized;
    }

    private void FixedUpdate()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.MovePosition(rb.position + transform.TransformDirection(moveDirection) * velocity * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * Quaternion.Euler(-horizontal, horizontal, horizontal), rotationRate * Time.deltaTime);
    }

}
