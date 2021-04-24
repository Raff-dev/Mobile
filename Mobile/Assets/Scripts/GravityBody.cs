using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour
{
    // Start is called before the first frame update
    public GravityAttractor attractor;
    private Transform _transform;

    void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (attractor)
        {
            attractor.Attract(_transform);
        }
    }
}
