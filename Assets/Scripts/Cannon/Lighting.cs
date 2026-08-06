using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour
{

    public List<GameObject> enemies = new List<GameObject>();// danh sách các Enemy trong tầm đánh
    public List<GameObject> enemies2 = new List<GameObject>(); // là bản sao của List enemies
    public List<GameObject> enemyNearest = new List<GameObject>(); // Danh sách Enemy gần nhất
    private Animator _anim;
    public GameObject pos;// vị trí sinh ra tia sét
    public int Dame;
    public float cowdown;
    public bool isShooting = false;
    private GameObject target;
    private GameObject nearTarget;
    //private GameObject target;
    //----------More Light---------------
    public bool morelight = false;
    public int levelLight { get; set; }
    public int numberTime = 0; // so lan nhay cua Lighting ==0 la ko nhay ==1 la nhay sang 1 muc tieu
    private int number = 0;
    private GameObject Interface = null;
    //--------------------AREAN LIGHTING DAME------------
    public GameObject[] effects;
    public bool isAreaLight;
    public int levelArea { get; set; }
    public int Area_dame1, Area_dame2, Area_dame3;
    public int Area_Dame { get; set; }

    void Start()
    {
        _anim = GetComponent<Animator>();
        int layer = Mathf.Clamp(Mathf.Abs(50 - Mathf.RoundToInt((transform.position.y - 1) * 15)), 1, 150);// đặt layer theo trục Y
        GetComponent<SpriteRenderer>().sortingOrder = layer;
        SoundTower._instance.CannonTower();
    }
    // Update is called once per frame
    void Update()
    {
        RemoveNull();
        if (enemies.Count > 0)
        {
            if (isShooting == false)
            {
                isShooting = true;
                Invoke("SetShootingFalse", cowdown);
                //  Invoke("Shoot", 0.5f);
                _anim.SetTrigger("Fire");
            }
        }
    }
    #region tim Enemy gan nhat
    public void FindNearEnemy()// Add các Enemy gần nhất vào trong List NearestEnemies
    {

        for(int j = 0; j < enemies2.Count; j++)
        {
            enemies2.RemoveAt(j);
        }
        // sao chep List enemies vao List enemies2
        for(int i = 0; i < enemies.Count;i++)
        {
            if (!enemies2.Contains(enemies[i]))
            {
                enemies2.Add(enemies[i]);
            }
        }
        target = enemies2[0];// gán mục tiêu là Enemy đầu tiên trong List
        nearTarget = target;
        enemies2.RemoveAt(0);// XÓa Enemy đc chọn ra khỏi List
        enemyNearest.Add(target);//
        if (levelLight == 1) { numberTime = 3; } // =3 la nhay 4 muc tieu
        else if (levelLight == 2) { numberTime = 4; } // =4 la nhay 5 muc tieu

        if (numberTime > 0)
        {
            for (int i = 1; i < numberTime; i++)
            {
                number = 100;
                FindInEnemies2();// tìm Enemy gần nhất ở trong List Enemies2
                if (enemies2.Count > 0)
                {
                    if (!enemyNearest.Contains(nearTarget))// thêm Enemy gần nhất trong List Enemies2 vào List EnemyNearest
                    {
                        enemyNearest.Add(nearTarget);
                        enemies2.Remove(nearTarget); // Xoa enemy gan nhat voi target ra khoi List 2
                    }
                    else
                    {
                        break;
                    }

                }
            }
        }
       // print("SO luong tia set la: =" + enemyNearest.Count);
    }
    private void FindInEnemies2()// tìm Enemy gần với trụ nhất trong List Enemies2
    {
        RemoveNull();
        float nearestDistance = 1f;
        if (enemies2.Count > 0)// Tìm enemy gần nhất ở trong List Enemies2
        {
            for (int i = 0; i < enemies2.Count; i++)
            {
                // float distance = Vector3.Distance(enemies[0].transform.position, enemies2[i].transform.position);
                float distance = Vector2.Distance(nearTarget.transform.position, enemies2[i].transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    number = i;                  
                }
            }
            if (number != 100)
            {
                nearTarget = enemies2[number];
            }
        }
    }
    #endregion
    public void AddEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        if (enemy != null)
        {
            enemies.Remove(enemy);
        }
    }
    private void RemoveNull()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
            }
        }
        for (int i = 0; i < enemies2.Count; i++)
        {
            if (enemies2[i] == null)
            {
                enemies2.RemoveAt(i);
            }
        }
        for (int i = 0; i < enemyNearest.Count; i++)
        {
            if (enemyNearest[i] == null)
            {
                enemyNearest.RemoveAt(i);
            }
        }
    }
    public void Shoot()// THực hiện việc bắn laze
    {

      //  FindNearEnemy();
    }
    private void SetShootingFalse()// đặt lại giá trị isShooting =false
    {
        isShooting = false;
    }
    public void InStance_Light1()    // SInh ra các tia sét
    {
        SoundTower._instance.Light();
        InStance_Light2(target, pos); // sinh ra tia sét đầu tiên từ trụ đến mục tiêu đầu tiên
        //--------------------AREAN DAME----------------------
        if (isAreaLight)
        {
            if (levelArea == 1) { Area_Dame = Area_dame1; }
            else if (levelArea == 2) { Area_Dame = Area_dame2; }
            else if (levelArea == 3) { Area_Dame = Area_dame3; }
            AreaDame(Area_Dame);
        }
        if (enemyNearest.Count != 0)
        {
            for (int i = 1; i < enemyNearest.Count; i++)// sinh ra tia sét đến các mục tiêu khác
            {
                if (enemyNearest[i] != null)
                {
                    InStance_Light2(enemyNearest[i], enemyNearest[i - 1]);
                }
            }
        }
    }
    private void InStance_Light2(GameObject target, GameObject pos)// sinh ra tia sét tại 1 vị trí nào đó và hướng về phía mục tiêu
    {
        GameObject _Light = Instantiate(Resources.Load("Cannon/Light"), pos.transform.position, Quaternion.identity) as GameObject;
        Light _light = _Light.GetComponent<Light>();
        _light.Target = target;
        _light.dame = Dame;
    }
    //---------------------------AREA DAME---------------------------------------
    private void AreaDame(int dame) // sinh ra dame diện rộng
    {
        Effect_AreaLight();
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (Collider2D col in coll)
        {
            if (col.gameObject.tag == "Enemy")
            {
                col.gameObject.GetComponent<EnemyController>().TakeDamePhysic(dame);
            }
        }
    }
    private void Effect_AreaLight()// bật hiệu ứng sinh dame diện rộng
    {
        foreach (GameObject effect in effects)
        {
            effect.SetActive(true);
        }
    }
    void OnMouseDown()
    {
        if (Interface == null)
        {
            Interface = Instantiate(Resources.Load("Interface/CannonInterface4"), transform.position, Quaternion.identity) as GameObject;
            Interface.transform.parent = this.transform;

        }
        else
        {
            if (Interface.activeSelf == false)
            {
                Interface.SetActive(true);
            }
            else
            {
                Interface.SetActive(false);
            }
        }
    }
}
