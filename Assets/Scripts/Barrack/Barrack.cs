using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barrack : MonoBehaviour
{
    public GameObject Knight; // Prefab Knight
    public Transform Flag; // obj Flag
    private GameObject knight1, knight2, knight3;// là các obj Knight của nhà barrack
    private KnightController _k1, _k2, _k3;// code của 3 Knight
    private Vector3 offset1, offset2, offset3; // độ dịch vecto của các knight với cờ flag    
    public float TimeToMove; // tốc độ di chuyển đến Flag
    public Transform gate; // Nơi Knight được sinh ra

    //---------UpGrade---------
    private Animator barrack_Anim;// Animation của Barrack
    public int index = 0;// level của nhà Barrack

    [HideInInspector]
    public Vector3 pos1, pos2, pos3; // vị trí hiện tại của các Knight
    public bool spawnfirsttime = true;// kiếm tra xem đây có phải lần đầu tạo Knight hay không
    //-------------Interface----------------
    public Sprite[] _sprites_solider; // sprite avata đại diện cho các Knight
    private GameObject BarrackInter = null; // giao diện Uprade của nhà Barrack
    private Canvas _canvas;
    private Image _Image_solider;
    private Text Heal, Dame, armor, respawn;
    void Start()
    {
        #region cho Interface
        _canvas = GameObject.Find("Canvas_Info").GetComponent<Canvas>();
		_Image_solider = _canvas.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();        // Anh cua solider
        Heal = _canvas.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();
        Dame= _canvas.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();
        armor = _canvas.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();
        respawn = _canvas.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();
		_Image_solider.sprite = _sprites_solider[index]; // Avata của lính
        #endregion
        #region DO dich vector cho vi tri cua cac Knight
        offset1 = new Vector3(-0.15f, 0.1f, 0);
        offset2 = new Vector3(0, -0.1f, 0);
        offset3 = new Vector3(0.15f, 0.1f, 0);
        #endregion
        Flag = this.transform.gameObject.transform.Find("Flag");
        InvokeRepeating("UpdateLate", 0, 0.5f);
        barrack_Anim = GetComponent<Animator>();
		if (spawnfirsttime) // Lần đầu tiên tạo lính thì gọi hàm SpawnFirstTime
		{ StartCoroutine(SpawnFirstTime(1f, 1f, 1f)); }
		else // nếu không phải lần đầu tạo lính thì thay thế các Knight cũ bằng các Knight mới
            SpawnKnight();
		
		int layer = Mathf.Clamp(Mathf.Abs(50 - Mathf.RoundToInt((transform.position.y - 1) * 30)), 1, 150); // đặt layer theo trục Y
        GetComponent<SpriteRenderer>().sortingOrder = layer;
    }

    void UpdateLate()
    {
        SetPosFlag(); // đặt tọa độ của Flag cho Knight
        if (_k1 != null) // Update các chỉ số của Knight vào giao diện UI Info
        {
            //-----Thay doi cac thong so trong UIInfo
            Heal.text = _k1.maxHeal.ToString();
            if (_k1.armor < 5) { armor.text = "None"; }
            else if (_k1.armor >= 5 && _k1.armor < 10) { armor.text = "Low"; }
            else { armor.text = "Medium"; }
            Dame.text = _k1.dame.ToString();
            respawn.text = _k1.timerespawn.ToString();
            //---------------------------------------------
        }
        if(knight1==null||knight2==null||knight3 == null)
        {
            StartRespawn(10);
        }
    }
    void Update()
    {
        if (Manager.isFinishing == true) return;
        #region DI chuyển Knight về phía Flag
        if (_k1 != null)
        {
            if (knight1.activeSelf && (_k1.target == null || _k1.allowMove==false)) // Nếu Knight được sinh ra và được phép đk thì di chuyển nó về phía Flag
            {
				if (knight1.transform.position != (Flag.transform.position + offset1)) // Nếu ko ở vị trí của Flag thì sẽ di chuyển đến đó
                {
					knight1.transform.position = Vector2.MoveTowards(knight1.transform.position, Flag.transform.position + offset1, Time.deltaTime * TimeToMove);
                    if (_k1 != null)
                    {
						_k1._anim.SetInteger("Change", 1); // chạy Animation Run
						_k1.CheckFlipWith(Flag.gameObject); // check hướng quay mặt với mục tiêu là cờ Flag
                    }
                }
            }
        }
        if (_k2 != null)
        {
            if (knight2.activeSelf && (_k2.target == null || _k2.allowMove == false))
            {
                if (knight2.transform.position != (Flag.transform.position + offset2))
                {
                    knight2.transform.position = Vector3.MoveTowards(knight2.transform.position, Flag.transform.position + offset2, Time.deltaTime * TimeToMove);
                    if (_k2 != null)
                    {
                        _k2._anim.SetInteger("Change", 1);
                        _k2.CheckFlipWith(Flag.gameObject);
                    }
                }
            }
        }
        if (_k3 != null)
        {
            if (knight3.activeSelf && (_k3.target == null || _k1.allowMove == false))
            {
                if (knight3.transform.position != (Flag.transform.position + offset3))
                {
                    knight3.transform.position = Vector3.MoveTowards(knight3.transform.position, Flag.transform.position + offset3, Time.deltaTime * TimeToMove);
                    if (_k3 != null)
                    {
                        _k3._anim.SetInteger("Change", 1);
                        _k3.CheckFlipWith(Flag.gameObject);
                    }
                }
            }
        }
        #endregion
		// thực hiện việc đóng/ mở UI_Info của nhà Barrack
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Vector3.zero);
        if (hit.collider == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _canvas.enabled = false;
                _canvas.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else
        {
            if (hit.collider.gameObject.tag == "LimitSpell")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _canvas.enabled = false;
                    _canvas.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }

    }
    private void SetPosFlag()// Đặt giá trị tọa độ Flag cho Knight
    {
        if (_k1 != null)
            _k1.positionFlag = Flag.transform.position + offset1;
        if (_k2 != null)
            _k2.positionFlag = Flag.transform.position + offset2;
        if (_k3 != null)
            _k3.positionFlag = Flag.transform.position + offset3;
    }
	//-- Tao ra 3 Knight lan dau tien khi xây nhà--------------------
    IEnumerator SpawnFirstTime(float time1, float time2, float time3)
    {
        yield return new WaitForSeconds(time1);
        knight1 = Instantiate(Knight, gate.position, Quaternion.identity) as GameObject;
        barrack_Anim.SetTrigger("Open");
        SoundTower._instance.OpenGate();
        knight1.name = "Knight1";
        knight1.transform.parent = this.transform;        
        _k1 = knight1.GetComponent<KnightController>();

        yield return new WaitForSeconds(time2);
        knight2 = Instantiate(Knight, gate.position, Quaternion.identity) as GameObject;
        barrack_Anim.SetTrigger("Open");
        SoundTower._instance.OpenGate();
        knight2.name = "Knight2";
        knight2.transform.parent = this.transform;
        _k2 = knight2.GetComponent<KnightController>();

        yield return new WaitForSeconds(time3);
        knight3 = Instantiate(Knight, gate.position, Quaternion.identity) as GameObject;
        barrack_Anim.SetTrigger("Open");
        SoundTower._instance.OpenGate();
        knight3.name = "Knight3";
        knight3.transform.parent = this.transform;
        //   AllowControl3 = true;
        _k3 = knight3.GetComponent<KnightController>();
    }
    public void GetinforKnight()// Lấy tọa độ của các Knigh hiện tại
    {
        if (knight1 != null && transform.Find("Knight1"))
        {
            pos1 = knight1.transform.position;
        }
        if (knight2 != null && transform.Find("Knight2"))
        {
            pos2 = knight2.transform.position;
        }
        if (knight3 != null && transform.Find("Knight3"))
        {
            pos3 = knight3.transform.position;
        }
    }
    public void SpawnKnight()// Thay thế các Knight cũ bằng Knight mới.
    {
        if (pos1 != Vector3.zero)
        {
            knight1 = Instantiate(Knight, pos1, Quaternion.identity) as GameObject;
            knight1.name = "Knight1";
            knight1.transform.parent = this.transform;
            _k1 = knight1.GetComponent<KnightController>();
        }
        if (pos2 != Vector3.zero)
        {
            knight2 = Instantiate(Knight, pos2, Quaternion.identity) as GameObject;
            knight2.name = "Knight2";
            knight2.transform.parent = this.transform;
            _k2 = knight2.GetComponent<KnightController>();
        }
        if (pos3 != Vector3.zero)
        {
            knight3 = Instantiate(Knight, pos3, Quaternion.identity) as GameObject;
            knight3.name = "Knight3";
            knight3.transform.parent = this.transform;
            _k3 = knight3.GetComponent<KnightController>();
        }
        else
            RespawnKnight();
    }
    public void StartRespawn(int time)// Hồi sinh Knight sau 1 khoảng thời gian
    {
        Invoke("RespawnKnight", time);
    }
    void RespawnKnight()// thực hiện việc hồi sinh Knight
    {
        if (knight1 == null && !transform.Find("Knight1"))
        {
            knight1 = Instantiate(Knight, gate.position, Quaternion.identity) as GameObject;
            barrack_Anim.SetTrigger("Open");
            SoundTower._instance.OpenGate();
            knight1.name = "Knight1";
            knight1.transform.parent = this.transform;
            _k1 = knight1.GetComponent<KnightController>();
        }
        else if (knight2 == null && !transform.Find("Knight2"))
        {
            knight2 = Instantiate(Knight, gate.position, Quaternion.identity) as GameObject;
            barrack_Anim.SetTrigger("Open");
            SoundTower._instance.OpenGate();
            knight2.name = "Knight2";
            knight2.transform.parent = this.transform;
            _k2 = knight2.GetComponent<KnightController>();
        }
      else  if (knight3 == null && !transform.Find("Knight3"))
        {
            knight3 = Instantiate(Knight, gate.position, Quaternion.identity) as GameObject;
            barrack_Anim.SetTrigger("Open");
            SoundTower._instance.OpenGate();
            knight3.name = "Knight3";
            knight3.transform.parent = this.transform;
            _k3 = knight3.GetComponent<KnightController>();
        }

        Heal.text = _k1.maxHeal.ToString();
        if (_k1.armor < 5) { armor.text = "None"; }
        else if (_k1.armor >= 5 && _k1.armor < 10) { armor.text = "Low"; }
        else { armor.text = "Medium"; }
        Dame.text = _k1.dame.ToString();
        respawn.text = _k1.timerespawn.ToString();
        //---------------------------------------------
       // yield return new WaitForSeconds(0.5f);
    }
	void OnMouseDown() // thực hiện bật tắt giao diện Upgrade và UI_info của nhà Barrack
    {
        SoundTower._instance.Click();
        if (index < 3)
        {
            if (BarrackInter == null)
            {
                GameObject BarrackInterface = Resources.Load("Interface/BarrackInterface" + index) as GameObject;
                BarrackInter = Instantiate(BarrackInterface, transform.position, Quaternion.identity);
                BarrackInter.transform.parent = transform;
                ShowUI_info();

            }
            else
            {
                _canvas.enabled = false;
                _canvas.transform.GetChild(1).gameObject.SetActive(false);
                Destroy(BarrackInter, 0.1f);
            }
        }
        else
        {
            if (BarrackInter == null)
            {
                BarrackInter = Instantiate(Resources.Load("Interface/BarrackInterface" + index), transform.position, Quaternion.identity) as GameObject;
                BarrackInter.transform.parent = this.transform;
                ShowUI_info();
            }
            else
            {
                if (BarrackInter.activeSelf == false)
                {
                    BarrackInter.SetActive(true);
                    ShowUI_info();
                }
                else
                {
                    BarrackInter.SetActive(false);
                    GameObject.Find("Canvas_Tower").GetComponent<Canvas>().enabled = false;
                    _canvas.enabled = false;
                    _canvas.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }

    }
	private void ShowUI_info() // Hiển thị giao diện UI_info
    {
        _canvas.enabled = true;
        _canvas.transform.GetChild(0).gameObject.SetActive(false);
        _canvas.transform.GetChild(2).gameObject.SetActive(false);
        _canvas.transform.GetChild(3).gameObject.SetActive(false);
        _canvas.transform.GetChild(1).gameObject.SetActive(true);
    }
}
