using System.Net.Http;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Net.Mail;

public class RegisterButton : NavigationButton {

    [SerializeField] TMPro.TMP_Text messageText;
    [SerializeField] TMPro.TMP_InputField emailField;
    [SerializeField] TMPro.TMP_InputField password1Field;
    [SerializeField] TMPro.TMP_InputField password2Field;

    public const string SUCCESS_ACCOUNT_CREATED = "You have succesfully created an account.\nPlease verify your email.";

    public const int VALIDATION_MIN_PASSWORD_LENGTH = 8;
    public const string VALIDATION_EMAIL_INVALID = "This email is invalid";
    public const string VALIDATION_PASSWORDS_NOT_MATCHING = "Provided passwords do not match";
    public string VALIDATION_PASSWORD_TOO_SHORT = "The password must contain at least " + VALIDATION_MIN_PASSWORD_LENGTH + " characters";

    public const string ERROR_EMAIL_TAKEN = "This email is already in use";
    public const string ERROR_SOMETING_WRONG = "Something went wrong, check your connection and try later";
    public const string ERROR_UNKNOWN = "Unknown error has occured";

    public const string RESPONSE_EMAIL_TAKEN = "profile with this email address already exists.";

    public const string EMAIL_FIELD = "email";
    public const string PASSWORD1_FIELD_NAME = "password";
    public const string PASSWORD2_FIELD_NAME = "re_password";

    public Color ERROR_COLOR = UnityEngine.Color.red;
    public Color MESSAGE_COLOR = UnityEngine.Color.blue;

    public const string EMAIL_REGEX = @"/^\S+@\S+\.\S+$/";

    public override void navigate() {
        showMessage(null);
        string error = validateInput();

        if (error != null) {
            showError(error);
        } else {
            StartCoroutine(register());
        }
    }

    private string validateInput() {
        Debug.Log(emailField.text);

        if (!emailIsValid(emailField.text)) {
            return VALIDATION_EMAIL_INVALID;
        }

        if (password1Field.text.Length < VALIDATION_MIN_PASSWORD_LENGTH) {
            return VALIDATION_PASSWORD_TOO_SHORT;
        }

        if (!password1Field.text.Equals(password2Field.text)) {
            return VALIDATION_PASSWORDS_NOT_MATCHING;
        }

        return null;
    }

    private bool emailIsValid(string email) {
        try {
            MailAddress m = new MailAddress(email);
            return true;
        } catch (FormatException) {
            return false;
        }
    }

    public IEnumerator register() {
        WWWForm form = new WWWForm();
        form.AddField(EMAIL_FIELD, emailField.text);
        form.AddField(PASSWORD1_FIELD_NAME, password1Field.text);
        form.AddField(PASSWORD2_FIELD_NAME, password2Field.text);

        UnityWebRequest request = UnityWebRequest.Post(ServiceUtil.REGISTER_URL, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError) {
            showError(ERROR_SOMETING_WRONG);
        } else if (request.responseCode == ServiceUtil.STATUS_201_CREATED) {
            showMessage(SUCCESS_ACCOUNT_CREATED);
        } else if (request.responseCode == ServiceUtil.STATUS_400_BAD_REQUEST) {
            Response response = JsonUtility.FromJson<Response>(request.downloadHandler.text);

            if (response.password.Count > 0) {
                showError(response.password[0]);
            } else if (response.email.Count > 0) {
                showError(response.email[0]);
            } else if (response.re_password.Count > 0) {
                showMessage(response.re_password[0]);
            } else {
                showMessage(ERROR_UNKNOWN);
            }
        }
    }

    public void showMessage(string text) {
        messageText.color = MESSAGE_COLOR;
        messageText.text = text;
    }

    public void showError(string text) {
        messageText.color = ERROR_COLOR;
        messageText.text = text;
    }

    [Serializable]
    public class Response {
        public List<string> email;
        public List<string> password;
        public List<string> re_password;
    }
}

