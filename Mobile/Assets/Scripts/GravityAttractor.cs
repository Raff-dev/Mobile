using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    public float gravityModifier = -12;
    public float rotationModifier = 50f;

    public void Attract(Transform body)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized;
        Vector3 localUp = body.up;

        body.GetComponent<Rigidbody>().AddForce(gravityUp * gravityModifier);

        Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation, targetRotation, rotationModifier * Time.deltaTime);
    }
}
