using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTower : MonoBehaviour {

    [Header("Mage Tower")]
    public AudioClip[] Mage;
    public AudioClip ball,Laze;
    [Header("Cannon Tower")]
    public AudioClip[] Cannon;
    public AudioClip shoot, bum;
    public AudioClip lighting;
    [Header("Archer Tower")]
    public AudioClip[] Archer;
    public AudioClip [] Arrow;
    [Header("Barrack Tower")]
    public AudioClip[] Barrack;
    public AudioClip  [] attack ;
    public AudioClip opengate;
    [Header("Solider")]
    public AudioClip[] soliders;

    public AudioClip sell, upgrade,click;

    public AudioClip build;
    private int a=0, b=0, c=0, m=0,s=0;

    public static SoundTower _instance;
    private  AudioSource _audio;
    public static bool state=true; // ==true là mở âm thanh
    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(this.gameObject);
        _audio = GetComponent<AudioSource>();
        if (state == true)
        {
            _audio.mute = false;
        }
        else
        {
            _audio.mute = true;
        }
	}

    public void MageTower()
    {
        _audio.PlayOneShot(Mage[m]);
        m++;
        if(m>= Mage.Length)
        {
            m = 0;
        }
    }
    public void ArcherTower()
    {
        _audio.PlayOneShot(Archer[a]);
        a++;
        if (a >= Archer.Length)
        {
            a = 0;
        }
    }
    public void BarrackTower()
    {
        _audio.PlayOneShot(Barrack[b]);
        b++;
        if (b >= Barrack.Length)
        {
            b = 0;
        }
    }
    public void CannonTower()
    {
        _audio.PlayOneShot(Cannon[c]);
        c++;
        if (c >= Cannon.Length)
        {
            c = 0;
        }
    }
    public void SoliderSpawn()
    {
        _audio.PlayOneShot(soliders[s]);
        s++;
        if (s >= soliders.Length)
        {
            s = 0;
        }
    }
    public void KnightFight()
    {
        _audio.PlayOneShot(attack[Random.Range(0,attack.Length)]);
    }
    public void ShootArrow()
    {
        _audio.PlayOneShot(Arrow[Random.Range(0, Arrow.Length)]);
    }
    public void Cannon_Fire()
    {
        _audio.PlayOneShot(shoot);
    }
    public void CannonExplosion()
    {
        _audio.PlayOneShot(bum);
    }
    public void Light()
    {
        _audio.PlayOneShot(lighting);
    }
    public void OpenGate()
    {
        _audio.PlayOneShot(opengate);
    }
    public void Sell()
    {
        _audio.PlayOneShot(sell);
    }
    public void Upgrade()
    {
        _audio.PlayOneShot(upgrade);
    }
    public void MageBall()
    {
        _audio.PlayOneShot(ball);
    }
    public void MageLaze()
    {
        _audio.PlayOneShot(Laze);
    }
    public void Build()
    {
        _audio.PlayOneShot(build);
    }
    public void Click()
    {
        _audio.PlayOneShot(click);
    }
    public void SetSound(bool boo)
    {
        if (boo == true)
        {
            _audio.mute = false;
            state = true;
        }
        else
        {
            _audio.mute = true;
            state = false;
        }
    }
}
