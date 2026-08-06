using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public bool On; // la phim On hay Off
    public Sprite yellow, gray;
    
    void Start()
    {
        if (On == true) // Nếu là phím onl thì sẽ chạy hàm CheckOn
        {
           // InvokeRepeating("CheckOn", 0f, 0.2f);
            CheckOn();
        }
        else if (On == false) // Nếu là phím onl thì sẽ chạy hàm CheckOff
        {
          //  InvokeRepeating("CheckOff", 0f, 0.2f);
            CheckOff();
        }
    }
    private void CheckOn()
    {
        if (SoundTower.state == true) // nếu tower bật chức năng sound
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
        if (SoundTower.state == false) // nếu tower bật chức năng sound
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
        SoundTower._instance.SetSound(true);
        Sound_Enemy._instance.SetSound(true);
        SoundManager[] sounds = FindObjectsOfType<SoundManager>();
        foreach(SoundManager s in sounds)
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
        SoundTower._instance.SetSound(false);
        Sound_Enemy._instance.SetSound(false);
        SoundManager[] sounds = FindObjectsOfType<SoundManager>();
        foreach (SoundManager s in sounds)
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
