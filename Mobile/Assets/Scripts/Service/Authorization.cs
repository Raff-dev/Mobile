using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class Authorization {
    public const string REFRESH_KEY_NAME = "refresh";
    public const string ACCESS_KEY_NAME = "access";

    public const string AUTHORIZATION_HEADER_NAME = "Authorization";
    public const string AUTHORIZATION_HEADER_VALUE_PREFIX = "JWT ";

    public static IEnumerator login(WWWForm form, System.Action<ResponseMessage> handleResponse) {
        UnityWebRequest request = UnityWebRequest.Post(ServiceUtil.LOGIN_URL, form);
        yield return request.SendWebRequest();

        ResponseMessage message = getResponseMessage(request);
        handleResponse(message);
    }

    private static ResponseMessage getResponseMessage(UnityWebRequest request) {
        if (request.result == UnityWebRequest.Result.ConnectionError) {
            return new ResponseMessage(ServiceUtil.ERROR_SOMETING_WRONG, true);
        } else if (request.responseCode == ServiceUtil.STATUS_200_OK) {
            return new ResponseMessage(null, request.downloadHandler.text, false);
        } else if (request.responseCode == ServiceUtil.STATUS_401_UNAUTHORIZED) {
            AuthorizationResponse response = JsonUtility.FromJson<AuthorizationResponse>(
                request.downloadHandler.text);

            if (response.detail != null) {
                return new ResponseMessage(response.detail, true);
            }
        }
        return new ResponseMessage(ServiceUtil.ERROR_UNKNOWN, true);
    }

    public static WWWForm createLoginForm(string email, string password) {
        WWWForm form = new WWWForm();
        form.AddField(ServiceUtil.EMAIL_FIELD_NAME, email);
        form.AddField(ServiceUtil.PASSWORD_FIELD_NAME, password);
        return form;
    }

    public static void setAuthorizationHeader(UnityWebRequest request, bool isRefresh) {
        string accessKey = AUTHORIZATION_HEADER_VALUE_PREFIX + (isRefresh
            ? PlayerPrefs.GetString(REFRESH_KEY_NAME)
            : PlayerPrefs.GetString(ACCESS_KEY_NAME));

        request.SetRequestHeader(AUTHORIZATION_HEADER_NAME, accessKey);
    }

    [Serializable]
    public class AuthorizationResponse {
        public string refresh;
        public string access;
        public string detail;
    }

    public static void logout() {
        PlayerPrefs.DeleteKey(ACCESS_KEY_NAME);
        PlayerPrefs.DeleteKey(REFRESH_KEY_NAME);
    }

    public static void saveAuthorizationTokens(string responseText) {
        AuthorizationResponse response = JsonUtility.FromJson<AuthorizationResponse>(responseText);
        PlayerPrefs.SetString(ACCESS_KEY_NAME, response.access);
        PlayerPrefs.SetString(REFRESH_KEY_NAME, response.refresh);
    }
}
