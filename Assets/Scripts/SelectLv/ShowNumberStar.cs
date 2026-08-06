using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowNumberStar : MonoBehaviour {

    public static int totalStar;
    public Text _text;
    public static ShowNumberStar _instance;
    public static int currentStar;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
	void Start ()
    {

		for(int i = 1; i <= 20; i++)
        {
            totalStar += PlayerPrefs.GetInt("Star" + i);
        }
        currentStar = totalStar - PlayerPrefs.GetInt("Cost");
        _text.text = currentStar.ToString();
	}
    public void ShowCurrentStar()
    {
        currentStar = totalStar - PlayerPrefs.GetInt("Cost");
        _text.text = currentStar.ToString();
    }

}
