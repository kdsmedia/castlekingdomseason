using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {

    public int indexLV;
    public Sprite GreyFlag,YelloFlagRed,YelloFlagGrey;
    public GameObject Star1, Star2, Star3;
    private Sprite _basic;
    public GameObject Panel_map_info;

    //-----------
	void Start ()
    {
       // PlayerPrefs.SetInt("Lock", 10); // cho LV mở khóa là 20
       // PlayerPrefs.SetInt("Star" + indexLV, 0); // cho tất cả đều 3 sao

        if (PlayerPrefs.GetInt("Lock") >= indexLV || indexLV==1)// nếu đã qua level
        {
            if(PlayerPrefs.GetInt("Lock") > indexLV)
            GetComponent<SpriteRenderer>().sprite = GreyFlag;
            GetComponent<Collider2D>().enabled = true;
            if (PlayerPrefs.GetInt("Star" + indexLV) == 1)
            {
                Star1.SetActive(true);
            }
            else if(PlayerPrefs.GetInt("Star" + indexLV) == 2)
            {
                Star1.SetActive(true);
                Star2.SetActive(true);
            }
            else if(PlayerPrefs.GetInt("Star" + indexLV) == 3)
            {
                Star1.SetActive(true);
                Star2.SetActive(true);
                Star3.SetActive(true);
            }
        }
        else// nếu chưa qua level
        {
          //  GetComponent<Collider2D>().enabled = false;
            gameObject.SetActive(false);
        }
        _basic = GetComponent<SpriteRenderer>().sprite;
    }
    void OnMouseDown()
    {
        SoundTower._instance.Click();
        SpawnLevel.level = indexLV;
        StartCoroutine(UI_Button());
    }

    private IEnumerator UI_Button() // tạo hiệu ứng click khi click vào button level
    {
        if (GetComponent<SpriteRenderer>().sprite == GreyFlag)
        {
            GetComponent<SpriteRenderer>().sprite = YelloFlagGrey;
        }
        else
        GetComponent<SpriteRenderer>().sprite = YelloFlagRed;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().sprite = _basic;
        yield return new WaitForSeconds(0.1f);
        Panel_map_info.SetActive(true);
        Show_Info_Map._instace.Check(this);
        transform.parent.gameObject.SetActive(false);
    }
}
