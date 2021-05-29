using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginMenu : NavigationHandler {

    private void Awake() {
        StartCoroutine(Authorization.verify(message => {
            Debug.Log(message.message);

            if (!message.isError) {
                base.navigate();
            }
        }));
    }
}