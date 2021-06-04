using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CircularInstantiator : MonoBehaviour {
    public float heightOffset = 0;

    public Transform center;
    public Transform parent;
    public GameObject player;

    public GameObject asteroid;
    public GameObject crystal;

    [SerializeField] private float initialSpeedMultiplier = 3f;
    public double spawnAngleDistanceDegrees = 30f;
    private double playerAngleDegrees;
    private double previousSpawnAngleDegrees;
    private double playerDistance;
    private double previousPlayerAngle;


    private List<float> radiuses = new List<float> { 70f, 80f, 90f, 100f, 110f, 120f };

    private Dictionary<GameObject, double> instances;

    private System.Random random = new System.Random();

    private void Start() {
        instances = new Dictionary<GameObject, double>();
        previousSpawnAngleDegrees = 0;
        playerAngleDegrees = 0;
        playerDistance = 0;
        previousPlayerAngle = getAngleDegrees(player.transform.position, center.position);
        player.GetComponent<PlayerController>().forwardSpeed *= initialSpeedMultiplier;
    }

    private void Update() {
        playerAngleDegrees = getAngleDegrees(player.transform.position, center.position);

        if (playerDistance > 180) {
            player.GetComponent<PlayerController>().forwardSpeed = 40;
        }

        if (previousPlayerAngle > playerAngleDegrees) {
            playerDistance += playerAngleDegrees + 360 - previousPlayerAngle;
        } else {
            playerDistance += playerAngleDegrees - previousPlayerAngle;
        }

        previousPlayerAngle = playerAngleDegrees;

        // Debug.Log($"Player angle {playerAngleDegrees}");
        // Debug.Log($"Previous angle {previousSpawnAngleDegrees}");
        // Debug.Log($"Player Distance {playerDistance}");

        if (playerDistance - spawnAngleDistanceDegrees > previousSpawnAngleDegrees) {
            spawn(-previousSpawnAngleDegrees + 270);
            previousSpawnAngleDegrees += spawnAngleDistanceDegrees;
            removePassedInstances();
        }
    }

    private void removePassedInstances() {
        foreach (GameObject instance in instances.Keys) {
            if (instances[instance] < playerDistance) {
                Destroy(instance);
            } else {
                break;
            }
        }
    }

    protected virtual void spawn(double angle) {
        int asteroidsCount = UnityEngine.Random.Range(0, radiuses.Count - 2);
        int crystalsCount = UnityEngine.Random.Range(0, radiuses.Count - asteroidsCount);
        asteroidsCount = 3;
        crystalsCount = 3;
        List<float> usedRadiuses = new List<float>(radiuses);
        int radiusesCount = usedRadiuses.Count;

        foreach (int instanceCounter in Enumerable.Range(0, asteroidsCount + crystalsCount - 1)) {
            int index = UnityEngine.Random.Range(1, (usedRadiuses.Count));
            float radius = usedRadiuses[index];
            usedRadiuses.RemoveAt(index);

            GameObject target = instanceCounter < asteroidsCount ? asteroid : crystal;
            Vector3 point = getPointOnCircumference(radius, (float)angle, center.position);

            GameObject instance = Instantiate(target, point, Quaternion.identity, parent);
            instances.Add(instance, previousSpawnAngleDegrees + 360);
        }
    }

    public static double getAngleDegrees(Vector3 targetPosition, Vector3 centerPosition) {
        double playerAngleRad = Math.Atan2(
           targetPosition.x - centerPosition.x,
           targetPosition.z - centerPosition.z
       );
        return (playerAngleRad / Math.PI * 180) + (playerAngleRad < 0 ? 360 : 0);
    }

    public Vector3 getPointOnCircumference(float radius, float angle, Vector3 offset) {
        angle = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(angle) * radius + offset.x;
        float z = Mathf.Sin(angle) * radius + offset.z;
        return new Vector3(x, heightOffset, z);
    }
}
