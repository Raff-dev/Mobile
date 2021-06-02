using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsBehavior : MonoBehaviour {
    public float rotationSpeed = 200.0f;

    void Update() {
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }
}
