using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour {
    public GameObject skin;
    public CustomizationManager customizationManager;
    public TMPro.TMP_Text priceText;

    public void onClick() {
        customizationManager.select(skin);
    }
}
