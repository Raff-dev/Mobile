using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscilatingObject : MonoBehaviour
{
    public CentripetalAttractor attractor;

    void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
    }

    void FixedUpdate()
    {
        if (attractor)
        {
            attractor.Oscillate(transform);
        }
    }
}
