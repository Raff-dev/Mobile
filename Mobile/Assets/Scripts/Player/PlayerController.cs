using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public const string DEFAULT_SKIN = "StarSparrow1";
    public const string PREFERENCE_SKIN = "skin";
    public const string PATH_SKINS = "skins";

    [SerializeField] private GameOverMenu gameOverMenu;

    [SerializeField] private float strafeSpeedMagnifier = 3f;
    [SerializeField] private float rollSpeedMagnifier = 3f;

    [SerializeField] private float strafeAcceleration = 5f;
    [SerializeField] private float rollAcceleration = 5f;

    [SerializeField] private float forwardSpeed = 20f;
    [SerializeField] private float lookRateSpeed = 90f;

    [SerializeField] private float minRadius = 80f;
    [SerializeField] private float maxRadius = 120f;

    [SerializeField] private float maxRollDegree = 35f;
    [SerializeField] private float rollBackSpeed = 30f;

    private float rollSpeed;
    private float strafeSpeed;
    private float horizontal = 0f;


    private GameObject playerModel;

    private void Awake() {
        loadPlayerSkin();
    }

    private void loadPlayerSkin() {
        string skinName = PlayerPrefs.HasKey(PREFERENCE_SKIN)
            ? PlayerPrefs.GetString(PREFERENCE_SKIN)
            : DEFAULT_SKIN;
        GameObject model = Resources.Load($"{PATH_SKINS}/{skinName}", typeof(GameObject)) as GameObject;
        playerModel = Instantiate(model, transform.position, Quaternion.identity, transform);
    }

    void Update() {
        horizontal = Input.GetAxis("Horizontal");
        strafeSpeed = Mathf.Lerp(strafeSpeed, horizontal * strafeSpeedMagnifier, strafeAcceleration * Time.deltaTime);

        float rSquared = (float)(Math.Pow(transform.position.x, 2) + Math.Pow(transform.position.z, 2));

        if (rSquared > maxRadius * maxRadius) {
            strafeSpeed = 1.0f;
        } else if (rSquared < minRadius * minRadius) {
            strafeSpeed = -1.0f;
        }

        if (playerModel) {
            float modelRotation = playerModel.transform.rotation.eulerAngles.z;
            if (modelRotation > 180) modelRotation -= 360;

            if (horizontal != 0 && Math.Abs(modelRotation) < maxRollDegree) {
                rollSpeed = Mathf.Lerp(rollSpeed, horizontal * rollSpeedMagnifier, rollAcceleration * Time.deltaTime);
                playerModel.transform.Rotate(0f, 0f, -rollSpeed * Time.deltaTime, Space.Self);

            } else if (Math.Abs(modelRotation) > 1f) {
                int direction = -(int)(Math.Abs(modelRotation) / modelRotation);
                rollBackSpeed = Mathf.Lerp(rollBackSpeed, rollSpeedMagnifier, rollAcceleration * Time.deltaTime);
                playerModel.transform.Rotate(0f, 0f, direction * rollBackSpeed * Time.deltaTime, Space.Self);
            }
        }

        transform.Rotate(0.0f, strafeSpeed * lookRateSpeed * Time.deltaTime, 0.0f, Space.Self);
        transform.position += transform.right * strafeSpeed * Time.deltaTime;
        transform.position += transform.forward * forwardSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag(Tag.Obstacle.ToString())) {
            Destroy(gameObject);
            gameOverMenu.gameObject.SetActive(true);
        }
    }
}
