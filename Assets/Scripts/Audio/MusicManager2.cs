using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager2 : MonoBehaviour
{
    public Sprite on, off;
    void Start()
    {
        if (Music.state == true)
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
        SoundTower._instance.Click();
        if (Music.state == true)
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
        if (Music.state == true)
        {
            Music._instance.SetMusic(false);
            GetComponent<Image>().sprite = off;
            return;
        }
        else if(Music.state == false)
        {
            Music._instance.SetMusic(true);
            GetComponent<Image>().sprite = on;
            return;
        }
    }
   
}
