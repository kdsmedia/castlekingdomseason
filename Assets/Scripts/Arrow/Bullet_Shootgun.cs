using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Shootgun : MonoBehaviour
{

    public GameObject enemy { get; set; }
    public float speed;
    public int dame { get; set; }
    public GameObject[] Parts; // các viên đạn nhỏ của shotgun
    public GameObject effect;// hiệu ứng nổ của các viên đạn nhỏ
    private Vector3 enemypos; // vị trí của Enemy
    void Start()
    {
        if (enemy != null)
            enemypos = enemy.transform.position;
        else
            Destroy(gameObject);

    }
    void Update()
    {
        if (enemy != null) // di chuyển tới vị trí của  Enemy theo thời gian
        {
            transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, Time.deltaTime * speed);
            Vector3 dir = enemy.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.left);
        }
        else // di chuyển đến vị trí cuối cùng của enemy
        {
            if (enemypos != Vector3.zero)
                transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, Time.deltaTime * speed);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy" || col.tag=="Fly")
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f); // tìm tất cả các Enemy trong bán kính vụ nổ
            //sinh hiệu ứng nổ cho các viên đạn con
            foreach(GameObject onepart in Parts)
            {
                Instantiate(effect, onepart.transform.position, Quaternion.identity);
            }
            foreach(Collider2D colli in colliders) // tính Dame cho các enemy trong bán kính vụ nổ
            {
                if (colli.gameObject.GetComponent<EnemyController>() != null)
                {
                    colli.gameObject.GetComponent<EnemyController>().TakeDamePhysic(dame);
                }
                else if(colli.gameObject.GetComponent<FlyController>() != null)
                {
                    colli.gameObject.GetComponent<FlyController>().TakeDamePhysic(dame);
                }
            }
            Destroy(gameObject,0.2f);
        }
    }
}
