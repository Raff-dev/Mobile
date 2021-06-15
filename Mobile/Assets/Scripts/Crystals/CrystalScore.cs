using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalScore : MonoBehaviour
{
    public int scoreValue = 10;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tag.Player.ToString())
        {
            other.GetComponent<ScoreCounter>().addScore(scoreValue);
            Destroy(gameObject);
        }
    }
}
