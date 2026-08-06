using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Archer : MonoBehaviour
{

    [HideInInspector]
    public GameObject CurrentZone, NextZone;// game obj tầm đánh hiện tại và tiếp theo
    public int Archer_Index, next_index;// chỉ số level hiện tại và tiếp theo của nhà Archer
    [HideInInspector]
    public List<GameObject> enemies = new List<GameObject>();// Danh Sách Enemy trong tầm đánh
    public GameObject target; // mục tiêu được chọn tấn công
    public float coldown;// thời gian coldowwn khi bắn tên
    public int Dame;// dame của nhà tên
    private float searchvalue = 0.5f;
    [Header("Vi tri sinh ra mui ten phia duoi")]
    public Transform pos1_down, pos2_down; // vị trí sinh ra mũi tên của 2 lính
    [Header("Vi tri sinh ra mui ten phia tren")]
    public Transform pos1_up, pos2_up; // vị trí sinh ra mũi tên của 2 lính
    private Animator Solider_Anim1, Solider_Anim2; // Animation của 2 lính 
    private GameObject Interface = null; // obj giao diện upgrade
    public bool isShooting = false;// nhà đang trong trạng thái bắn hay không
    //-----------GUN---------------------
    public bool isGun; // Xác định xem Archer này nó phải là Archer súng ko
    //-------------UI Interface----------------
    private Canvas _canvas;
    private Text Speedtext, dametext, rangetext;

    private SpriteRenderer _sprite;
    void Start()
    {
        Upgrade_property();// Kiểm tra thông tin Update ở panel select
        #region UI Infomation
        _canvas = GameObject.Find("Canvas_Info").GetComponent<Canvas>();
        dametext = _canvas.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
        Speedtext = _canvas.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();
        rangetext = _canvas.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();
        #endregion
        next_index = Archer_Index + 1;
        #region Load Zone
        CurrentZone = Instantiate(Resources.Load("Archer/Zone" + Archer_Index)) as GameObject;
        CurrentZone.transform.parent = transform;
        CurrentZone.transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
        CurrentZone.GetComponent<SpriteRenderer>().color = Color.blue;
        CurrentZone.SetActive(false);
        if (Resources.Load("Archer/Zone" + next_index) != null)
        {
            NextZone = Instantiate(Resources.Load("Archer/Zone" + next_index)) as GameObject;
            NextZone.transform.parent = transform;
            NextZone.transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
            NextZone.SetActive(false);
        }
        #endregion
        Solider_Anim1 = transform.Find("Solider").transform.GetComponent<Animator>();
        Solider_Anim2 = transform.Find("Solider2").transform.GetComponent<Animator>();
        //Show UI Infomation len neu UI_infomation duoc bat san
        if (_canvas.enabled == true)
            EnableUI_Infomation();
        SoundTower._instance.ArcherTower();
        _sprite = GetComponent<SpriteRenderer>();
        int layer = Mathf.Clamp(Mathf.Abs(50 - Mathf.RoundToInt((transform.position.y - 1) * 15)), 1, 150);
        _sprite.sortingOrder = layer;
    }
    private void Upgrade_property()
    {
        if (PlayerPrefs.GetInt("Archerupgrade2") == 1)
        {
            GameObject zone = transform.Find("Zone").gameObject;
            zone.transform.localScale = new Vector2(zone.transform.localScale.x + 0.1f, zone.transform.localScale.y + 0.1f);
        }
        if (PlayerPrefs.GetInt("Archerupgrade3") == 1)
        {
            Dame += 2;
        }
        if (PlayerPrefs.GetInt("Archerupgrade4") == 1)
        {
            GameObject zone = transform.Find("Zone").gameObject;
            zone.transform.localScale = new Vector2(zone.transform.localScale.x + 0.1f, zone.transform.localScale.y + 0.1f);
        }
        if (PlayerPrefs.GetInt("Archerupgrade5") == 1)
        {
            Dame += 2;
        }
    }// Kiểm tra thông tin Update ở panel select
    void OnMouseDown()
    {
        SoundTower._instance.Click();
        if (Archer_Index < 3)
        {
			if (Interface == null) // nếu chưa có obj Upgrade thì sinh ra nó và bật UI
            {
                Interface = Instantiate(Resources.Load("Interface/ArcherInterface" + Archer_Index), transform.position, Quaternion.identity) as GameObject;
                Interface.transform.parent = this.transform;
                CurrentZone.SetActive(true);
                EnableUI_Infomation();
            }
			else // ngược lại hủy obj upgrade và tắt UI
            {
                CurrentZone.SetActive(false);
                Destroy(Interface, 0.2f);
                _canvas.enabled = false;
                _canvas.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            if (Interface == null)
            {
                Interface = Instantiate(Resources.Load("Interface/ArcherInterface" + Archer_Index), transform.position, Quaternion.identity) as GameObject;
                Interface.transform.parent = this.transform;
                // Bat UI Info len
                EnableUI_Infomation();
            }
            else
            {
                CurrentZone.SetActive(false);
                if (Interface.activeSelf == false)
                {
                    Interface.SetActive(true);
                    EnableUI_Infomation();
                }
                else
                {
                    Interface.SetActive(false);
                    GameObject.Find("Canvas_Tower").GetComponent<Canvas>().enabled = false;
                    _canvas.enabled = false;
                    _canvas.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    } // bật tắt UI_info và obj Upgrade
    void Update()
    {
        if (Manager.isFinishing == true) return;
        RemoveNull();
        if (enemies.Count > 0) // neu tim thay Enemy
        {
            // RemoveNull();
            target = enemies[0];
            if (isShooting == false && !isGun) // Nếu không phải nhà súng và đang không bắn thì được phép bắn tên
            {
                isShooting = true;
                Invoke("SetShootingFalse", coldown);
                StartCoroutine(Shoot_Arrow()); // Gọi hàm bắn tên
            }
            else if (isShooting == false && isGun) // Nếu là nhà súng và đang không bắn thì được phép bắn đạn
            {
                isShooting = true;
                Invoke("SetShootingFalse", coldown);
                StartCoroutine(Shoot_bullet()); // gọi hàm bắn súng
            }
        }
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Vector3.zero);
        if (hit.collider == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _canvas.enabled = false;
                _canvas.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            if (hit.collider.gameObject.tag == "LimitSpell")
            {
                if (Input.GetMouseButton(0))
                {
                    _canvas.enabled = false;
                    _canvas.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }

    }
    IEnumerator Shoot_Arrow()// Hàm thực hiện việc bắn tên
    {

        if (target == null) yield break;
        if (transform.position.x < target.transform.position.x)
        {
            transform.Find("Solider").transform.localScale = new Vector3(1, 1, 1);
            transform.Find("Solider2").transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.Find("Solider").transform.localScale = new Vector3(-1, 1, 1);
            transform.Find("Solider2").transform.localScale = new Vector3(-1, 1, 1);
        }
        if (target == null ) yield break;
        #region Xac dinh la ban Enemy phia tren hay phia duoi
		if (transform.position.y < target.transform.position.y) // thực hiện bắn mục tiểu ở vị trí Y cao hơn
        {
            if (enemies.Count == 0 )
            {
                yield break;
            }
            Solider_Anim1.SetTrigger("Atk_up");
			yield return new WaitForSeconds(0.5f); // Sau thời gian nào đó sẽ sinh ra mũi tên
            MakeArrow(pos1_up);
            if (enemies.Count == 0 )
            {
                yield break;
            }
            Solider_Anim2.SetTrigger("Atk_up");
			yield return new WaitForSeconds(0.5f);// sau thời gian nào đó sẽ sinh ra mũi tên 2
            MakeArrow(pos2_up);
        }
		else // thực hiện bắn mục tiểu ở vị trí Y thấp hơn
        {
            if (enemies.Count == 0 )
            {
                yield break;
            }
            Solider_Anim1.SetTrigger("Atk_down");
            yield return new WaitForSeconds(0.5f);
            MakeArrow(pos1_down);
            if (enemies.Count == 0)
            {
                yield break;
            }
            Solider_Anim2.SetTrigger("Atk_down");
            yield return new WaitForSeconds(0.5f);
            MakeArrow(pos2_down);
        }
        #endregion
    } 
    private void SetShootingFalse()// cài đặt isShooting = false
    {
        isShooting = false;
    } 
    private void MakeArrow(Transform pos)// Hàm Instance tên
    {
        GameObject arrow = Instantiate(Resources.Load("Archer/Arrow"), pos.transform.position, Quaternion.identity) as GameObject;
        Parabol _arrow = arrow.GetComponent<Parabol>();
        _arrow.Dame = Dame;
        if (enemies.Count > 0)
        {
            if (target == null && enemies[0] != null) target = enemies[0];
            _arrow.target = target;
            _arrow.maxLaunch = getminSpeed();
        }
        else
        {
            Destroy(arrow);
        }
    }
    #region Ham truyen tham so cho mui ten bay
    private float getminSpeed()
    {
        float aux = 1f;
        while (moreSpeed(aux) == true)
        { aux = aux + searchvalue; }
        return aux;
    }
    private bool moreSpeed(float speed)
    {
        if (enemies[0] == null) return false;
        bool aux = false;
        float xTarget = enemies[0].transform.position.x;
        float yTarget = enemies[0].transform.position.y;
        float xCurrent = transform.position.x;
        float yCurrent = transform.position.y;
        float xDistance = Mathf.Abs(xTarget - xCurrent);
        float yDistance = yTarget - yCurrent;
        float fireAngle = 1.57075f - (float)(Mathf.Atan((Mathf.Pow(speed, 2f) + Mathf.Sqrt(Mathf.Pow(speed, 4f) - 9.8f * (9.8f * Mathf.Pow(xDistance, 2f) + 2f * yDistance * Mathf.Pow(speed, 2f)))) / (9.8f * xDistance)));
        float xSpeed = (float)Mathf.Sin(fireAngle) * speed;
        float ySpeed = (float)Mathf.Cos(fireAngle) * speed;
        if ((xTarget - xCurrent) < 0f) { xSpeed = -xSpeed; }
        if (float.IsNaN(xSpeed) || float.IsNaN(ySpeed)) { aux = true; }
        return aux;
    }
    #endregion
    public void AddEnemy(GameObject enemy)// thêm enemy vào List
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)// xóa Enemy khỏi List
    {
        if (enemy != null)
        {
            enemies.Remove(enemy);
        }
    }
    public void ShowNextZone(bool turn) // bật/tắt tầm đánh sau khi upgrade
    {
        NextZone.SetActive(turn);
    }
    public void ShowCurrentZone(bool turn)// bật/tắt tầm đánh hiện tại
    {
        CurrentZone.SetActive(turn);
    }
    //-----------GUN----------------
    IEnumerator Shoot_bullet()
    {
        RemoveNull();
        if (target == null)
        {
            yield break;
        }
        //------quay mat solider trái phải theo huong enemy--------
        if (transform.position.x < target.transform.position.x)
        {
            transform.Find("Solider").transform.localScale = new Vector3(1, 1, 1);
            transform.Find("Solider2").transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.Find("Solider").transform.localScale = new Vector3(-1, 1, 1);
            transform.Find("Solider2").transform.localScale = new Vector3(-1, 1, 1);
        }
        if (target == null)
        {
            yield break;
        }
        // GOi cac Animation Up
        if (transform.position.y < target.transform.position.y)
        {
                Solider_Anim1.SetTrigger("Atk_up");
                yield return new WaitForSeconds(0.5f);
                MakeBullet(pos1_up, false, false);
                Solider_Anim2.SetTrigger("Atk_up");
                yield return new WaitForSeconds(0.5f);
                MakeBullet(pos1_up, false, false);            
        }
        else
        {
                Solider_Anim1.SetTrigger("Atk_down");
                yield return new WaitForSeconds(0.5f);
                MakeBullet(pos1_down, false, false);
                Solider_Anim2.SetTrigger("Atk_down");
                yield return new WaitForSeconds(0.5f);
                MakeBullet(pos2_down, false, false);
        }
    }  // hàm thực hiện việc bắn súng
    private void MakeBullet(Transform pos, bool oneshot, bool shootgun)// Tạo đạn cho nhà súng
    {
            GameObject bullet = Instantiate(Resources.Load("Archer/Bullet_gun"), pos.transform.position, Quaternion.identity) as GameObject;
            Gun _bullet = bullet.GetComponent<Gun>();
            if (oneshot == false) // Nếu là đạn thường thì sẽ có lượng dame cố định
                _bullet.dame = Dame;
            else// Nếu là Headshot thì dame =500
                _bullet.dame = 500;
            if (enemies.Count == 0)
            {
                Destroy(bullet);
            }
            else
            {
                if (target == null && enemies[0] != null) target = enemies[0];
                _bullet.enemy = target;
            }              
    }
    //---------- Bat giao dien UI Infomation Acher------------
    private void EnableUI_Infomation()
    {
        _canvas.enabled = true;
        _canvas.transform.GetChild(1).gameObject.SetActive(false);
        _canvas.transform.GetChild(2).gameObject.SetActive(false);
        _canvas.transform.GetChild(3).gameObject.SetActive(false);
        _canvas.transform.GetChild(0).gameObject.SetActive(true);
        dametext.text = Dame.ToString();
        if (coldown <= 0.5) { Speedtext.text = "Very Fast"; }
        else if (coldown > 0.5 && coldown < 1) { Speedtext.text = "Fast"; }
        else if (coldown > 1 && coldown < 1.5) { Speedtext.text = "Medium"; }
        else { Speedtext.text = "Slow"; }
        if (Archer_Index == 0) { rangetext.text = "Short"; }
        else if (Archer_Index == 1) { rangetext.text = "Average"; }
        else if (Archer_Index == 2) { rangetext.text = "Long"; }
        else if (Archer_Index == 3) { rangetext.text = "Great"; }
        else { rangetext.text = "Extreme"; }
    }
    void RemoveNull()// xóa các obj null khỏi List
    {

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
                enemies.Remove(enemies[i]);
        }
    }
}
