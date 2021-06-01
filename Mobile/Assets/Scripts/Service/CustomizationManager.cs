using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomizationManager : MonoBehaviour {
    [SerializeField] private int SKIN_THUMBNAIL_WIDTH = 100;
    [SerializeField] private int SKIN_THUMBNAIL_HEIGHT = 100;

    [SerializeField] private TMPro.TMP_Text stellarPointsText;
    [SerializeField] private GameObject contentContainer;
    [SerializeField] private UnityEngine.UI.Button buttonPrefab;
    [SerializeField] private GameObject[] skins;

    private List<Texture2D> textures;

    private void OnEnable() {
        textures = new List<Texture2D>();

        StartCoroutine(Profile.fetchProfileInfo(response => {
            if (response.isError) {
                Debug.Log(MessageResponse.from(response).message);
            } else {
                Profile.ProfileResponse data = DataResponse<Profile.ProfileResponse>.from(response).data;
                setStellarPointsText(data.stellar_points);
            }
        }));

        foreach (GameObject skin in skins) {
            UnityEngine.UI.Button button = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, contentContainer.transform);
            if (PlayerPrefs.HasKey(PlayerController.PREFERENCE_SKIN) &&
                PlayerPrefs.GetString(PlayerController.PREFERENCE_SKIN).Equals(skin.name)) {
                button.Select();
            }
            SkinButton skinButton = button.GetComponent<SkinButton>();

            skinButton.skin = skin;
            skinButton.customizationManager = this;

            Texture2D thumbnail = RuntimePreviewGenerator.GenerateModelPreview(skin.transform, 1024, 1024);
            textures.Add(thumbnail);
            Rect rect = new Rect(0, 0, thumbnail.width, thumbnail.height);
            button.GetComponent<Image>().sprite = Sprite.Create(thumbnail, rect, Vector2.down, 1);
        }
    }

    private void OnDisable() {
        foreach (Texture2D texture in textures) {
            Destroy(texture);
        }
        textures.Clear();

        foreach (Transform child in contentContainer.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void setStellarPointsText(int stellarPoints) {
        stellarPointsText.text = "Stellar Points: " + stellarPoints.ToString();
    }

    public void select(GameObject skin) {
        PlayerPrefs.SetString(PlayerController.PREFERENCE_SKIN, skin.name);
    }
}


