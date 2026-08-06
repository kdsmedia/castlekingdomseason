using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //------ cac thuoc tinh
    public float maxHeal;// Máu của Enemy
    public float currentHeal { get; set; }
    public int Dame;// Dame Enemy
    public float movespeed;// tốc độ di chuyển
    public int Armor;// giáp
    public int ResistanMagic;// kháng phép
    private float searchvalue = 0.1f;
    //------------
    private Transform slider; // Thanh máu
    private Animator _anim; // Animation của Enemy
    private Transform target;// check Flip với Target nay
    public bool AllowMove = true;
    public bool FightToDeath { get; set; } // Kiểm tra xem Enemy này có đang thực hiện tấn công Knight ko
    public bool isDead = false;
    //-----Path
    public GameObject Path; // GameObject chứa các điểm để Enemy đi theo
    private List<GameObject> Paths = new List<GameObject>(); // Danh sách các path có trên bản đồ
    public List<Vector3> Paths_Pos = new List<Vector3>(); // vị trí của các path trong DS paths
    private Vector3 offset; // độ dịch với vị trí path
    public Vector3 PointToMove; // vị trí path gần nhất để đi tới
    public GameObject _KnightToFight = null; // Knight cần phải tấn công
    public bool StartAttack; // =true thì sẽ tấn công Knight
    public bool isRange; // là Enemy đánh gần hay xa
    private bool isAttacking = false; // có đang tấn công hay ko
    public float radius; // tầm đánh
    public GameObject bloodEffect; // Hiệu ứng vũng máu
    public Transform blood_pos; //vị trí vũng máu khi Enemy chết
    public int coin; // số tiền kiếm đc khi giết Enemy
    private SpriteRenderer _sprite;
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        StartAttack = false;
        FightToDeath = false;
        currentHeal = maxHeal;
        slider = transform.Find("HealBar").transform.Find("slider").transform;
        _anim = GetComponent<Animator>();
        _anim.SetInteger("Change", 0);// trang thai mac dinh dau tien la chay
        FindPath();
        if (isRange) // Nếu là Enemy bắn cung thì chạy hàm tìm kiếm và tấn công Knight
        {
            InvokeRepeating("FindKnight", 0, 1f);
        }
    }
    public void FindPath()    // Tìm vị trí path trên bản đồ 
    {
        for (int i = 0; i < Path.transform.childCount; i++)
        {
            if (!Paths.Contains(Path.transform.GetChild(i).gameObject))
            {
                Paths.Add(Path.transform.GetChild(i).gameObject);
            }
        }
        // Paths = GameObject.FindGameObjectsWithTag("Path");
        foreach (GameObject path in Paths)
        {
            offset = new Vector3(Random.Range(0.05f, 0.1f), Random.Range(0.05f, 0.3f), 0);
            Vector3 pathpos = path.transform.position + offset;
            Paths_Pos.Add(pathpos);
        }
    }
    private void MoveToPath()    // Ham di chuyen den path
    {
        float neareset_path = 100f;
        for (int i = 0; i < Paths_Pos.Count; i++) // chay xong vong lap nay se tim duoc path ngan nhat
        {
            float distance = Vector2.Distance(transform.position, Paths_Pos[i]);
            if (distance <= neareset_path)
            {
                neareset_path = distance;
                PointToMove = Paths_Pos[i]; //toa do path ngan nhat
            }
        }
        //-- di chuyen den path
        if (transform.position != PointToMove) // neu khac toa do voi điểm gần nhất thì di chuyển đến nó
        {
            CheckFlip(PointToMove); // check Flip voi diem Path
            transform.position = Vector2.MoveTowards(transform.position, PointToMove, Time.deltaTime * movespeed);// di chuyen den path
            if (transform.position == Paths_Pos[Paths_Pos.Count - 1]) // Nếu di chuyển đến path cuối cùng thì sẽ trừ máu Player
            {
                Manager._instance.TakeHeart();
                Destroy(gameObject);
            }
            if (transform.position.y >= PointToMove.y && Mathf.Abs(transform.position.x - PointToMove.x) <= 0.2f && Mathf.Abs(transform.position.y - PointToMove.y) > 0.2f) // Chạy xuống
                _anim.SetInteger("Change", 2);
            else if (transform.position.y < PointToMove.y && Mathf.Abs(transform.position.x - PointToMove.x) <= 0.2f && Mathf.Abs(transform.position.y - PointToMove.y) > 0.2f) // Chạy lên
                _anim.SetInteger("Change", 1);
            else
                _anim.SetInteger("Change", 0);// Trạng thái chạy ngang phải
        }
        else
        {
            Paths_Pos.Remove(PointToMove);
        }
    }
    void Update()
    {
        if (Manager.isFinishing == true) { _anim.enabled = false; return; }
        // đặt layer theo vị trí tọa độ Y
        int layer = Mathf.Clamp(Mathf.Abs(50 - Mathf.RoundToInt((transform.position.y - 1) * 15)), 1, 150);
        _sprite.sortingOrder = layer;
        if (isDead) return;
        if (!StartAttack) //Nếu không phải tấn công Knight thì sẽ di chuyển theo Path
        {
            MoveToPath();
        }
        else// ngược lại sẽ tấn công Knight
        {
            if (_KnightToFight == null) // Neu Knight bi giet thì nó sẽ tiếp tục di chuyển đến path
            {
                StartAttack = false;
                FightToDeath = false;
            }
            else // Nếu Knight còn sống thì tấn công
            {
                if (!isRange && !isAttacking) // Dành cho Enemy đánh gần
                {
                    isAttacking = true;
                    AttackKnight();
                    Invoke("SetAttackingFalse", 1f);
                }
                else if (isRange && !isAttacking) // dành cho Enemy đánh xa
                {
                    float distance = Vector2.Distance(transform.position, _KnightToFight.transform.position);
                    if (distance > 0.3f && distance <= radius) // nếu khoảng cách tấn công trong khoảng 0.3-radius thì bắn cung
                    {
                        isAttacking = true;
                        ShootKnight();
                        Invoke("SetAttackingFalse", 1f);
                    }
                    else if (distance <= 0.3f)// ngược lại thì sẽ tấn công bằng dao
                    {
                        isAttacking = true;
                        AttackKnight();
                        Invoke("SetAttackingFalse", 1f);
                    }
                    else if (_KnightToFight != null)
                    {
                        _KnightToFight = null;
                    }
                }
            }
        }
    }
    private void SetAttackingFalse() // đặt isAttacking =false
    {
        isAttacking = false;
    }
    private void CheckFlip(Vector3 pos) // kiểm tra việc quay mặt với tọa độ pos
    {
        if (transform.position.x <= pos.x)
        {
            if (transform.localScale.x == -1)
            {
                transform.localScale = new Vector3(1, 1, 1);
                slider.parent.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            }

        }
        else
        {
            if (transform.localScale.x == 1)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                slider.parent.transform.localScale = new Vector3(-0.3f, 0.3f, 1f);
            }
        }
    }
    public void TakeDamePhysic(int _dame) // Hàm nhận Dame từ sát thương vật lý
    {
        int truedame = _dame - Armor; // Dame sau khi da tru giap
        if (currentHeal > 0)
        {
            Sound_Enemy._instance.Hurt();
            currentHeal = currentHeal - truedame;
            if (currentHeal > 0)
                slider.transform.localScale = new Vector3((1 / maxHeal) * currentHeal, transform.localScale.y, transform.localScale.z); // thay doi thanh mau
            else
            {
                slider.transform.localScale = new Vector3(0, 1, 1);
                Die();
            }
        }
        else
            Die();
    }
    public void TakeDamageMagic(int _dame)// Hàm nhận Dame từ sát thương vật lý
    {
        int truedame = _dame - ResistanMagic; // Dame sau khi da tru khang phep
        if (currentHeal > 0)
        {
            currentHeal = currentHeal - truedame;
            if (currentHeal > 0)
                slider.transform.localScale = new Vector3((1 / maxHeal) * currentHeal, transform.localScale.y, transform.localScale.z); // thay doi thanh mau
            else
            {
                slider.transform.localScale = new Vector3(0, 1, 1);
                Die();
            }
        }
        else
            Die();
    }
    private void Die()
    {
        Sound_Enemy._instance.Die();
        _anim.SetInteger("Change", 6); // ANimation die   
        isDead = true;
        Instantiate(bloodEffect, blood_pos.position, Quaternion.identity);
        Manager.money += coin;
        Manager._instance.setcoin();
        Destroy(gameObject, 1f);
    }
    private void AttackKnight()// Hàm tấn công tầm gần
    {
        _anim.SetInteger("Change", 4);
        Invoke("GiveDame", 0.5f);
        CheckFlip(_KnightToFight.gameObject.transform.position);
    }
    public void GiveDame() // Hàm gây dame cho Knight
    {
        if (_KnightToFight != null) // Neu Knight con song
        {
            if (_KnightToFight.GetComponent<KnightController>() != null)
            {
                _KnightToFight.GetComponent<KnightController>().TakeDame(Dame);
                if (_KnightToFight.GetComponent<KnightController>().currentHeal <= 0)
                {
                    _KnightToFight = null;
                    StartAttack = false;
                }
            }
            else if (_KnightToFight.GetComponent<Golem>() != null)
            {
                _KnightToFight.GetComponent<Golem>().TakeDame(Dame);
                if (_KnightToFight.GetComponent<Golem>().currentHeal <= 0)
                {
                    _KnightToFight = null;
                    StartAttack = false;
                }
            }
            else if (_KnightToFight.GetComponent<Hero>() != null)
            {
                _KnightToFight.GetComponent<Hero>().TakeDame(Dame);
                if (_KnightToFight.GetComponent<Hero>().currentHeal <= 0)
                {
                    _KnightToFight = null;
                    StartAttack = false;
                }
            }
        }
    }
    //-------------Range Enemy--------------
    #region Ham truyen tham so cho mui ten bay
    private float getminSpeed()
    {
        float aux = 0.1f;
        while (moreSpeed(aux) == true)
        { aux = aux + searchvalue; }
        return aux;
    }
    private bool moreSpeed(float speed)
    {
        bool aux = false;
        float xTarget = _KnightToFight.transform.position.x;
        float yTarget = _KnightToFight.transform.position.y;
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
    private void FindKnight() // Tìm Knight để tấn công
    {
        GameObject[] Knights = GameObject.FindGameObjectsWithTag("Knight");
        float nearestDistance = 10f;
        foreach (GameObject knight in Knights)
        {
            float distan = Vector3.Distance(transform.position, knight.transform.position);
            if (distan < nearestDistance)
            {
                nearestDistance = distan;
                _KnightToFight = knight;
            }
        }
        if (nearestDistance < radius)
        {
            StartAttack = true;
        }
    }
    private void ShootKnight() // thực hiện việc bắn tên vào mục tiêu
    {
        CheckFlip(_KnightToFight.transform.position);
        _anim.SetInteger("Change", 5);
        GameObject arrow = Instantiate(Resources.Load("Archer/Arrow"), transform.position, Quaternion.identity) as GameObject;
        Parabol _arrow = arrow.GetComponent<Parabol>();
        _arrow.target = _KnightToFight;
        _arrow.Dame = Dame;
        if (_KnightToFight != null)
        {
            _arrow.maxLaunch = getminSpeed();
        }
        if (_KnightToFight == null)
            Destroy(arrow);
    }
}
