using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform PlayerTransform;

    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothModifier = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = transform.position - PlayerTransform.position;


    }

    // Update is called once per frame
    void Update()
    {

    }

    // LateUpdate is called after Update methods
    void LateUpdate()
    {
        Vector3 newPos = PlayerTransform.position + _cameraOffset;

        // transform.position = Vector3.Slerp(transform.rotation, PlayerTransform.rotation, SmoothModifier);
        transform.LookAt(PlayerTransform);
    }
}
