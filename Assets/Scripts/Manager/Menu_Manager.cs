using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_Manager : MonoBehaviour
{
    public GameObject upgradepanel, HeroShopPanel,coinShopPanel;
    public GameObject LevelManager;
    public Animator ControlGate;
    AsyncOperation ao;
    private bool click;
	public Text gemsValueText;
	public bool hasShop;
    void Start()
    {
		//PlayerPrefs.DeleteAll ();
        click = false;
        if (SceneManager.GetActiveScene().buildIndex != 0)
            ControlGate.SetInteger("Change", 1); // Open Gate
		if(hasShop)
		ShowGemsText();
        Time.timeScale = 1f;
    }
    public void LoadGame()
    {
        StartCoroutine(LoadSceneGamePlay());
    }
    private IEnumerator LoadSceneGamePlay()
    {
        if (click == true) yield break;
        ao = SceneManager.LoadSceneAsync(2);
        ao.allowSceneActivation = false;
        click = true;
        ControlGate.SetInteger("Change", 2); // Close Gate
        yield return new WaitForSeconds(1f);
        click = false;
        ao.allowSceneActivation = true;
        Music._instance.PlayonGamePlay();
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        Music._instance.PlayonMenu();
    }
    public void SelectLV()
    {
        Time.timeScale = 1f;
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            Manager.isFinishing = true;
           // GetComponent<Animator>().SetInteger("Change", 2);
        }
        StartCoroutine(LoadSceneSelect());
    }
    private IEnumerator LoadSceneSelect()
    {
        if (click == true) yield break;
        ao = SceneManager.LoadSceneAsync(1);
        ao.allowSceneActivation = false;
        click = true;
        ControlGate.SetInteger("Change", 2);
        yield return new WaitForSeconds(1f);
        click = false;
        ao.allowSceneActivation = true;
        Music._instance.PlayonMenu();
    }
    public void ShowUpgrade()
    {
        upgradepanel.SetActive(true);
        LevelManager.SetActive(false);
    }
    public void HideUpgrade()
    {
        upgradepanel.SetActive(false);
        LevelManager.SetActive(true);
    }
    public void Reset()
    {
        PlayerPrefs.SetInt("Archer", 0);
        PlayerPrefs.SetInt("Barrack", 0);
        PlayerPrefs.SetInt("Mage", 0);
        PlayerPrefs.SetInt("Cannon", 0);
        PlayerPrefs.SetInt("Rain", 0);
        PlayerPrefs.SetInt("Solider", 0);
        Select_Upgrade[] upgrades = FindObjectsOfType<Select_Upgrade>();
        foreach (Select_Upgrade upgrade in upgrades)
        {
            upgrade.Check();
        }
        PlayerPrefs.SetInt("Cost", 0);
        ShowNumberStar._instance.ShowCurrentStar();
        for (int i = 1; i <= 5; i++)
        {
            PlayerPrefs.SetInt("Archerupgrade" + i, 0);
            PlayerPrefs.SetInt("Barrackupgrade" + i, 0);
            PlayerPrefs.SetInt("Cannonupgrade" + i, 0);
            PlayerPrefs.SetInt("Mageupgrade" + i, 0);
            PlayerPrefs.SetInt("Rainupgrade" + i, 0);
            PlayerPrefs.SetInt("Soliderupgrade" + i, 0);
        }
    }
    public void ShowShopHero()
    {
        HeroShopPanel.SetActive(true);
        LevelManager.SetActive(false);
    }
    public void HideShopHero()
    {
        HeroShopPanel.SetActive(false);
        LevelManager.SetActive(true);
    }
    public void Restart()
    {
        StartCoroutine(RestartGame());
    }

	public void OpenCoinShop()
	{
		coinShopPanel.SetActive (true);
	}

	public void CloseCoinShop()
	{
		coinShopPanel.SetActive (false);
	}

	public void ShowGemsText()
	{
		gemsValueText.text = PlayerPrefs.GetInt ("Gems").ToString ();
	}

	public void UpdateGems(int _value)
	{
		int currentGems = PlayerPrefs.GetInt ("Gems");
		PlayerPrefs.SetInt ("Gems", currentGems + _value);
		ShowGemsText ();
	}

    IEnumerator RestartGame()
    {
        Time.timeScale = 1f;
        ControlGate.SetInteger("Change", 2); // Close Gate
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(2);
    }
}
