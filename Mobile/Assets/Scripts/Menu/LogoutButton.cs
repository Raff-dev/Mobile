using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoutButton : NavigationHandler {

    public override void navigate() {
        Authorization.logout();
        base.navigate();
    }
}
