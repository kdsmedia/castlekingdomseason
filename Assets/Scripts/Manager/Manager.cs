using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
  
    public static Manager _instance;
	public int initCoin;
    public GameObject _Interface;
    private GameObject preInterface;
    public GameObject build;
    public Sprite[] sprite;
    public GameObject[] Tower;
    public bool onclick { get; set; }

    //------------MONEY-------------
    public static int money;
    public Text _moneytext;
    public static int life;
    public Text lifeText;
    //--------GamOver----
    public GameObject panel_Over;
    public GameObject panel_pause;
    public GameObject panel_option;
    public GameObject panel_win;
    //----------Win Game---------
    public GameObject star1, star2, star3;
    public Animator _anim;
    public GameObject DeniedClick;
    public static bool isFinishing = false;
    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
	void Start ()
    {
		money = initCoin;
        life = 20;
        _moneytext.text = money.ToString();
        lifeText.text = life.ToString();

    }
    public void setcoin()
    {
        _moneytext.text = money.ToString();
    }
    public void TakeHeart()
    {
        if (life > 0)
        {
            life -= 1;
        }
        else return;
        lifeText.text = life.ToString();
        if (life <= 0)
        {
            isFinishing = true;
            GameOver();
        }
    }
    private void GameOver()
    {
        DeniedClick.SetActive(true);
        panel_Over.SetActive(true);
        _anim.SetInteger("Change", 3);
        Invoke("settimeslacezero", 1f);  
    }
    public void PauseGame()
    {
        DeniedClick.SetActive(true);
        Time.timeScale = 0f;
        panel_pause.SetActive(true);
		AdsControl.Instance.showAds ();
    }
    public void ConTinue()
    {
        panel_pause.SetActive(false);
        DeniedClick.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Resume()
    {
        StartCoroutine(HidePanelOption());
        Time.timeScale = 1f;
        DeniedClick.SetActive(false);
    }
    IEnumerator HidePanelOption()
    {
        _anim.SetInteger("Change", 2);
        yield return new WaitForSeconds(0.8f);
         panel_option.SetActive(false);
    }
    public void Option()
    {
        DeniedClick.SetActive(true);
        panel_option.SetActive(true);
        _anim.SetInteger("Change", 1);
        Invoke("settimeslacezero", 1f);
    }
    public void WinGame()// Hiện panel win game và đánh giá sao
    {
        DeniedClick.SetActive(true);
        PlayerPrefs.SetInt("Lock", SpawnLevel.level + 1);
        panel_win.SetActive(true);
        if (life < 5)
        {
            PlayerPrefs.SetInt("Star" + SpawnLevel.level, 1);
            star1.SetActive(true);
        }
        else if(life<10 && life >= 5)
        {
            PlayerPrefs.SetInt("Star" + SpawnLevel.level, 2);
            star1.SetActive(true);
            star2.SetActive(true);
        }
        else if (life >= 10)
        {
            PlayerPrefs.SetInt("Star" + SpawnLevel.level, 3);
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(true);
        }
        _anim.SetInteger("Change", 4);
      //  Invoke("settimeslacezero", 1f);
    }
	public void ShowInterface(Node node)
    {
        foreach(Transform child in node.transform)
        {
            if (child.transform.name == "Interface")
            {
                Canvas _canvas = GameObject.Find("Canvas_Tower").GetComponent<Canvas>();
                if (_canvas.isActiveAndEnabled)
                {
                    _canvas.enabled = false;
                }
                Destroy(child.gameObject);
                return;
            }
        }
        GameObject Interface = Instantiate(_Interface, node.transform.position, Quaternion.identity);
        Interface.name = "Interface";
        Interface.transform.parent = node.transform;
    }
    public void HideInterface()
    {
        GameObject preInterface = GameObject.Find("Interface");
        if (preInterface!=null)
        {
            Destroy(preInterface.gameObject);
        }
        else
            return;
    }
    public void Build(string name, Vector3 node, GameObject obj)
    {
        SoundTower._instance.Build();
       GameObject _build= Instantiate(build,node, Quaternion.identity) as GameObject;
        _build.transform.parent = obj.transform.parent.transform.parent;
        foreach(Sprite _sprite in sprite)
        {
            if(_sprite.name==name)
            {
                _build.GetComponent<SpriteRenderer>().sprite = _sprite;
            }
        }
    }
    // Hien tower len taij vi tri cua Node, dong thoi Disactive Node di
    public void ShowTower(string name, Vector3 node,GameObject obj)
    {
       // obj.SetActive(false);
        foreach (GameObject child in Tower)
        {
            if(child.gameObject.name==name)
            {
                Instantiate(child, node, Quaternion.identity);
            }
        }
       Destroy(obj.gameObject);


    }
    private void settimeslacezero()
    {
		AdsControl.Instance.showAds ();
        Time.timeScale = 0;
    }
    private void settimescaleone()
    {
        Time.timeScale = 1;
    }

}
