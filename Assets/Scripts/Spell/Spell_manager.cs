using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell_manager : MonoBehaviour
{

    public GameObject RainPos;
    public GameObject Rain = null;
    public float colddown;
    private float timer;
    public static bool countTime = false;
    public Image _blackImage;
    public Sprite RainX;
    private Sprite imageobj;
    void Start()
    {
        countTime = false;
        imageobj = GetComponent<Image>().sprite;
        timer = colddown;
        if (PlayerPrefs.GetInt("Rainupgrade3") == 1)
        {
            colddown -= 10;
        }
        if (PlayerPrefs.GetInt("Rainupgrade4") == 1)
        {
            colddown -= 10;
        }
    }
    public void ShowRain()
    {
        if (countTime == true || Manager.isFinishing==true) return;
        if (Rain == null)
        {
            GetComponent<Image>().sprite = RainX;
            Rain = Instantiate(RainPos, transform.position, Quaternion.identity) as GameObject;
        }
        else
        {
            GetComponent<Image>().sprite = imageobj;
            Destroy(Rain);
        }

    }
    public void ChangeImage()
    {
        GetComponent<Image>().sprite = imageobj;
    }
    private void Update()
    {
        if (countTime == true)
        {
            timer -= Time.deltaTime;
            _blackImage.fillAmount = Mathf.Abs(timer / colddown);
            if (timer <= 0)
            {
                timer = colddown;
                countTime = false;
            }
        }
    }
}
