using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public const string DEFAULT_SKIN = "StarSparrow1";
    public const string PREFERENCE_SKIN = "skin";
    public const string PATH_SKINS = "skins";

    [SerializeField] private GameOverMenu gameOverMenu;

    public float forwardSpeedMagnifier = 5f;
    public float strafeSpeedMagnifier = 3f;
    public float hoverSpeedMagnifier = 2f;

    public float forwardAcceleration = 2.5f;
    public float strafeAcceleration = 5f;

    public float forwardSpeed = 20f;
    public float strafeSpeed;
    public float hoverSpeed;

    public float lookRateSpeed = 90f;

    public float minRadius = 74f;
    public float maxRadius = 125f;

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

        float rSquared = (float)(Math.Pow(transform.position.x, 2) + Math.Pow(transform.position.z, 2));

        if (rSquared > maxRadius * maxRadius) {
            strafeSpeed = 1.0f;
        } else if (rSquared < minRadius * minRadius) {
            strafeSpeed = -1.0f;
        }

        transform.Rotate(0.0f, strafeSpeed * lookRateSpeed * Time.deltaTime, 0.0f, Space.Self);
        transform.position += transform.forward * forwardSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag(Tag.Obstacle.ToString())) {
            Destroy(gameObject);
            gameOverMenu.gameObject.SetActive(true);
        }
    }
}
