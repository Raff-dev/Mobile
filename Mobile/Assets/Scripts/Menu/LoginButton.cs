using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginButton : FormControlButton {

    public override void navigate() {
        ResponseMessage message = validateInput();
        message.show(this.messageText);

        if (message.isError) return;

        WWWForm form = Authorization.createLoginForm(emailField.text, passwordField.text);
        StartCoroutine(Authorization.login(form, handleLoginResponse));
    }

    public void handleLoginResponse(ResponseMessage message) {
        if (message.isError) {
            message.show(this.messageText);
        } else {
            Authorization.saveAuthorizationTokens(message.data);
            base.navigate();
        }
    }
}
