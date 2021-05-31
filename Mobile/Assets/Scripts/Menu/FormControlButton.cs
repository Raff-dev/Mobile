using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public abstract class FormControlButton : NavigationHandler {
    public const int VALIDATION_MIN_PASSWORD_LENGTH = 8;
    public const string VALIDATION_EMAIL_INVALID = "This email is invalid";
    private string VALIDATION_PASSWORD_TOO_SHORT = "The password must contain at least " + VALIDATION_MIN_PASSWORD_LENGTH + " characters";

    [SerializeField] protected TMPro.TMP_Text messageText;
    [SerializeField] protected TMPro.TMP_InputField emailField;
    [SerializeField] protected TMPro.TMP_InputField passwordField;

    protected virtual MessageResponse validateInput() {
        if (!emailIsValid(emailField.text))
            return MessageResponse.validationError(VALIDATION_EMAIL_INVALID);

        if (passwordField.text.Length < VALIDATION_MIN_PASSWORD_LENGTH)
            return MessageResponse.validationError(VALIDATION_PASSWORD_TOO_SHORT);

        return MessageResponse.ok();
    }

    protected virtual bool emailIsValid(string email) {
        try {
            MailAddress m = new MailAddress(email);
            return true;
        } catch (Exception e) {
            if (e is FormatException | e is ArgumentException)
                return false;
            throw e;
        }
    }
}
