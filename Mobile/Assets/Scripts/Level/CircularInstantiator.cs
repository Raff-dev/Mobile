using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CircularInstantiator : MonoBehaviour
{
    public int instancesPerSection = 0;
    public int sections = 1;
    public float heightOffset = 0;

    public Transform center;
    public Transform parent;
    public GameObject target;
    public List<float> radiuses;

    private System.Random random = new System.Random();

    void Start()
    {
        setUp();
        instantiate();
    }

    protected virtual void setUp()
    {
        if (this.center == null)
        {
            this.center = this.transform;
        }
    }

    protected virtual void instantiate()
    {
        float interval = 360f / this.sections;
        for (float angleStart = 0f; angleStart <= 360f; angleStart += interval)
        {
            foreach (int instanceCounter in Enumerable.Range(0, instancesPerSection))
            {
                float radius = this.radiuses[this.random.Next(this.radiuses.Count)];
                Vector3 point = getRandomPointOnCircumference(radius, angleStart, angleStart + interval);

                Instantiate(this.target, point, Quaternion.identity, this.parent);
            }
        }
    }

    public Vector3 getRandomPointOnCircumference(float radius, float maxAngleDegrees, float minAngleDegrees)
    {
        return getRandomPointOnCircumference(radius, maxAngleDegrees, minAngleDegrees, this.center.position);
    }

    public Vector3 getRandomPointOnCircumference(float radius, float maxAngleDegrees, float minAngleDegrees, Vector3 offset)
    {
        var angle = (random.Next() * (maxAngleDegrees - minAngleDegrees) + minAngleDegrees) / 180 * Mathf.PI;
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        return new Vector3(x + offset.x, heightOffset + offset.y, z + offset.z);
    }
}
