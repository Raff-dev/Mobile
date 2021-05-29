using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour {
    public virtual void open() {
        gameObject.SetActive(true);
    }

    public virtual void hide() {
        gameObject.SetActive(false);
    }
}
