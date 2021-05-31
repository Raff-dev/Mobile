using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    [SerializeField] private TMPro.TMP_Text highScoreText;

    private void Awake() {
        StartCoroutine(Profile.fetchProfileInfo(response => {
            if (response.isError) {
                Debug.Log(MessageResponse.from(response).message);
            } else {
                Profile.ProfileResponse data = DataResponse<Profile.ProfileResponse>.from(response).data;
                Debug.Log(data.toString());

                setHighScoreText(data.high_score);
            }
        }));
    }

    private void setHighScoreText(int highScore) {
        highScoreText.text = $"High Score: {highScore}";
    }
}
