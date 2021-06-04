using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentripetalAttractor : MonoBehaviour {
    public float rotationModifier = 5f;

    public void Oscillate(Transform body) {
        Vector3 localForward = body.forward;
        Vector3 toCenter = (transform.position - body.position).normalized;
        Vector3 perpendicular = Vector3.Cross(toCenter, Vector3.up).normalized;

        Quaternion targetRotation = Quaternion.FromToRotation(localForward, perpendicular) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation, targetRotation, rotationModifier * Time.deltaTime);
    }
}
