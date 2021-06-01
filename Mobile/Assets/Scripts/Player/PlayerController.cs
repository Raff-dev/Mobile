using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeedMagnifier = 5f;
    public float strafeSpeedMagnifier = 3f;
    public float hoverSpeedMagnifier = 2f;

    public float forwardAcceleration = 2.5f;
    public float strafeAcceleration = 5f;
    public float hoverAcceleration = 2f;

    public float forwardSpeed = 20f;
    public float strafeSpeed;
    public float hoverSpeed;

    public float lookRateSpeed = 90f;

    public float minRadius = 74f;
    public float maxRadius = 125f;


    public float X;
    public float Z;
    public float Y;


    public float NewSpeed;

    void Start()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {


        strafeSpeed = Mathf.Lerp(strafeSpeed, Input.GetAxis("Horizontal") * strafeSpeedMagnifier, strafeAcceleration * Time.deltaTime);
        hoverSpeed = Mathf.Lerp(hoverSpeed, Input.GetAxis("Vertical") * hoverSpeedMagnifier, hoverAcceleration * Time.deltaTime);

        X = transform.position.x;
        Z = transform.position.z;
        Y = transform.position.y;

        if (X*X + Z*Z > maxRadius*maxRadius)
        {
            strafeSpeed = 1.0f;

        } else if(X*X + Z*Z < minRadius*minRadius)
        {
            strafeSpeed = -1.0f;
        }
 

        transform.Rotate(
           0.0f,
           strafeSpeed * lookRateSpeed * Time.deltaTime,
           0.0f,
           Space.Self
       );
        transform.position += transform.forward * forwardSpeed * Time.deltaTime;

    }
}
