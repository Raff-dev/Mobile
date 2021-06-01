using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public const string DEFAULT_SKIN = "StarSparrow1";
    public const string PREFERENCE_SKIN = "skin";
    public const string PATH_SKINS = "skins";

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

    private void Awake() {
        loadPlayerSkin();
    }

    private void loadPlayerSkin() {
        string skinName = PlayerPrefs.HasKey(PREFERENCE_SKIN)
            ? PlayerPrefs.GetString(PREFERENCE_SKIN)
            : DEFAULT_SKIN;
        GameObject model = Resources.Load($"{PATH_SKINS}/{skinName}", typeof(GameObject)) as GameObject;
        Instantiate(model, transform.position, Quaternion.identity, transform);
    }

    void Update() {
        strafeSpeed = Mathf.Lerp(strafeSpeed, Input.GetAxis("Horizontal") * strafeSpeedMagnifier, strafeAcceleration * Time.deltaTime);
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
