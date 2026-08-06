using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour {

    public bool On; // la phim On hay Off
    public Sprite yellow, gray;
    void Start()
    {
        if (On == true)
        {
           // InvokeRepeating("CheckOn", 0f, 0.5f);
            CheckOn();
        }
        else if (On == false)
        {
          //  InvokeRepeating("CheckOff", 0f, 0.5f);
            CheckOff();
        }
    }
    private void CheckOn()
    {
        if (Music.state == true) // nếu tower bật chức năng sound
        {
            GetComponent<Image>().sprite = yellow;
        }
        else
        {
            GetComponent<Image>().sprite = gray;
        }
    }
    private void CheckOff()
    {
        if (Music.state == false) // nếu tower bật chức năng sound
        {
            GetComponent<Image>().sprite = yellow;
        }
        else
        {
            GetComponent<Image>().sprite = gray;
        }
    }

    public void SetSoundOn()
    {
        GetComponent<Image>().sprite = yellow;
        Music._instance.SetMusic(true);
        MusicManager [] sounds = FindObjectsOfType<MusicManager>();
        foreach (MusicManager s in sounds)
        {
            if (s == this)
            {
                s.CheckOn();
            }
            else
            {
                s.CheckOff();
            }
        }
    }
    public void SetSoundOff()
    {
        GetComponent<Image>().sprite = yellow;
        Music._instance.SetMusic(false);
        MusicManager[] sounds = FindObjectsOfType<MusicManager>();
        foreach (MusicManager s in sounds)
        {
            if (s == this)
            {
                s.CheckOff();
            }
            else
            {
                s.CheckOn();
            }
        }
    }
}
