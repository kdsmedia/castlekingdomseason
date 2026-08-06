using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mage : MonoBehaviour
{

    //------Property-------
    public int Mage_index, next_index; // chỉ số của nhà Mage 
    private GameObject witch; // obj phù thủy của nhà Mage
    private Animator _anim; // Animation của nhà Mage
    private Animator Witch_Anim;// Animation của phù thủy
    public List<GameObject> enemies = new List<GameObject>();// Danh sách Enemy trong tầm tấn công
    public GameObject target = null;// mục tiêu được chọn để tấn công
    public float coldown;// thời gian colddown
    public Transform posdown, posup; // vị trí sinh đạn khi bắn ở phía trên và phía dưới
    public int dame; // Dame của nhà Mage
    private GameObject Interface = null; // Giao diện khi click vào nhà Mage
    private GameObject CurrentZone, NextZone; // Tầm đánh của trụ hại tại và tương lai khi Upgrade
    //----
    public bool isShooting = false; // có đang trong trạng thái bắn enemy hay không
    private bool shootUp; // phù thủy bắn enemy phía trên nó hay phía dưới nó
    //-----------MAGE3---------------
    public Transform pos_laze; // vị trí sinh ra Laze
    //----------UI Info---------------
    private Canvas _canvas;
    private Text Speedtext, dametext, rangetext;
    void Start()
    {
        Upgrade_Peoperty(); // Kiểm tra xem chức năng Upgrade tại Panel Select level đã được chọn chưa
        #region Khoi tao UI Info
        _canvas = GameObject.Find("Canvas_Info").GetComponent<Canvas>();
        dametext = _canvas.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>(); // Dòng Text hiển thị Dame trên UI
        Speedtext = _canvas.transform.GetChild(3).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();// Dòng Text hiển thị Speed trên UI
        rangetext = _canvas.transform.GetChild(3).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();// Dòng Text hiển thị Range trên UI
        #endregion
        #region Khoi tao Zone
        next_index = Mage_index + 1;
        CurrentZone = Instantiate(Resources.Load("Mage/Zone" + Mage_index)) as GameObject;
        CurrentZone.transform.parent = transform;
        CurrentZone.transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
        CurrentZone.GetComponent<SpriteRenderer>().color = Color.blue;
        CurrentZone.SetActive(false);
        if (Resources.Load("Mage/Zone" + next_index) != null)
        {
            NextZone = Instantiate(Resources.Load("Mage/Zone" + next_index)) as GameObject;
            NextZone.transform.parent = transform;
            NextZone.transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
            NextZone.SetActive(false);
        }
        #endregion
        _anim = GetComponent<Animator>(); // Animation của nhà Mage
        InvokeRepeating("SlowUpdate", 0, 0.2f);
        Witch_Anim = transform.Find("Witch").GetComponent<Animator>(); // Animation của phù thủy
        SoundTower._instance.MageTower();
        int layer = Mathf.Clamp(Mathf.Abs(50 - Mathf.RoundToInt((transform.position.y - 1) * 15)), 1, 150); // cài đặt layer theo vị trí Y
        GetComponent<SpriteRenderer>().sortingOrder = layer;
    }
    private void Upgrade_Peoperty()// Kiểm tra xem chức năng Upgrade tại Panel Select level đã được chọn chưa
    {
        if (PlayerPrefs.GetInt("Mageupgrade1") == 1)
        {
            GameObject zone = transform.Find("Mage").gameObject;
            zone.transform.localScale = new Vector2(zone.transform.localScale.x + 0.1f, zone.transform.localScale.y + 0.1f);
        }
        if (PlayerPrefs.GetInt("Mageupgrade2") == 1)
        {
            dame += 5;
        }
        if (PlayerPrefs.GetInt("Mageupgrade4") == 1)
        {
            dame += 5;
        }
        if (PlayerPrefs.GetInt("Mageupgrade5") == 1)
        {
            dame += 5;
        }
    }
    void SlowUpdate()
    {
        if (Manager.isFinishing == true) return; // Nếu Game đã kết thúc thì return
        RemoveNull();
        if (enemies.Count == 0) // Nếu List Enemy rỗng thì return
        {
            return;
        }
        else // Ngược lại gán Target là Enemy đầu tiên
        {
            target = enemies[0];
        }

        if (target == null && enemies.Count > 0)
        {
            target = enemies[0];
        }
        else
        {
            if (isShooting == false)
            {
                isShooting = true;
                Invoke("SetShootingFalse", coldown); // chuyển sang trạng thía isShooting=false sau thời gian coldown
                StartCoroutine(Shoot());
            }
        }
    }
    void Update()
    {
        if (Manager.isFinishing == true) return;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Vector3.zero);
        if (hit.collider == null)
        {
            if (Input.GetMouseButton(0))
            {
                _canvas.enabled = false;
                _canvas.transform.GetChild(3).gameObject.SetActive(false);
            }
        }
        else
        {
            if (hit.collider.gameObject.tag == "LimitSpell")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _canvas.enabled = false;
                    _canvas.transform.GetChild(3).gameObject.SetActive(false);
                }
            }
        }
    }
    IEnumerator Shoot()// Hàm sinh Đạn hoặc Laze
    {
        _anim.SetTrigger("Fire");// Nhà Mage chạy Animation bắn
        if (enemies.Count > 0)
            CheckFlip(target);
        else
            yield break;

        if (shootUp) // Chạy Animation bắn Enemy có vị trí Y cao hơn vị trí của nhà Mage
        {
            Witch_Anim.SetTrigger("Up");
            yield return new WaitForSeconds(0.35f);
            if (Mage_index < 3 || Mage_index == 4) // Nếu là nhà Mage nhỏ hoặc nhà Mage 4 thì bắn viên đạn
                MakeBullet(posup.position);
            else
                MakeLaze(); // Nếu là Mage 3 thì bắn Laze
        }
        else// Chạy Animation bắn Enemy có vị trí Y thấp hơn vị trí của nhà Mage
        {
            Witch_Anim.SetTrigger("Down");
            yield return new WaitForSeconds(0.4f);
            if (Mage_index < 3 || Mage_index == 4) // Nếu là nhà Mage nhỏ hoặc nhà Mage 4 thì bắn viên đạn
                MakeBullet(posup.position);
            else// Nếu là Mage 3 thì bắn Laze
                MakeLaze();
        }
    } 
    private void SetShootingFalse() // đặt isShooting =false
    {
        isShooting = false;
    }
    public void MakeBullet(Vector3 pos) // Hàm Instance ra đạn
    {
        if (Mage_index == 4) // Nếu là nhà Mage 4 thì load đạn riêng cho nó
        {
            GameObject bullet = Instantiate(Resources.Load("Mage/Mage_bullet"), pos, Quaternion.identity) as GameObject;
            RemoveNull();
            if (enemies.Count > 0)
            {
                if (target == null && enemies[0] != null)
                {
                    target = enemies[0];
                }
                bullet.GetComponent<Ball>().target = this.target;
            }
            else
                Destroy(bullet);
            bullet.GetComponent<Ball>().dame = this.dame;
        }
        else // Còn lại thì Load 1 loại đạn chung
        {
            GameObject bullet = Instantiate(Resources.Load("Mage/Ball"), pos, Quaternion.identity) as GameObject;
            if (enemies.Count > 0)
            {
                if (target == null && enemies[0] != null)
                {
                    target = enemies[0];
                }
                bullet.GetComponent<Ball>().target = this.target;
            }
            else
                Destroy(bullet);
            bullet.GetComponent<Ball>().dame = this.dame;
        }
    }
    public void MakeLaze() // Hàm Instance ra laze
    {
        SoundTower._instance.MageLaze();
        int type = Random.Range(1, 3);
        if (type == 1) // loại 1 là tia laze cong chỉ dùng cho nhà Mage 3
        {
            GameObject bullet = Instantiate(Resources.Load("Mage/Laze" + type), pos_laze.position, Quaternion.identity) as GameObject;
            RemoveNull();
            if (enemies.Count > 0)
            {
                if (target == null && enemies[0] != null)
                {
                    target = enemies[0];
                }
                target = enemies[0];
                bullet.GetComponent<Light>().Target = this.target;
            }
            else
                Destroy(bullet);
            bullet.GetComponent<Light>().dame = this.dame;
        }
        else if (type == 2) // loại 2 là tia Laze thẳng thuộc chức năng upgrade của nhà Mage 3
        {
            GameObject bullet = Instantiate(Resources.Load("Mage/Laze" + type), pos_laze.position, Quaternion.identity) as GameObject;
            RemoveNull();
            if (enemies.Count > 0)
            {
                if (target == null && enemies[0] != null)
                {
                    target = enemies[0];
                }
                target = enemies[0];
                bullet.GetComponent<Light>().Target = this.target;
            }
            else
                Destroy(bullet);
            bullet.GetComponent<Light>().dame = this.dame;
        }
    }
    private void CheckFlip(GameObject target)// So Sánh vị trí Y của nhà Mage với Enemy
    {
        if (transform.position.y <= target.transform.position.y)
        {
            shootUp = true;
        }
        else
        {
            shootUp = false;
        }
    } 
    public void AddEnemy(GameObject enemy)// thêm Enemy vào List
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }
    public void RemoveEnemy(GameObject enemy)// xóa Enemy khỏi List
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }
    private void RemoveNull()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
                enemies.Remove(enemies[i]);
        }
    }// Xóa các obj null khỏi List
    void OnMouseDown()// bật/tắt giao diện UI_info và obj upgrade
    {
        SoundTower._instance.Click();
        if (Mage_index < 3) // nếu các trụ là loại cấp thấp từ 0 đến 2
        {
            if (Interface == null) // Nếu giao diện nâng cấp chưa tồn tại thì Instance
            {
                Interface = Instantiate(Resources.Load("Interface/MageInterface" + Mage_index), transform.position, Quaternion.identity) as GameObject;
                Interface.transform.parent = this.transform;
                CurrentZone.SetActive(true);// hiện tầm đánh hiện tại
                EnableUI_Infomation(); // hiển thị UI thông số của nhà Mage
            }
            else // Nếu đã tồn tại giao diện nâng cấp thì hủy
            {
                CurrentZone.SetActive(false); // tắt tầm đánh hiện tại
                Destroy(Interface, 0.2f); // Hủy giao diện
                _canvas.enabled = false; // tắt UI infomation của nhà Mage
                _canvas.transform.GetChild(3).gameObject.SetActive(false);// tắt UI infomation của nhà Mage
            }
        }
        else // nếu là nhà Mage cấp 3 hoặc 4
        {
            if (Interface == null)
            {
                Interface = Instantiate(Resources.Load("Interface/MageInterface" + Mage_index), transform.position, Quaternion.identity) as GameObject;
                Interface.transform.parent = this.transform;
                EnableUI_Infomation();// hiển thị UI_info 
            }
            else
            {
                if (Interface.activeSelf == false) // Nếu giao diện bị Disactive thì bật nó lên
                {
                    Interface.SetActive(true);
                    EnableUI_Infomation();
                }
                else // Nếu giao diện đang Active thì tắt nó đi
                {
                    Interface.SetActive(false);
                    _canvas.enabled = false;
                    GameObject.Find("Canvas_Tower").GetComponent<Canvas>().enabled = false;
                    _canvas.transform.GetChild(3).gameObject.SetActive(false);
                }
            }
        }
    }
    public void ShowNextZone(bool turn)// Bật /tắt Range đánh tiêp theo
    {
        NextZone.SetActive(turn);
    } 
    public void ShowCurrentZone(bool turn)// Bật /tắt Range đánh hiện tại
    {
        CurrentZone.SetActive(turn);
    }
    private void EnableUI_Infomation()// Hiển thị UI_Infomation
    {
        _canvas.enabled = true;
        _canvas.transform.GetChild(0).gameObject.SetActive(false);
        _canvas.transform.GetChild(1).gameObject.SetActive(false);
        _canvas.transform.GetChild(2).gameObject.SetActive(false);
        _canvas.transform.GetChild(3).gameObject.SetActive(true);
        dametext.text = dame.ToString();
        if (coldown <= 0.5) { Speedtext.text = "Very Fast"; }
        else if (coldown > 0.5 && coldown < 1) { Speedtext.text = "Fast"; }
        else if (coldown > 1 && coldown < 1.5) { Speedtext.text = "Medium"; }
        else { Speedtext.text = "Slow"; }
        if (Mage_index == 0) { rangetext.text = "Short"; }
        else if (Mage_index == 1) { rangetext.text = "Average"; }
        else if (Mage_index == 2) { rangetext.text = "Long"; }
        else if (Mage_index == 3) { rangetext.text = "Great"; }
        else { rangetext.text = "Great"; }
    } 
}
