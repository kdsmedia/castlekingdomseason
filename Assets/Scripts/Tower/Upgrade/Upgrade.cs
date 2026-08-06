using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public int index;
    private GameObject House; // Obj cha của nó là nhà nào
    private Vector3 pos1, pos2, pos3;
    private bool isBarrack, isArcher, isMage, isCannon;
    private Barrack _barrack;
    private Archer _archer;
    private Cannon _cannon;
    private Mage _mage;

    //----------------
    public bool allowclick = false;
    private bool wascall = false;
    public bool waspoint = false; // co dang duoc chi chuot toi hay ko
    Collider2D coll;

    public Canvas UI_Interface = null;
    private Vector3 offset = new Vector3(0, 0.5f, 0);
    public Text _text;
    private int number = 0;

    private Upgrade[] _upgrades;
    public int cost = 10;
    private SpriteRenderer _sprite;

    void setonclicktrue()
    {
        allowclick = true;
        wascall = false;
    }// Sau 1 khoang thoi gian 0.2s thi duoc phep click chuot

    private void ShowText() // Hiển thị Cavas chứa khung thông tin Upgrade level tiếp theo
    {
        if (UI_Interface.enabled == false) // Neu UI text dang DisActive thì bật nó lên 
        {
            UI_Interface.transform.position = transform.position + offset;
            UI_Interface.enabled = true;
            ShowNextZone();
        }
        else
        {
            UI_Interface.enabled = false;
            HideNextZone();
        }
    }
    private void DisActiveUI_text()
    {
        if (UI_Interface.enabled == true)
        {
            UI_Interface.enabled = false;
        }
        HideNextZone();
    }
    private void SwitchText() // Viết Text mô tả level tiếp theo khi upgrade
    {
        if (_barrack)
        {
            if (index == 1) { _text.text = "Barrack Level 2"; }
            else if (index == 2) { _text.text = "Barrack Level 3"; }
            else if (index == 3) { _text.text = "Barrack Level 4"; }
            else if (index == 4) { _text.text = "Barrack Level 5"; }
        }
        else if (_mage)
        {
            if (index == 1) { _text.text = "Mage Level 2"; }
            else if (index == 2) { _text.text = "Mage Level 3"; }
            else if (index == 3) { _text.text = "Mage Level 4"; }
            else if (index == 4) { _text.text = "Mage Level 5"; }
        }
        else if (_archer)
        {
            if (index == 1) { _text.text = "Archer Level 2"; }
            else if (index == 2) { _text.text = "Archer Level 3"; }
            else if (index == 3) { _text.text = "Archer Level 4"; }
            else if (index == 4) { _text.text = "Archer Level 5"; }
        }
        else if (_cannon)
        {
            if (index == 1) { _text.text = "Cannon Level 2"; }
            else if (index == 2) { _text.text = "Cannon Level 3"; }
            else if (index == 3) { _text.text = "Cannon Level 4"; }
            else if (index == 4) { _text.text = "Cannon Level 5"; }
        }
    }
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        allowclick = false;
        coll = GetComponent<Collider2D>();
        UI_Interface = GameObject.Find("Canvas_Tower").GetComponent<Canvas>();
        _text = UI_Interface.transform.GetChild(0).transform.GetChild(0).transform.GetComponent<Text>();
        //------------
        #region Xác định Obj cha là nhà nào trong 4 nhà
        House = transform.parent.transform.parent.gameObject;
        if (House.GetComponent<Barrack>() != null)
        {
            _barrack = House.GetComponent<Barrack>(); // barrack cu
            isBarrack = true;
        }
        if (House.GetComponent<Archer>() != null)
        {
            _archer = House.GetComponent<Archer>();
            isArcher = true;
        }
        if (House.GetComponent<Cannon>() != null)
        {
            _cannon = House.GetComponent<Cannon>();
            isCannon = true;
        }
        else
        {
            _mage = House.GetComponent<Mage>();
            isMage = true;
        }
        #endregion
        _upgrades = FindObjectsOfType<Upgrade>();
        InvokeRepeating("CheckEnoughMoney", 0f, 0.5f);
        upgrade_property();
    }
    private void upgrade_property()
    {
        if (_mage == true)
        {
            if (PlayerPrefs.GetInt("Mageupgrade3") == 1)
            {
                cost = Mathf.RoundToInt(cost * 0.9f);
            }
        }
        else if (_cannon == true)
        {
            if (PlayerPrefs.GetInt("Cannonupgrade3") == 1)
            {
                cost = Mathf.RoundToInt(cost * 0.9f);
            }
            if (PlayerPrefs.GetInt("Cannonupgrade4") == 1)
            {
                cost = Mathf.RoundToInt(cost * 0.75f);
            }
        }
    }
    void OnMouseDown()
    {
        SoundTower._instance.Click();
        if (cost > Manager.money) { return; }
        number++;
        SwitchText();
        ShowText();
        if (UI_Interface.enabled == false && number >= 2)
        {
            SoundTower._instance.Upgrade();
            Manager.money -= cost;
            Manager._instance.setcoin();
            HideCurrentZone();
            number = 0;
            DisActiveUI_text();
            #region Instance Tower tiep theo va pha huy tower cu
            if (isBarrack)
            {
                GameObject newBarrack = Instantiate(Resources.Load("Barrack/Barrack" + index), House.transform.position, Quaternion.identity) as GameObject;
                newBarrack.GetComponent<Barrack>().spawnfirsttime = false;

                _barrack.GetinforKnight();
                if (_barrack.pos1 != Vector3.zero)
                {
                    pos1 = _barrack.pos1;
                    newBarrack.GetComponent<Barrack>().pos1 = pos1;
                }
                if (_barrack.pos2 != Vector3.zero)
                {
                    pos2 = _barrack.pos2;
                    newBarrack.GetComponent<Barrack>().pos2 = pos2;
                }
                if (_barrack.pos3 != Vector3.zero)
                {
                    pos3 = _barrack.pos3;
                    newBarrack.GetComponent<Barrack>().pos3 = pos3;
                }
                newBarrack.transform.Find("Flag").transform.position = House.transform.Find("Flag").transform.position;
                Destroy(House);
            }

            else if (isArcher)
            {
                Instantiate(Resources.Load("Archer/Archer" + index), House.transform.position, Quaternion.identity);
                Destroy(House);
            }
            else if (isCannon)
            {
                Instantiate(Resources.Load("Cannon/Cannon" + index), House.transform.position, Quaternion.identity);
                Destroy(House);
            }
            else
            {
                Instantiate(Resources.Load("Mage/Mage" + index), House.transform.position, Quaternion.identity);
                Destroy(House);
            }
            #endregion
        }
    }
    private void ShowNextZone()
    {
        if (_archer)
        {
            _archer.ShowNextZone(true);
        }
        else if (_cannon)
        {
            _cannon.ShowNextZone(true);
        }
        else if (_mage)
        {
            _mage.ShowNextZone(true);
        }
    }

    void HideNextZone()
    {
        if (_archer)
        {
            _archer.ShowNextZone(false);
        }
        else if (_cannon)
        {
            _cannon.ShowNextZone(false);
        }
        else if (_mage)
        {
            _mage.ShowNextZone(false);
        }
    }
    void HideCurrentZone()
    {
        if (_archer)
        {
            _archer.ShowCurrentZone(false);
        }
        else if (_cannon)
        {
            _cannon.ShowCurrentZone(false);
        }
        else if (_mage)
        {
            _mage.ShowCurrentZone(false);
        }
    }
    //-----------------------------
    void Update()
    {
        // print("trang thai cua Canvas=" + UI_Interface.isActiveAndEnabled);
        if (allowclick == false && !wascall)
        {
            wascall = true;
            Invoke("setonclicktrue", 0.1f);
        }
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Vector3.zero);
        #region Neu dang chi chuot vao 1 vung Collider
        if (hit.collider != null) // Nếu chạm vào 1 vùng Collider nào đó
        {
            if (hit.collider == coll) // Nếu Collider va chạm bằng chính Obj này
            {
                waspoint = true;
            }
            else // COllider đang chỉ chuột đến là 1 Obj khác
            {
                waspoint = false;
                number = 0;
                if (Input.GetMouseButtonDown(0) && allowclick == true)
                {
                    Invoke("Check",0.05f);
                }
            }

        }
        #endregion
        #region Neu chi chuot vao vung ko co bat ky collider nao
        else
        {

            if (Input.GetMouseButtonDown(0) && allowclick==true)
            {
                waspoint = false;
                number = 0;
                DisActiveUI_text();
                HideNextZone();
                HideCurrentZone();
                Destroy(transform.parent.gameObject, 0.1f);
            }
        }
        #endregion
    }
    private void Check()
    {
        _upgrades = FindObjectsOfType<Upgrade>();
        if (_upgrades.Length == 1)
        {
            if (!_upgrades[0].waspoint)
            {
                DisActiveUI_text();
                HideNextZone();
                HideCurrentZone();
                Destroy(transform.parent.gameObject, 0.1f);
            }
        }
        else if (_upgrades.Length == 2)
        {
            if (!_upgrades[0].waspoint && !_upgrades[1].waspoint)
            {
                DisActiveUI_text();
                HideNextZone();
                HideCurrentZone();
                Destroy(transform.parent.gameObject, 0.1f);
            }

        }
        else if (_upgrades.Length == 3)
        {
            if (!_upgrades[0].waspoint && !_upgrades[1].waspoint && !_upgrades[2].waspoint)
            {
                DisActiveUI_text();
                HideNextZone();
                HideCurrentZone();
                Destroy(transform.parent.gameObject, 0.1f);
            }

        }
        else if (_upgrades.Length == 4)
        {
            if (!_upgrades[0].waspoint && !_upgrades[1].waspoint && !_upgrades[2].waspoint && !_upgrades[3].waspoint)
            {
                number = 0;
                DisActiveUI_text();
                HideNextZone();
                HideCurrentZone();
                Destroy(transform.parent.gameObject, 0.1f);
            }
        }
    }
    private void CheckEnoughMoney()
    {
        if (Manager.money >= cost)
        {
            _sprite.color = Color.white;
        }
        else
        {
            _sprite.color = Color.grey;
        }
    }

}
