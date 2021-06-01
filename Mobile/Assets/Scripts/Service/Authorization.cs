using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class Authorization {
    public const string PREFERENCE_REFRESH_TOKEN = "refresh";
    public const string PREFERENCE_ACCESS_TOKEN = "access";

    public const string AUTHORIZATION_HEADER_NAME = "Authorization";
    public const string AUTHORIZATION_HEADER_VALUE_PREFIX = "JWT ";

    public static IEnumerator login(WWWForm form, System.Action<Response> handleResponse) {
        UnityWebRequest request = UnityWebRequest.Post(ServiceUtil.LOGIN_URL, form);
        yield return request.SendWebRequest();

        Response response = getLoginResponse(request);
        handleResponse(response);
    }

    public static WWWForm createLoginForm(string email, string password) {
        WWWForm form = new WWWForm();
        form.AddField(ServiceUtil.EMAIL_FIELD_NAME, email);
        form.AddField(ServiceUtil.PASSWORD_FIELD_NAME, password);
        return form;
    }

    public static void setAuthorizationHeader(UnityWebRequest request) {
        string accessKey = AUTHORIZATION_HEADER_VALUE_PREFIX + PlayerPrefs.GetString(PREFERENCE_ACCESS_TOKEN);
        request.SetRequestHeader(AUTHORIZATION_HEADER_NAME, accessKey);
    }

    public static bool wasLoggedIn() {
        return PlayerPrefs.HasKey(PREFERENCE_REFRESH_TOKEN) && PlayerPrefs.HasKey(PREFERENCE_ACCESS_TOKEN);
    }

    public static IEnumerator verify(System.Action<MessageResponse> handleResponse) {
        if (!wasLoggedIn()) {
            handleResponse(MessageResponse.loggedOutError());
        }

        string accessToken = PlayerPrefs.GetString(PREFERENCE_ACCESS_TOKEN);
        WWWForm form = new WWWForm();
        form.AddField(ServiceUtil.TOKEN_FIELD_NAME, accessToken);

        UnityWebRequest request = UnityWebRequest.Post(ServiceUtil.VERIFY_URL, form);
        yield return request.SendWebRequest();

        MessageResponse response = (MessageResponse)getAuthResponse(request);
        handleResponse(response);
    }

    public static Response getAuthResponse(
        UnityWebRequest request,
        Func<UnityWebRequest, bool> isSuccess,
        Func<UnityWebRequest, Response> getReturn
    ) {

        if (isSuccess(request))
            return getReturn(request);

        else if (request.result == UnityWebRequest.Result.ConnectionError)
            return MessageResponse.connectionError();

        else if (request.responseCode == ServiceUtil.STATUS_400_BAD_REQUEST)
            return MessageResponse.badRequestError();

        else if (request.responseCode == ServiceUtil.STATUS_401_UNAUTHORIZED) {
            AuthResponse response = JsonUtility.FromJson<AuthResponse>(request.downloadHandler.text);
            return MessageResponse.unauthorizedError(response.detail);

        } else if (request.responseCode == ServiceUtil.STATUS_404_NOT_FOUND)
            return MessageResponse.notFoundError();

        else if (!wasLoggedIn())
            return MessageResponse.loggedOutError();

        return MessageResponse.unknownError();
    }

    public static Response getAuthResponse(UnityWebRequest request) {
        return getAuthResponse(
            request: request,
            isSuccess: request => request.responseCode == ServiceUtil.STATUS_200_OK,
            getReturn: request => MessageResponse.ok()
        );
    }

    private static Response getLoginResponse(UnityWebRequest request) {
        return getAuthResponse(
            request: request,
            isSuccess: request => request.responseCode == ServiceUtil.STATUS_200_OK,
            getReturn: request => {
                AuthResponse response = JsonUtility.FromJson<AuthResponse>(request.downloadHandler.text);
                return DataResponse<AuthResponse>.ok(data: response);
            }
        );
    }

    public static void logout() {
        PlayerPrefs.DeleteKey(PREFERENCE_ACCESS_TOKEN);
        PlayerPrefs.DeleteKey(PREFERENCE_REFRESH_TOKEN);
        PlayerPrefs.DeleteKey(PlayerController.PREFERENCE_SKIN);
    }

    public static void saveAuthorizationTokens(AuthResponse response) {
        PlayerPrefs.SetString(PREFERENCE_ACCESS_TOKEN, response.access);
        PlayerPrefs.SetString(PREFERENCE_REFRESH_TOKEN, response.refresh);
    }

    [Serializable]
    public class AuthResponse {
        public string refresh;
        public string access;
        public string detail;
    }
}
