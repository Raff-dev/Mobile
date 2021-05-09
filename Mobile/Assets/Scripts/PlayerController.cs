using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeedMagnifier = 5f;
    public float strafeSpeedMagnifier = 3f;
    public float hoverSpeedMagnifier = 2f;

    public float forwardAcceleration = 2.5f;
    public float strafeAcceleration = 2f;
    public float hoverAcceleration = 2f;

    public float forwardSpeed = 20f;
    public float strafeSpeed;
    public float hoverSpeed;

    public float lookRateSpeed = 90f;

    void Start()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        strafeSpeed = Mathf.Lerp(strafeSpeed, Input.GetAxis("Horizontal") * strafeSpeedMagnifier, forwardAcceleration * Time.deltaTime);
        hoverSpeed = Mathf.Lerp(hoverSpeed, Input.GetAxis("Vertical") * hoverSpeedMagnifier, hoverAcceleration * Time.deltaTime);

        transform.Rotate(
            hoverSpeed * lookRateSpeed * Time.deltaTime,
            strafeSpeed * lookRateSpeed * Time.deltaTime,
            0.0f,
            Space.Self
        );

        transform.position += transform.forward * forwardSpeed * Time.deltaTime;
    }
}
