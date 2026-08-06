using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Set_Viking : MonoBehaviour {

    public GameObject VikingPos;
    private GameObject Viking = null;
    public float colddown;
    private float timer;
    public static bool countTime = false;
    public Image _blackImage;
    public Sprite soliderX;
    private Sprite imageobj;
    void Start()
    {
        countTime = false;
        imageobj = GetComponent<Image>().sprite;
        timer = colddown;
    }
    public void ShowViking()
    {
        if (countTime == true || Manager.isFinishing==true) return;
        if (Viking == null)
        {
            GetComponent<Image>().sprite = soliderX;
            Viking = Instantiate(VikingPos, transform.position, Quaternion.identity) as GameObject;
        }
        else
        {
            GetComponent<Image>().sprite = imageobj;
            Destroy(Viking);
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
