using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public int score = 0;
    [SerializeField] private TMPro.TMP_Text highScoreText;

    public void addScore(int value)
    {
        score += value;
        highScoreText.text = $"Score: " + score;
    }
}
