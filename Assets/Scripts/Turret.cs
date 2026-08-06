using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    private Vector3 nodeposition;
    private Collider2D coll;
    public Canvas UI_Interface = null;
    public Text _text;
    public GameObject Zone = null;
    private int number = 0;
    public Turret[] _turrets;
    public bool waspick = false;
    public bool allowclick = false;
    private Vector3 offset = new Vector3(0, 0.5f, 0);
    private bool wascall = false;

    private SpriteRenderer _sprite;
    public int cost = 10;
    // Use this for initialization
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        allowclick = false;
        _turrets = FindObjectsOfType<Turret>();
        nodeposition = transform.parent.transform.position;
        coll = GetComponent<Collider2D>();
        UI_Interface = GameObject.Find("Canvas_Tower").GetComponent<Canvas>();
        _text = UI_Interface.transform.GetChild(0).transform.GetChild(0).transform.GetComponent<Text>();
        InvokeRepeating("CheckEnoughMoney", 0f, 0.5f);
    }
    void setonclicktrue()
    {
        allowclick = true;
        wascall = false;
    }
    private void Update()
    {
        if (allowclick == false && !wascall)
        {
            wascall = true;
            Invoke("setonclicktrue", 0.1f);
        }
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Vector3.zero);
        if (hit.collider != null) // nếu bắt đc va chạm
        {
            if (hit.collider == coll) // Nếu chuột đang chỉ là chính obj thì waspick = true
            {
                waspick = true;
            }
            else // Nếu con chỉ chuột đang va chạm với 1 obj khác 
            {
                waspick = false;
                if (Input.GetMouseButtonDown(0) && allowclick == true)
                {
                    Invoke("Check", 0.1f);
                }
            }

        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                waspick = false;
                number = 0;
                HideUI();
                Destroy(transform.parent.gameObject, 0.1f);
            }
        }
    }
    private void Check()
    {
        _turrets = FindObjectsOfType<Turret>();
        if (!_turrets[0].waspick && !_turrets[1].waspick && !_turrets[2].waspick && !_turrets[3].waspick) // Nếu đang ko chỉ vào tất cả các nút nâng c
        {

            //  print("ca 4 cai turret deu ko dc click");
            HideUI();
            number = 0;
            Destroy(transform.parent.gameObject, 0.1f);

        }
        else
        {
            number = 0;
            if (Zone != null) // Tắt tầm bắn của trụ
            {
                Destroy(Zone);
            }
        }
    }
    private void HideUI()
    {
        //  print("GOi ham Hide UI");
        if (UI_Interface.isActiveAndEnabled == true)
        {
            UI_Interface.enabled = false;
        }
        if (Zone != null) // Tắt tầm bắn của trụ
        {
            //  print("Ham destroy trong hideUI");
            Destroy(Zone);
        }
    }
    private void OnMouseDown()
    {
        waspick = true;
        SoundTower._instance.Click();
        if (cost > Manager.money) { return; }
        number++;
        ShowZone();
        Show_UI_Text();
        if (number >= 2)
        {
            HideUI();
            Manager.money -= cost;
            Manager._instance.setcoin();
            number = 0;
            BuildTower();
        }

    }
    private void Show_UI_Text()
    {
        if (UI_Interface.isActiveAndEnabled == false) // Nếu UI đang Disactive thì bật nó lên và đặt vị trí cho nó
        {
            UI_Interface.enabled = true;
            UI_Interface.transform.position = transform.position + offset;
            // UI_Interface.transform.position = transform.parent.transform.parent.transform.position;
            #region Dong Text Mieu ta cac loai nha
            if (this.transform.name == "Barrack")
            {
                _text.text = "Nha Barrack";
            }
            else if (this.transform.name == "Mage")
            {
                _text.text = "Nha Mage";
            }
            if (this.transform.name == "Cannon")
            {
                _text.text = "Nha Cannon";
            }
            if (this.transform.name == "Archer")
            {
                _text.text = "Nha Archer";
            }
            #endregion
        }
        else // Nếu UI đang được bật thì tắt nó đi
        {
            if (UI_Interface.transform.position == transform.position + offset)
            {
                UI_Interface.enabled = false;
            }

            else
            {
                UI_Interface.enabled = true;
                UI_Interface.transform.position = transform.position + offset;
                // ShowZone();
            }
        }
    }
    private void ShowZone()
    {
        if (this.gameObject.name == "Barrack")
        {
            if (Zone == null)
            {
                Zone = Instantiate(Resources.Load("Barrack/Zone0"), transform.parent.transform.parent.transform.position, Quaternion.identity) as GameObject;
                Zone.transform.parent = transform;
            }
            else
                Destroy(Zone);
        }
        else if (this.gameObject.name == "Archer")
        {
            if (Zone == null)
            {
                Zone = Instantiate(Resources.Load("Archer/Zone0"), transform.parent.transform.parent.transform.position, Quaternion.identity) as GameObject;
                Zone.transform.parent = transform;
            }

            else
                Destroy(Zone);
        }
        else if (this.gameObject.name == "Cannon")
        {
            if (Zone == null)
            {
                Zone = Instantiate(Resources.Load("Cannon/Zone0"), transform.parent.transform.parent.transform.position, Quaternion.identity) as GameObject;
                Zone.transform.parent = transform;
            }

            else
                Destroy(Zone);
        }
        else if (this.gameObject.name == "Mage")
        {
            if (Zone == null)
            {
                Zone = Instantiate(Resources.Load("Mage/Zone0"), transform.parent.transform.parent.transform.position, Quaternion.identity) as GameObject;
                Zone.transform.parent = transform;
            }

            else
                Destroy(Zone);
        }
    } // Hiển thị tầm đánh của các Tower
    private void BuildTower()
    {
        transform.parent.transform.parent.GetComponent<Collider2D>().enabled = false;
        if (this.transform.name == "Barrack")
        {
            Manager._instance.Build("BarrackBuild", nodeposition, this.gameObject);
        }
        else if (this.transform.name == "Mage")
        {
            Manager._instance.Build("MageBuild", nodeposition, this.gameObject);
        }
        else if (this.transform.name == "Cannon")
        {
            Manager._instance.Build("CannonBuild", nodeposition, this.gameObject);
        }
        else if (this.transform.name == "Archer")
        {
            Manager._instance.Build("ArcherBuild", nodeposition, this.gameObject);
        }
        Destroy(transform.parent.gameObject);
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
