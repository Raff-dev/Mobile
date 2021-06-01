using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomizationManager : MonoBehaviour {
    private string SKIN_FIELD_NAME = "name";
    private Color BUTTON_COLOR_LOCKED;
    private Color BUTTON_COLOR_UNLOCKED;

    [SerializeField] private TMPro.TMP_Text stellarPointsText;
    [SerializeField] private GameObject contentContainer;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private Button unlockButton;
    [SerializeField] private GameObject[] skins;

    private Dictionary<string, Button> skinButtons;
    private Dictionary<string, int> skinPrices;

    private List<string> ownedSKins;
    private string selected;

    private List<Texture2D> textures;

    private void OnEnable() {
        BUTTON_COLOR_LOCKED = new Color(0.38f, 0.38f, 0.38f);
        BUTTON_COLOR_UNLOCKED = new Color(1, 1, 1, 1);

        ownedSKins = new List<string>();
        skinButtons = new Dictionary<string, Button>();
        textures = new List<Texture2D>();
        selected = null;

        StartCoroutine(Profile.fetchProfileInfo(response => {
            if (response.isError) {
                Debug.Log(MessageResponse.from(response).message);
            } else {
                Profile.ProfileResponse data = DataResponse<Profile.ProfileResponse>.from(response).data;
                setStellarPointsText(data.stellar_points);

                ownedSKins = data.skins.Select(skin => skin.name).ToList();
                StartCoroutine(fetchSkinsInfo(response => {
                    if (response.isError) {
                        Debug.Log(MessageResponse.from(response).message);
                    } else {
                        SkinResponse data = DataResponse<SkinResponse>.from(response).data;
                        skinPrices = data.skins.ToDictionary(skin => skin.name, skin => skin.price);
                        loadPlayerSkins();
                    }
                }));
            }
        }));

    }

    private void setButtonColor(Button button, Color color) {
        ColorBlock cb = button.colors;
        cb.normalColor = color;
        button.colors = cb;
    }

    private void loadPlayerSkins() {
        foreach (GameObject skin in skins) {
            Button button = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, contentContainer.transform);

            skinButtons.Add(skin.name, button);

            if (!ownedSKins.Contains(skin.name)) {
                setButtonColor(button, BUTTON_COLOR_LOCKED);
            }

            if (PlayerPrefs.HasKey(PlayerController.PREFERENCE_SKIN) &&
                PlayerPrefs.GetString(PlayerController.PREFERENCE_SKIN).Equals(skin.name)) {
                button.Select();
            }

            SkinButton skinButton = button.GetComponent<SkinButton>();
            skinButton.priceText.text = skinPrices[skin.name].ToString();
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
        if (ownedSKins.Contains(skin.name)) {
            PlayerPrefs.SetString(PlayerController.PREFERENCE_SKIN, skin.name);
            selected = null;
            unlockButton.interactable = false;
        } else {
            selected = skin.name;
            unlockButton.interactable = true;
        }
    }

    public void unlock() {
        StartCoroutine(unlockSkin(selected, response => {
            if (response.isError) {
                Debug.Log(MessageResponse.from(response).message);
            } else {
                Profile.ProfileResponse data = DataResponse<Profile.ProfileResponse>.from(response).data;
                setStellarPointsText(data.stellar_points);

                ownedSKins.Add(selected);
                PlayerPrefs.SetString(PlayerController.PREFERENCE_SKIN, selected);

                Button button = skinButtons[selected];
                button.Select();
                setButtonColor(button, BUTTON_COLOR_UNLOCKED);

                unlockButton.interactable = false;
            }
        }));
    }

    private IEnumerator unlockSkin(string skinName, System.Action<Response> handleResponse) {
        WWWForm form = new WWWForm();
        form.AddField(SKIN_FIELD_NAME, skinName);

        UnityWebRequest request = UnityWebRequest.Post(ServiceUtil.UNLOCK_SKIN_URL, form);
        Authorization.setAuthorizationHeader(request);

        yield return request.SendWebRequest();
        Response response = getSkinUnlockResponse(request);
        handleResponse(response);
    }

    public static Response getSkinUnlockResponse(UnityWebRequest request) {
        return Authorization.getAuthResponse(
            request: request,
            isSuccess: request => request.responseCode == ServiceUtil.STATUS_201_CREATED,
            getReturn: request => {
                Profile.ProfileResponse response = JsonUtility.FromJson<Profile.ProfileResponse>(request.downloadHandler.text);
                return DataResponse<Profile.ProfileResponse>.ok(data: response);
            }
        );
    }

    private IEnumerator fetchSkinsInfo(System.Action<Response> handleResponse) {
        UnityWebRequest request = UnityWebRequest.Get(ServiceUtil.SKINS_LIST_URL);
        Authorization.setAuthorizationHeader(request);

        yield return request.SendWebRequest();
        Response response = getSkinInfoResponse(request);
        handleResponse(response);
    }

    public static Response getSkinInfoResponse(UnityWebRequest request) {
        return Authorization.getAuthResponse(
            request: request,
            isSuccess: request => request.responseCode == ServiceUtil.STATUS_200_OK,
            getReturn: request => {
                string json = "{\"skins\":" + request.downloadHandler.text + "}";
                SkinResponse response = JsonUtility.FromJson<SkinResponse>(json);
                Debug.Log(response.skins.Count);
                return DataResponse<SkinResponse>.ok(data: response);
            }
        );
    }

    [Serializable]
    public class SkinResponse {
        public List<Skin> skins;

        [Serializable]
        public class Skin {
            public string name;
            public int price;
        }
    }

}
