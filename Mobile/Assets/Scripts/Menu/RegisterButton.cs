using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;
using System.Net.Mail;

public class RegisterButton : FormControlButton {
    [SerializeField] TMPro.TMP_InputField password2Field;

    public const string VALIDATION_PASSWORDS_NOT_MATCHING = "Provided passwords do not match";

    public override void navigate() {
        MessageResponse message = validateInput();
        message.show(this.messageText);

        if (message.isError) return;

        WWWForm form = Registration.createRegisterForm(
            emailField.text, passwordField.text, password2Field.text);

        StartCoroutine(Registration.register(form, message =>
            message.show(this.messageText)));
    }

    protected override MessageResponse validateInput() {
        MessageResponse message = base.validateInput();

        if (message.isError) return message;

        if (!passwordField.text.Equals(password2Field.text)) {
            return MessageResponse.validationError(VALIDATION_PASSWORDS_NOT_MATCHING);
        }

        return MessageResponse.ok();
    }
}
