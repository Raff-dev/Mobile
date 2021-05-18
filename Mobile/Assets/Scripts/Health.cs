using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public int health;
    public int numOfHearts;

    public RawImage[] full_hearts;
    public RawImage[] empyt_hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Update(){
        for (int i=0; i< full_hearts.Length; i++)
        {
            if (i < numOfHearts){
                full_hearts[i].enabled = true;
                empyt_hearts[i].enabled = false;



            } else{
                full_hearts[i].enabled = false;
                empyt_hearts[i].enabled = true;
            }
        }
    }
}
