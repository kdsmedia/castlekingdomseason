using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyController : MonoBehaviour
{

    //------ cac thuoc tinh cơ bản của Enemy
    public float maxHeal; // máu tối đa
    public float currentHeal; // máu hiện tại
    public float movespeed;// tốc độ bay
    public int Armor;// giáp
    public int ResistanMagic;// kháng phép
    private Transform slider; // Thanh máu
    private Animator _anim;// Animation
    private bool isDead = false;

    public GameObject Path;// obj chứa Path
    private List<GameObject> Paths = new List<GameObject>(); // Danh sách các path có trên bản đồ
    private List<Vector3> Paths_Pos = new List<Vector3>(); // vị trí của các path trong DS paths
    private Vector3 offset; // độ dịch với vị trí path
    private Vector3 PointToMove; // vị trí path gần nhất để đi tới
    private SpriteRenderer _sprite;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        currentHeal = maxHeal;
        slider = transform.Find("HealBar").transform.Find("slider").transform;
        _anim = GetComponent<Animator>();
        _anim.SetInteger("Change", 0);// trang thai mac dinh dau tien la chay
        FindPath();
    }
    private void FindPath()    // Tìm vị trí path trên bản đồ 
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
    private void MoveToPath()// thực hiện việc di chuyển đến path gần nhất
    {
        float neareset_path = 100f;
        for (int i = 0; i < Paths_Pos.Count; i++) // chay xong vong lap nay se tim duoc path ngan nhat
        {
            float distance = Vector3.Distance(transform.position, Paths_Pos[i]);
            if (distance <= neareset_path)
            {
                neareset_path = distance;
                PointToMove = Paths_Pos[i]; //toa do path ngan nhat
            }
        }
        //-- di chuyen den path
        if (transform.position != PointToMove) // neu khac toa do voi path
        {
            CheckFlipWith(PointToMove); // check Flip voi diem Path
            transform.position = Vector3.MoveTowards(transform.position, PointToMove, Time.deltaTime * movespeed);// di chuyen den path
            if (transform.position == Paths_Pos[Paths_Pos.Count - 1]) // Nếu di chuyển đến path cuối cùng thì sẽ trừ máu Player
            {
                Manager._instance.TakeHeart();
                Destroy(gameObject);
            }
            if (transform.position.y >= PointToMove.y && Mathf.Abs(transform.position.x - PointToMove.x) <= 0.2f) // Chạy xuống
                _anim.SetInteger("Change", 2);
            else if (transform.position.y < PointToMove.y && Mathf.Abs(transform.position.x - PointToMove.x) <= 0.2f) // Chạy lên
                _anim.SetInteger("Change", 1);
            else
                _anim.SetInteger("Change", 0); // Trạng thái chạy ngang
        }
        else
        {
            Paths_Pos.Remove(PointToMove);
        }
    }
    private void CheckFlipWith(Vector3 target) // check quay trái/ phải với path
    {
        if (transform.position.x <= target.x)
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
    public void TakeDamePhysic(int _dame) // nhận dame từ sát thương vật lý
    {
        int truedame = _dame - Armor; // Dame sau khi da tru giap va khang phep
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
    public void TakeDamageMagic(int _dame)// nhận dame từ sát thương phép
    {
        int truedame = _dame - ResistanMagic; // Dame sau khi da tru giap va khang phep
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
        _anim.SetInteger("Change", 3); // ANimation die   
        isDead = true;
        Destroy(gameObject, 1f);
    }
    // Update is called once per frame
    void Update()
    {
        if (Manager.isFinishing == true) { _anim.enabled = false; return; }
        // đặt layer theo tọa độ Y
        int layer = Mathf.Clamp(Mathf.Abs(50 - Mathf.RoundToInt((transform.position.y - 1) * 15)), 1, 150);
        _sprite.sortingOrder = layer;
        if (isDead) { return; }
            MoveToPath();
    }
}
