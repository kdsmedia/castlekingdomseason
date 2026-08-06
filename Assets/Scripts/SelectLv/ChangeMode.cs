using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMode : MonoBehaviour {

    public Text _text;
    private static int number;
	// Use this for initialization
	void Start ()
    {
        if (number == 0)
        {
            _text.text = "Easy";
        }
        else if (number == 1)
        {
            _text.text = "Normal";
        }
        else if (number == 2)
        {
            _text.text = "Casual";
        }
        else if (number == 3)
        {
            _text.text = "Vertical";
        }
    }
    public void Change()
    {
        if (number < 3)
        {
            number++;
        }
        else
        {
            number = 0;
        }
        if (number == 0)
        {
            _text.text = "Easy";
        }
        else if (number == 1)
        {
            _text.text = "Normal";
        }
        else if (number == 2)
        {
            _text.text = "Casual";
        }
        else if (number == 3)
        {
            _text.text = "Vertical";
        }
    }
}
