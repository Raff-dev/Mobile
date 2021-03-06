using System.Net.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class Registration {
    public const string SUCCESS_ACCOUNT_CREATED = "You have succesfully created an account.\nPlease verify your email.";

    public static IEnumerator register(WWWForm form, System.Action<MessageResponse> handleResponse) {
        UnityWebRequest request = UnityWebRequest.Post(ServiceUtil.REGISTER_URL, form);
        yield return request.SendWebRequest();

        MessageResponse message = getMessageResponse(request);
        handleResponse(message);
    }

    private static MessageResponse getMessageResponse(UnityWebRequest request) {
        if (request.result == UnityWebRequest.Result.ConnectionError)
            return MessageResponse.connectionError();

        else if (request.responseCode == ServiceUtil.STATUS_201_CREATED)
            return MessageResponse.ok(message: SUCCESS_ACCOUNT_CREATED);

        else if (request.responseCode == ServiceUtil.STATUS_400_BAD_REQUEST) {
            RegisterResponse response = JsonUtility.FromJson<RegisterResponse>(request.downloadHandler.text);

            if (response.password.Count > 0)
                return MessageResponse.validationError(response.password[0]);

            else if (response.email.Count > 0)
                return MessageResponse.validationError(response.email[0]);

            else if (response.re_password.Count > 0)
                return MessageResponse.validationError(response.re_password[0]);
        }

        return MessageResponse.unknownError();
    }

    public static WWWForm createRegisterForm(string email, string password, string password2) {
        WWWForm form = new WWWForm();
        form.AddField(ServiceUtil.EMAIL_FIELD_NAME, email);
        form.AddField(ServiceUtil.PASSWORD_FIELD_NAME, password);
        form.AddField(ServiceUtil.PASSWORD2_FIELD_NAME, password2);
        return form;
    }

    [Serializable]
    public class RegisterResponse {
        public List<string> email;
        public List<string> password;
        public List<string> re_password;
    }
}
