using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

    private AudioSource _audio;
    public static Music _instance;
    public AudioClip music_menu,music_gameplay;
    public static bool state = true;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
	void Start ()
    {
        _audio = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
        PlayonMenu();
	}
	public void PlayonMenu()
    {
        _audio.clip = music_menu;
        _audio.Play();
    }
    public void PlayonGamePlay()
    {
        _audio.clip = music_gameplay;
        _audio.Play();
    }
    public void SetMusic(bool boo)
    {
        if (boo == true)
        {
            state = true;
            _audio.mute = false;
        }
        else
        {
            state = false;
            _audio.mute = true;
        }
    }
}
