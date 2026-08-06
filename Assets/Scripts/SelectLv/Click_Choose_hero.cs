using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Click_Choose_hero : MonoBehaviour {

    private Choose_Hero[] _choose;
    private Choose_Hero _main;
    public Text _text;
    public static Click_Choose_hero _instance;
    public GameObject[] correct_image;
	public GameObject purchasePanel;
    void Awake()
    {
		PlayerPrefs.DeleteAll ();
        if (_instance == null)
        {
            _instance = this;
        }
    }
	void Start ()
    {
        _choose = FindObjectsOfType<Choose_Hero>();       
        Check();
	}
    public void Check()
    {
        foreach(Choose_Hero choose in _choose)
        {
            if (choose.wasChoose == true)
            {
                _main = choose;
                break;
            }
        }
        if (PlayerPrefs.GetInt("Hero" + _main.index) == 0)
        {
			if (PlayerPrefs.GetInt ("Hero_Unlock_" + _main.index) == 1) {
				correct_image [_main.index - 1].SetActive (false);
				_text.text = "SELECT";
			}
			else
				_text.text = "Unlock by 200 gems";
        }
        else if(PlayerPrefs.GetInt("Hero" + _main.index) == 1)
        {
			if (PlayerPrefs.GetInt ("Hero_Unlock_" + _main.index) == 1) {
				correct_image [_main.index - 1].SetActive (true);
				foreach (GameObject _correct in correct_image) {
					if (_correct != correct_image [_main.index - 1]) {
						_correct.SetActive (false);
					}
				}
				_text.text = "DESELECT";
			}
			else
				_text.text = "Unlock by 200 gems";
           
        }
    }
    public void Change()
    {
		if (PlayerPrefs.GetInt ("Hero_Unlock_" + _main.index) == 1) {
			foreach (Choose_Hero choose in _choose) {
				if (choose.wasChoose == true) {
					_main = choose;
					break;
				}
			}
			if (PlayerPrefs.GetInt ("Hero" + _main.index) == 0) {
			
				PlayerPrefs.SetInt ("Hero" + _main.index, 1);
				_text.text = "DESELECT";
				correct_image [_main.index - 1].SetActive (true);
				foreach (GameObject _correct in correct_image) {
					if (_correct != correct_image [_main.index - 1]) {
						_correct.SetActive (false);
					}
				}
				foreach (Choose_Hero choose in _choose) {
					if (choose != _main) {
						PlayerPrefs.SetInt ("Hero" + choose.index, 0);
					}
				}

			
			} else if (PlayerPrefs.GetInt ("Hero" + _main.index) == 1) {
			
				PlayerPrefs.SetInt ("Hero" + _main.index, 0);
				_text.text = "SELECT";
				correct_image [_main.index - 1].SetActive (false);
			
			}
		} else {

			int currentGems = PlayerPrefs.GetInt ("Gems");
			if (currentGems >= 200) {

				FindObjectOfType<Menu_Manager> ().UpdateGems (-200);
				PlayerPrefs.SetInt ("Hero_Unlock_" + _main.index, 1);
				Check ();
			} else
				ShowPurchasePanel ();
		}
    }

	public void ShowPurchasePanel()
	{
		purchasePanel.SetActive (true);
	}

	public void HidePurchasePanel()
	{
		purchasePanel.SetActive (false);
	}
}
