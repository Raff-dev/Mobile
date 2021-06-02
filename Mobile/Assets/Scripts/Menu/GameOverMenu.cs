using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameOverMenu : MenuPanel {
    public const string MENU_SCENE_NAME = "Menu";
    public const string GAME_SCENE_NAME = "orbital";

    private const float STELLAR_POINTS_RATIO = 0.1f;
    private const string STELLAR_POINTS_NAME = "stellar_points";
    private const string SCORE_NAME = "score";

    [SerializeField] private TMPro.TMP_Text stellarPointsGainText;
    [SerializeField] private TMPro.TMP_Text stellarPointsText;
    [SerializeField] private TMPro.TMP_Text scoreText;
    [SerializeField] private TMPro.TMP_Text highScoreText;
    [SerializeField] private ScoreCounter scoreCounter;

    private void Awake() {
        gameOver();
    }

    public void gameOver() {
        int stellarPointsGain = calculateStellarPointsGain(scoreCounter.score);
        setStellarPointsGainText(stellarPointsGain);
        setScoreText(scoreCounter.score);
        highScoreText.gameObject.SetActive(false);

        StartCoroutine(gainStellarPoints(stellarPointsGain, response => {
            if (response.isError) {
                Debug.Log(MessageResponse.from(response).message);
            } else {
                Profile.ProfileResponse data = DataResponse<Profile.ProfileResponse>.from(response).data;
                setStellarPointsText(data.stellar_points);
            }
        }));

        StartCoroutine(setHighScore(scoreCounter.score, response => {
            if (response.isError) {
                Debug.Log(MessageResponse.from(response).message);
            } else {
                Profile.ProfileResponse data = DataResponse<Profile.ProfileResponse>.from(response).data;

                if (data.is_high_score) {
                    highScoreText.gameObject.SetActive(true);
                }
            }
        }));
    }

    private IEnumerator gainStellarPoints(int stellarPointsGain, System.Action<Response> handleResponse) {
        WWWForm form = new WWWForm();
        form.AddField(STELLAR_POINTS_NAME, stellarPointsGain);

        UnityWebRequest request = UnityWebRequest.Post(ServiceUtil.STELLAR_POINTS_GAIN_URL, form);
        Authorization.setAuthorizationHeader(request);

        yield return request.SendWebRequest();
        Response response = Profile.getProfileResponse(request);
        handleResponse(response);
    }

    private IEnumerator setHighScore(int score, System.Action<Response> handleResponse) {
        WWWForm form = new WWWForm();
        form.AddField(SCORE_NAME, score);

        UnityWebRequest request = UnityWebRequest.Post(ServiceUtil.HIGH_SCORE_URL, form);
        Authorization.setAuthorizationHeader(request);

        yield return request.SendWebRequest();
        Response response = Profile.getProfileResponse(request);
        handleResponse(response);
    }

    private int calculateStellarPointsGain(int score) {
        return (int)(score * STELLAR_POINTS_RATIO);
    }

    public void setScoreText(int score) {
        scoreText.text = "Score: " + score.ToString();
    }

    public void setStellarPointsGainText(int stellarPointsGain) {
        stellarPointsGainText.text = "Stellar Points Gain: " + stellarPointsGain.ToString();
    }
    public void setStellarPointsText(int stellarPointsGain) {
        stellarPointsText.text = "Stellar Points: " + stellarPointsGain.ToString();
    }

    public void playAgain() {
        SceneManager.LoadScene(GAME_SCENE_NAME);
    }

    public void returnToMainMenu() {
        SceneManager.LoadScene(MENU_SCENE_NAME);
    }
}
