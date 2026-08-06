using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Enemy : MonoBehaviour {

    public AudioClip EnemyDie, EnemyHurt;
    public AudioClip chimkeu, SpawnMinion;
    public AudioClip shoot, strike;
    public static Sound_Enemy _instance;
    private AudioSource _audio;
    public static bool state = true; // ==true là mở âm thanh
    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    void Start ()
    {
        _audio = GetComponent<AudioSource>();
        if (state == true)
        {
            _audio.mute = false;
        }
        else
        {
            _audio.mute = true;
        }
        DontDestroyOnLoad(this.transform);
	}
    public void SpawnSound()
    {
        _audio.PlayOneShot(SpawnMinion);
    }
    public void Twitter()
    {
        _audio.PlayOneShot(chimkeu);
    }
    public void Hurt()
    {
        _audio.PlayOneShot(EnemyHurt);
    }
    public void Die()
    {
        _audio.PlayOneShot(EnemyDie);
    }
    public void SetSound(bool boo)
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
    public void Shoot()
    {
        _audio.PlayOneShot(shoot);
    }
    public void Strike()
    {
        _audio.PlayOneShot(strike);
    }
}
