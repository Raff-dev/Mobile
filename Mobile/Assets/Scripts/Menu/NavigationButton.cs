using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationButton : MonoBehaviour {
    [SerializeField]
    private MenuPanel toOpen;

    [SerializeField]
    private MenuPanel toClose;

    public virtual void navigate() {
        if (toClose) {
            toClose.hide();
        }
        if (toOpen) {
            toOpen.open();
        }
    }
}
