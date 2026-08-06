using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rung : MonoBehaviour {

    public Sprite on, off;
    void Start()
    {
        //PlayerPrefs.GetInt("Rung") == 1
        if (PlayerPrefs.GetInt("Rung") == 1)
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
        if (PlayerPrefs.GetInt("Rung") == 1)
        {
            PlayerPrefs.SetInt("Rung", 0);
            GetComponent<Image>().sprite = off;
            return;
        }
        else if (PlayerPrefs.GetInt("Rung") == 0)
        {
            PlayerPrefs.SetInt("Rung",1);
            GetComponent<Image>().sprite = on;
            return;
        }
    }
}
