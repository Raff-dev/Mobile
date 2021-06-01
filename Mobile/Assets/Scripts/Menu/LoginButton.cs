using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginButton : FormControlButton {

    public override void navigate() {
        MessageResponse message = validateInput();
        message.show(this.messageText);

        if (message.isError) return;

        WWWForm form = Authorization.createLoginForm(emailField.text, passwordField.text);
        StartCoroutine(Authorization.login(form, response => {
            Debug.Log("iserrror" + response.isError);
            if (response.isError) {
                ((MessageResponse)response).show(this.messageText);
            } else {

                Authorization.saveAuthorizationTokens(
                    ((DataResponse<Authorization.AuthResponse>)response).data);
                base.navigate();
            }
        }));
    }
}
