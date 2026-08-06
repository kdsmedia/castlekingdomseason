using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sell : MonoBehaviour
{
    public GameObject Gold_text;
    private int Gold;
    public int index;
    void Start()
    {
        Gold_text = GameObject.Find("UI").transform.GetChild(0).gameObject;
        if (index == 0)
        {
            if (PlayerPrefs.GetInt("Archerupgrade1") == 1)
            {
                Gold = Mathf.RoundToInt(70 * 0.9f);
            }
            Gold = Mathf.RoundToInt(70 * 0.6f);
        }
        else if (index == 1)
        {
            if (PlayerPrefs.GetInt("Archerupgrade1") == 1)
            {
                Gold = Mathf.RoundToInt(110 * 0.9f);
            }
            Gold = Mathf.RoundToInt(110 * 0.6f);
        }
        else if (index == 2)
        {
            if (PlayerPrefs.GetInt("Archerupgrade1") == 1)
            {
                Gold = Mathf.RoundToInt(160 * 0.9f);
            }
            Gold = Mathf.RoundToInt(160 * 0.6f);
        }
        else if (index == 3)
        {
            if (PlayerPrefs.GetInt("Archerupgrade1") == 1)
            {
                Gold = Mathf.RoundToInt(230 * 0.9f);
            }
            Gold = Mathf.RoundToInt(230 * 0.6f);
        }
        else if (index == 4)
        {
            if (PlayerPrefs.GetInt("Archerupgrade1") == 1)
            {
                Gold = Mathf.RoundToInt(250 * 0.9f);
            }
            Gold = Mathf.RoundToInt(250 * 0.6f);
        }
        else
        {
            if (PlayerPrefs.GetInt("Archerupgrade1") == 1)
            {
                Gold = Mathf.RoundToInt(250 * 0.9f);
            }
            Gold = Mathf.RoundToInt(250 * 0.6f);
        }
    }

    void OnMouseDown()
    {

        Instantiate(Resources.Load("Node"), transform.parent.transform.position, Quaternion.identity);
        Gold_text.SetActive(true);
        Gold_text.GetComponent<Text>().text = "+ " + Gold.ToString();
        Gold_text.transform.position = new Vector2(transform.parent.transform.parent.transform.position.x, transform.parent.transform.parent.transform.position.y + 1f);
        Manager.money += Gold;
        Manager._instance.setcoin();
        SoundTower._instance.Sell();
        Destroy(transform.parent.transform.parent.gameObject);
    }

}
