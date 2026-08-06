using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Show_Info_Map : MonoBehaviour {

    public static Show_Info_Map _instace;
    public Image[] _BlackStars;
    public Sprite _spritesYelloStar;
    public GameObject[] maps;
    public Text _text;
    AsyncOperation ao;
    public GameObject Control_gate;
    public GameObject LevelManager;
    public Sprite BlackStar;
    private bool click;
    void Awake()
    {
        if (_instace == null)
        {
            _instace = this;
        }
    }
    void Start()
    {
        click = false;
    }
    public void Check(Level level)
    {
        if (PlayerPrefs.GetInt("Star" + level.indexLV) == 1)
        {
            _BlackStars[0].sprite = _spritesYelloStar;
        }
        else if (PlayerPrefs.GetInt("Star" + level.indexLV) == 2)
        {
            _BlackStars[0].sprite = _spritesYelloStar;
            _BlackStars[1].sprite = _spritesYelloStar;
        }
        else if (PlayerPrefs.GetInt("Star" + level.indexLV) == 3)
        {
            _BlackStars[0].sprite = _spritesYelloStar;
            _BlackStars[1].sprite = _spritesYelloStar;
            _BlackStars[2].sprite = _spritesYelloStar;
        }
        else
        {
            _BlackStars[0].sprite = BlackStar;
            _BlackStars[1].sprite = BlackStar;
            _BlackStars[2].sprite = BlackStar;
        }
        maps[level.indexLV - 1].SetActive(true);
        foreach(GameObject map in maps)
        {
            if(map!= maps[level.indexLV - 1])
            {
                map.SetActive(false);
            }
        }
        if (level.indexLV == 1)
        {
            _text.text = "Level 1";
        }
        else if (level.indexLV == 2)
        {
            _text.text = "Level 2";
        }
        else if (level.indexLV == 3)
        {
            _text.text = "Level 3";
        }
        else if (level.indexLV == 4)
        {
            _text.text = "Level 4";
        }
        else if (level.indexLV == 5)
        {
            _text.text = "Level 5";
        }
        else if (level.indexLV == 6)
        {
            _text.text = "Level 6";
        }
        else if (level.indexLV == 7)
        {
            _text.text = "Level 7";
        }
        else if (level.indexLV == 8)
        {
            _text.text = "Level 8";
        }
        else if (level.indexLV == 9)
        {
            _text.text = "Level 9";
        }
        else if (level.indexLV == 10)
        {
            _text.text = "Level 10";
        }
    }

    public void Exit()
    {
        LevelManager.SetActive(true);
        gameObject.SetActive(false);
    }
    public void Battle()
    {
        StartCoroutine(LoadScene());
    }
    private IEnumerator LoadScene()
    {
        if (click == true) yield break;
        ao = SceneManager.LoadSceneAsync(2);
        ao.allowSceneActivation = false;
        click = true;
        Control_gate.GetComponent<Animator>().SetInteger("Change",2);
        yield return new WaitForSeconds(1f);
        click = false;
        ao.allowSceneActivation = true;
    }
}
