using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoutButton : NavigationButton {

    public override void navigate() {
        Authorization.logout();
        base.navigate();
    }
}
