using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CustomizationManager : MonoBehaviour {

    [SerializeField] private TMPro.TMP_Text stellarPointsText;

    private void Awake() {
        StartCoroutine(Profile.fetchProfileInfo(response => {
            if (response.isError) {
                Debug.Log(MessageResponse.from(response).message);
            } else {
                Profile.ProfileResponse data = DataResponse<Profile.ProfileResponse>.from(response).data;
                setStellarPointsText(data.stellar_points);
            }
        }));
    }

    private void setStellarPointsText(int stellarPoints) {
        stellarPointsText.text = "Stellar Points: " + stellarPoints.ToString();
    }
}

