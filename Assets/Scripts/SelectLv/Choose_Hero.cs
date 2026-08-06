using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Choose_Hero : MonoBehaviour {
    public Text _text;
    public Image [] _image;
    public GameObject Image_choose;
    public int index;
    public bool wasChoose=false;
    private Choose_Hero[] _chooses;
	// Use this for initialization
	void Start ()
    {
        _chooses = FindObjectsOfType<Choose_Hero>();
        if (index == 1)
        {
            Choose();
            wasChoose = true;
            Image_choose.transform.position = gameObject.transform.position;
        }
	}
    public void Choose()
    {
        wasChoose = true;
        Image_choose.transform.position = gameObject.transform.position;
        foreach (Choose_Hero choose in _chooses)
        {
            if (choose != this)
            {
                choose.wasChoose = false;
            }
        }
        _image[index-1].gameObject.SetActive(true);
        foreach(Image image in _image)
        {
            if (image != _image[index - 1])
            {
                image.gameObject.SetActive(false);
            }
        }
        LoadText();
        Click_Choose_hero._instance.Check();
    }
    private void LoadText()
    {
        if (index == 1)
        {
            _text.text = "Hero1";
        }
         else if (index == 2)
        {
            _text.text = "Hero2";
        }
    }
}
