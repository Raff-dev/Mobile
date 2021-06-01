using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalScore : MonoBehaviour
{
    public int scoreValue = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.TAG_PLAYER)
        {
            other.GetComponent<ScoreCounter>().score += this.scoreValue;
            onCollection();
        }
    }

    protected virtual void onCollection()
    {
        Destroy(gameObject);
    }
}
