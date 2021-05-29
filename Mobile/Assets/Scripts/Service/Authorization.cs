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

        ResponseMessage message = getLoginResponseMessage(request);
        handleResponse(message);
    }

    private static ResponseMessage getLoginResponseMessage(UnityWebRequest request) {
        if (request.result == UnityWebRequest.Result.ConnectionError)
            return new ResponseMessage(ServiceUtil.ERROR_SOMETING_WRONG, ResponseMessage.ERROR);

        else if (request.responseCode == ServiceUtil.STATUS_200_OK)
            return new ResponseMessage(null, ResponseMessage.SUCCESS, request.downloadHandler.text);

        else if (request.responseCode == ServiceUtil.STATUS_401_UNAUTHORIZED) {
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(
                request.downloadHandler.text);

            if (response.detail != null)
                return new ResponseMessage(response.detail, ResponseMessage.ERROR);
        }

        return new ResponseMessage(ServiceUtil.ERROR_UNKNOWN, ResponseMessage.ERROR);
    }

    public static WWWForm createLoginForm(string email, string password) {
        WWWForm form = new WWWForm();
        form.AddField(ServiceUtil.EMAIL_FIELD_NAME, email);
        form.AddField(ServiceUtil.PASSWORD_FIELD_NAME, password);
        return form;
    }

    public static void setAuthorizationHeader(UnityWebRequest request) {
        string accessKey = AUTHORIZATION_HEADER_VALUE_PREFIX + PlayerPrefs.GetString(ACCESS_KEY_NAME);
        request.SetRequestHeader(AUTHORIZATION_HEADER_NAME, accessKey);
    }

    public static bool wasAuthorized() {
        return PlayerPrefs.HasKey(REFRESH_KEY_NAME) && PlayerPrefs.HasKey(ACCESS_KEY_NAME);
    }

    public static IEnumerator verify(System.Action<ResponseMessage> handleResponse) {
        if (!wasAuthorized()) {
            handleResponse(new ResponseMessage(ServiceUtil.WARNING_LOGGED_OUT, ResponseMessage.ERROR));
        }

        string accessToken = PlayerPrefs.GetString(ACCESS_KEY_NAME);
        WWWForm form = new WWWForm();
        form.AddField(ServiceUtil.TOKEN_FIELD_NAME, accessToken);

        UnityWebRequest request = UnityWebRequest.Post(ServiceUtil.VERIFY_URL, form);
        yield return request.SendWebRequest();

        ResponseMessage message = getVerifyResponseMessage(request);
        handleResponse(message);
    }

    private static ResponseMessage getVerifyResponseMessage(UnityWebRequest request) {
        if (request.result == UnityWebRequest.Result.ConnectionError)
            return new ResponseMessage(ServiceUtil.ERROR_SOMETING_WRONG, ResponseMessage.ERROR);

        else if (request.responseCode == ServiceUtil.STATUS_200_OK)
            return new ResponseMessage();

        else if (request.responseCode == ServiceUtil.STATUS_401_UNAUTHORIZED) {
            Debug.Log(request.downloadHandler.text);

            JWTResponse response = JsonUtility.FromJson<JWTResponse>(
                request.downloadHandler.text);

            if (response.detail != null)
                return new ResponseMessage(response.detail, ResponseMessage.ERROR);

        }
        return new ResponseMessage(ServiceUtil.ERROR_UNKNOWN, ResponseMessage.ERROR);
    }

    [Serializable]
    public class LoginResponse {
        public string refresh;
        public string access;
        public string detail;
    }

    [Serializable]
    public class JWTResponse {
        public string detail;
        public string code;
    }

    public static void logout() {
        PlayerPrefs.DeleteKey(ACCESS_KEY_NAME);
        PlayerPrefs.DeleteKey(REFRESH_KEY_NAME);
    }

    public static void saveAuthorizationTokens(string responseText) {
        LoginResponse response = JsonUtility.FromJson<LoginResponse>(responseText);
        PlayerPrefs.SetString(ACCESS_KEY_NAME, response.access);
        PlayerPrefs.SetString(REFRESH_KEY_NAME, response.refresh);
    }
}
