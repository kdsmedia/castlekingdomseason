using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager2 : MonoBehaviour {

    public Sprite on, off;
    void Start()
    {
        if (SoundTower.state == true)
        {
            GetComponent<Image>().sprite = on;
        }
        else
        {
            GetComponent<Image>().sprite = off;
        }
    }
    private void Check()
    {
        if (SoundTower.state == true)
        {
            GetComponent<Image>().sprite = on;
        }
        else
        {
            GetComponent<Image>().sprite = off;
        }
    }
    public void Change()
    {
        SoundTower._instance.Click();
        if (SoundTower.state == true)
        {
            SoundTower._instance.SetSound(false);
            Sound_Enemy._instance.SetSound(false);
            GetComponent<Image>().sprite = off;
            return;
        }
        else if (SoundTower.state == false)
        {
            SoundTower._instance.SetSound(true);
            Sound_Enemy._instance.SetSound(true);
            GetComponent<Image>().sprite = on;
            return;
        }
    }
}
