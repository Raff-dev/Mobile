using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsBehavior : MonoBehaviour {
    public float speed = 200.0f;

    void Update() {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }
}
