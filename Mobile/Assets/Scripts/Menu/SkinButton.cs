using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinButton : MonoBehaviour {
    public GameObject skin;
    public CustomizationManager customizationManager;

    public void onClick() {
        customizationManager.select(skin);
    }
}
