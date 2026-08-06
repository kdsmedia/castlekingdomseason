using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    // các thuộc tính cơ bản của Golem
    public int dame;
    public int armor;
    public float moveSpeed;
    public float maxHeal;
    public float currentHeal;
    public int timerespawn;
    private Transform slider;
    public float coldown;

    public GameObject target = null;
    [HideInInspector]
    public Animator _anim;
    public Vector3 positionFlag { get; set; }
    public List<GameObject> ListEnemy = new List<GameObject>();

    public float radius;
    private Mage_Golem _mage;
    public bool isGolem, isKnight,isHero;

    //------
    public bool isDead = false;
    public EnemyController _target = null;
    public bool isAttacking = false;
    public bool allowMove = true;
    private float timeregend = 0;
    public bool MustReturn = false;
    private EnemyController _enemycode;
    private Vector3 pos_origin;
    private SpriteRenderer _sprite;
    // Use this for initialization
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        SoundTower._instance.SoliderSpawn();
        currentHeal = maxHeal;
        if (isGolem)
        {
            _mage = transform.parent.GetComponent<Mage_Golem>();
            InvokeRepeating("CheckUpgrade", 0f, 0.5f);
        }
        else if (!isKnight && !isGolem)
        {
            UpgradeProperty_Soldier();
            Invoke("Die", 15f);
        }
        pos_origin = transform.position;
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
        // radius = 0.3f;
        slider = transform.Find("HealBar").transform.Find("slider");
        _anim = GetComponent<Animator>();

    }
    private void UpgradeProperty_Soldier()
    {
        if (PlayerPrefs.GetInt("Soliderupgrade1") == 1)
        {
            dame += 1;
            maxHeal = maxHeal + 0.1f * maxHeal;
            currentHeal = maxHeal;
        }
        if (PlayerPrefs.GetInt("Soliderupgrade2") == 1)
        {
            maxHeal = maxHeal + 0.2f * maxHeal;
            currentHeal = maxHeal;
            armor += 1;
        }
        if (PlayerPrefs.GetInt("Soliderupgrade3") == 1)
        {
            maxHeal = maxHeal + 0.3f * maxHeal;
            currentHeal = maxHeal;
            armor += 2;
        }
        if (PlayerPrefs.GetInt("Soliderupgrade4") == 1)
        {
            maxHeal = maxHeal + 0.4f * maxHeal;
            currentHeal = maxHeal;
            armor += 3;
        }
        if (PlayerPrefs.GetInt("Soliderupgrade5") == 1)
        {
            maxHeal = maxHeal + 0.4f * maxHeal;
            currentHeal = maxHeal;
            armor += 3;
            dame += 3;
        }
    }
    private void CheckUpgrade()
    {
        if (_mage.levelgolem == 2) { maxHeal = 350; dame = 15; }
        else if (_mage.levelgolem == 3) { maxHeal = 400; dame = 20; }
    }
    private void Regend()
    {
        timeregend += Time.deltaTime;
        if (timeregend >= 3)
        {
            if (currentHeal > 0 && currentHeal < maxHeal)
            {
                currentHeal += Time.deltaTime * 4;
                slider.transform.localScale = new Vector3((1 / maxHeal) * currentHeal, 1, 1);
            }
            else if (currentHeal >= maxHeal)
            {
                currentHeal = maxHeal;
                slider.transform.localScale = new Vector3(1, 1, 1);
                timeregend = 0f;
            }
        }
    }
    void UpdateTarget() // tim muc tieu de chon va tan cong
    {
        if (Manager.isFinishing == true) return;
        if (target == null) MustReturn = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<EnemyController>() != null)
            {
                _enemycode = enemy.GetComponent<EnemyController>();
            }
            if (enemy == null) return;
            float range = Vector2.Distance(transform.position, enemy.transform.position); // khoang cach giua Knight va Enemy
            if (range <= radius && _enemycode.currentHeal > 0)// neu khoang cach < radius  thi them vao List
            {
                if (!ListEnemy.Contains(enemy) && enemy != null) { ListEnemy.Add(enemy); } // neu List chua co phan tu nay thi them vao

            }
            else // neu o ngoai radius thi loai khoi List
                ListEnemy.Remove(enemy);
        }
        RemoveNull();
        // Chạy ListeEnemy xem phần tử nào đã lock target là chính Obj thì sẽ return
        foreach (GameObject enemy in ListEnemy)
        {
            if (enemy == null) return;
            if (enemy.GetComponent<EnemyController>()._KnightToFight == gameObject)
            {
                SetTarget(enemy);
                MustReturn = true;
                return;
            }
        }
        if (MustReturn == true) return;
        //------KIỂM TRA LISTENEMY để chọn target-----------
        if (ListEnemy.Count == 1) // neu trong List chi co 1 thi tan cong Enemy do
        {
            isAttacking = false;
            if (ListEnemy[0] == null) return;
            SetTarget(ListEnemy[0]);
            if (CheckEnemyToFight4Ever(this, ListEnemy[0]))
            {
                MustReturn = true;
            }
            else if (!CheckEnemyToFight4Ever(this, ListEnemy[0]) && ListEnemy[0].GetComponent<EnemyController>()._KnightToFight == null)// nếu ko phải đánh tới chết và enemy chưa locktarget thì locktarget cho enemy là chính obj
            {
                isAttacking = false;
                ListEnemy[0].GetComponent<EnemyController>()._KnightToFight = this.gameObject;
            }
        }
        if (MustReturn == true) return;
        else if (ListEnemy.Count == 2)
        {
            if (ListEnemy[0] == null || ListEnemy[1] == null) return;
            isAttacking = false;
            RemoveNull();
            for (int i = 0; i <= 1; i++)
            {

                if (CheckEnemyToFight4Ever(this, ListEnemy[i]) == true) // neu check duoc phai danh toi chet enemy nay thi tan cong
                {
                    print("da chon duoc Enemy o vong for thu" + i);
                    SetTarget(ListEnemy[i]);
                    MustReturn = true;
                    break;
                }
                else if (!CheckEnemyToFight4Ever(this, ListEnemy[i]) && ListEnemy[i].GetComponent<EnemyController>()._KnightToFight == null)
                {
                    if (MustReturn == true) return;
                    ListEnemy[i].GetComponent<EnemyController>()._KnightToFight = this.gameObject;
                    SetTarget(ListEnemy[i]);
                    MustReturn = true;
                    break;
                }

            }
            // neu thay ca 2 deu da co Knight chon tan cong den chet thi chon con thu 2
            if (!CheckEnemyToFight4Ever(this, ListEnemy[0]) && !CheckEnemyToFight4Ever(this, ListEnemy[1]) && target == null)
            {
                SetTarget(ListEnemy[1]);
                MustReturn = true;
            }
        }
        else if (ListEnemy.Count >= 3)
        {
            RemoveNull();
            isAttacking = false;
            if (ListEnemy[0] == null || ListEnemy[1] == null || ListEnemy[2] == null) return;
            // Kiểm tra xem tất cả các Enemy nếu có 1 con locktarget là obj này thì break;
            foreach (GameObject enemy in ListEnemy)
            {
                // RemoveNull();
                if (CheckEnemyToFight4Ever(this, enemy)) // neu check fai danh toi chet thi tan cong enemy nay
                {
                    SetTarget(enemy);
                    MustReturn = true;
                    break;
                }
            }
            if (MustReturn == true) return;
            // Nếu ko bị lock thì chọn enemy để lock
            foreach (GameObject enemy in ListEnemy)
            {
                // RemoveNull();
                if (!CheckEnemyToFight4Ever(this, enemy) && enemy.GetComponent<EnemyController>()._KnightToFight == null) // neu check fai danh toi chet thi tan cong enemy nay
                {
                    if (MustReturn == true) return;
                    enemy.GetComponent<EnemyController>()._KnightToFight = this.gameObject;
                    SetTarget(enemy);
                    MustReturn = true;
                    break;
                }
            }
            if (!CheckEnemyToFight4Ever(this, ListEnemy[0]) && !CheckEnemyToFight4Ever(this, ListEnemy[1]) && !CheckEnemyToFight4Ever(this, ListEnemy[2]) && target == null)
            {
                SetTarget(ListEnemy[Random.Range(0, 3)]);
                MustReturn = true;
            }
        }
    }
    private void SetTarget(GameObject enemy)
    {
        // isAttacking = false;
        target = enemy;
        _target = target.GetComponent<EnemyController>();
    }
    private bool CheckEnemyToFight4Ever(Golem knight, GameObject enemy)
    {
        GameObject KnightToFight = enemy.GetComponent<EnemyController>()._KnightToFight;
        if (KnightToFight == knight.gameObject) // neu Nếu KnightToFight ko phải là Obj này
        {
            return true;
        }
        else
            return false;
    }

    private void setISAttackingfalse()
    {
        // print("Ham set Atk false da duoc goi");
        isAttacking = false;
        _anim.SetInteger("Change", 0); // moi them vao 7/5
    }
    // Update is called once per frame
    public void CheckFlipWith(GameObject enemy)
    {
        if (transform.position.x <= enemy.transform.position.x)
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
    void Update()
    {
        if (Manager.isFinishing == true) { _anim.enabled = false; return; }
        int layer = Mathf.Clamp(Mathf.Abs(50 - Mathf.RoundToInt((transform.position.y - 1) * 15)), 1, 150);
        _sprite.sortingOrder = layer;
        // print("dang chay ham update");
        if (isDead) return;
        if (target == null) // Nếu không có mục tiêu để tấn công 
        {
            allowMove = false;
            isAttacking = false;
            if (gameObject.transform.position == positionFlag) // khi dang o vi tri dat Flag
            {
                if (!isAttacking) // neu dang ko tan cong
                {
                    _anim.SetInteger("Change", 0);
                }

                allowMove = true;
            }
            if (transform.position != pos_origin && isKnight)
            {
                transform.position = Vector2.MoveTowards(transform.position, pos_origin, Time.deltaTime * moveSpeed);
            }
            else if(transform.position == pos_origin && isKnight)
            RegendHeal();
        }

        else // khi thay muc tieu moi di chuyen
        {
            allowMove = true;
            if (Vector2.Distance(transform.position, target.transform.position) > 1f) // neu enemy ra khoi tam cua Knight thi cho no = null
            {
                target = null;
                return; // moi them vao 4/5/2017
            }
            CheckFlipWith(target);// check quay mat theo huong enemy

            if (Vector3.Distance(transform.position, target.transform.position) > 1f) return;
            if (transform.localScale.x == 1)// neu huong mat ben phai
            {
                if (transform.position != target.transform.position + new Vector3(-0.2f, 0, 0) && allowMove) // Nếu vị trí Knight != vị trí Enemy
                {
                    transform.position = Vector2.MoveTowards(transform.position, target.transform.position + new Vector3(-0.2f, 0, 0), Time.deltaTime * moveSpeed);
                    _anim.SetInteger("Change", 1);
                    if (Mathf.Abs(transform.position.x - target.transform.position.x) <= 0.25f && Mathf.Abs(transform.position.y - target.transform.position.y) <= 0.15f)
                    {
                        AttackEnemy();
                    }
                }
                else
                {
                    // isAttacking = false;
                    AttackEnemy();
                }
            }
            else if (transform.localScale.x == -1)// neu huong mat ben trai
            {
                if (transform.position != target.transform.position + new Vector3(0.2f, 0, 0) && allowMove)
                {
                    transform.position = Vector2.MoveTowards(transform.position, target.transform.position + new Vector3(0.2f, 0, 0), Time.deltaTime * moveSpeed);
                    _anim.SetInteger("Change", 1);
                    if (Mathf.Abs(transform.position.x - target.transform.position.x) <= 0.25f && Mathf.Abs(transform.position.y - target.transform.position.y) <= 0.15f)
                    {
                        AttackEnemy();
                    }
                }
                else
                {
                    // isAttacking = false;
                    AttackEnemy();
                }
            }
        }
    }
    private void AttackEnemy()
    {
        // allowMove = false;
        if (isAttacking == false)
        {
            SoundTower._instance.KnightFight();
            //  print("dang chay ham tan cong");
            isAttacking = true;
            Invoke("setISAttackingfalse", coldown);
            if (_target.StartAttack == false)
            {
                _target.StartAttack = true;
            }
            if (isKnight)
            {
                int _random = Random.Range(0, 2);
                if (_random == 0)
                    _anim.SetInteger("Change", 2);
                else
                    _anim.SetInteger("Change", 4);
            }
            else if (isHero)
            {
                int _random = Random.Range(0, 10);
                if (_random == 2)
                {
                    if (currentHeal < maxHeal )
                    {
                        _anim.SetInteger("Change", 6);
                        currentHeal += 50;              
                        if (currentHeal >= maxHeal) { currentHeal = maxHeal; }
                        slider.transform.localScale = new Vector3((1 / maxHeal) * currentHeal, transform.localScale.y, transform.localScale.z);
                        Invoke("AllowHealing", 10f);
                    }
                }
            }
            else
            {
                _anim.SetInteger("Change", 2);
            }
        }
    }
    public void GiveDame()
    {
        // print("Da goi den ham GiveDame");

        if (_target != null)
        {
            _target.TakeDamePhysic(dame);
            if (_target.currentHeal <= 0)
            {
                target = null;
                _target = null;
            }
        }

    }
    public void TakeDame(int _dame)
    {
        if (currentHeal > 0)
        {
            if (!isHero)
            {
                int trueDame = _dame - armor;
                currentHeal -= trueDame;// tru mau
                if (currentHeal > 0)
                    slider.transform.localScale = new Vector3((1 / maxHeal) * currentHeal, transform.localScale.y, transform.localScale.z);// thay doi thanh mau
                else
                    Die();
            }
            else
            {
                int _ran = Random.Range(0, 10);
                if (_ran != 5)
                {
                    int trueDame = _dame - armor;
                    currentHeal -= trueDame;// tru mau
                    if (currentHeal > 0)
                        slider.transform.localScale = new Vector3((1 / maxHeal) * currentHeal, transform.localScale.y, transform.localScale.z);// thay doi thanh mau
                    else
                        Die();
                }
                else
                {
                    isAttacking = true;
                    Invoke("setISAttackingfalse", 1f);
                    _anim.SetInteger("Change", 5);
                }
            }
        }
        else
            Die();

    }
    private void RegendHeal()
    {
        timeregend += Time.deltaTime;
        if (timeregend >= 3) // hồi máu sau 3s
        {
            if (currentHeal > 0 && currentHeal < maxHeal)
            {
                currentHeal += Time.deltaTime * 4;
                slider.transform.localScale = new Vector3((1 / maxHeal) * currentHeal, 1, 1);
            }
            else if (currentHeal >= maxHeal)
            {
                currentHeal = maxHeal;
                slider.transform.localScale = new Vector3(1, 1, 1);
                timeregend = 0f;
            }
        }
    }
    private void Die()
    {
        _anim.SetInteger("Change", 3); // chay Animation Die
        isDead = true;
        Destroy(gameObject, 1f);
    }
    private void RemoveNull()
    {
        for (int i = 0; i < ListEnemy.Count; i++)
        {
            if (ListEnemy[i] == null)
                ListEnemy.Remove(ListEnemy[i]);
        }
    }
}
