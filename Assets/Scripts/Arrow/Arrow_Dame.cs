using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Dame : MonoBehaviour
{
    public bool isPoison; // có phải là tên độc hay không
    public int damepoison { get; set; } // dame của tên độc
    public GameObject poisonEffect; // hiệu ứng dính độc
    public int dame; // lấy giá trị từ obj cha là parabol
    private GameObject target;
    void Start()
    {
        target = transform.parent.GetComponent<Parabol>().target;
        dame = transform.parent.GetComponent<Parabol>().Dame;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == target.gameObject)
        {
            #region Gây dame cho mục tiêu
            if (col.gameObject.GetComponent<EnemyController>() != null)
            {
                col.gameObject.GetComponent<EnemyController>().TakeDamePhysic(dame);
                if (col.gameObject.GetComponent<EnemyController>().currentHeal > 0)
                {
                    Destroy(transform.parent.gameObject);
                }
                else if(col.gameObject.GetComponent<EnemyController>().currentHeal<=0)
                {
                    Destroy(transform.parent.gameObject,0.5f);
                }
            }
            else if (col.gameObject.GetComponent<KnightController>() != null)
            {
                col.gameObject.GetComponent<KnightController>().TakeDame(dame);
                 Destroy(transform.parent.gameObject);    
            }
            else if (col.gameObject.GetComponent<Golem>() != null)
            {
                col.gameObject.GetComponent<Golem>().TakeDame(dame);
                Destroy(transform.parent.gameObject);
            }
            else if (col.gameObject.GetComponent<FlyController>() != null)
            {
                col.gameObject.GetComponent<FlyController>().TakeDamePhysic(dame);
                if (col.gameObject.GetComponent<FlyController>().currentHeal > 0)
                {
                    Destroy(transform.parent.gameObject);
                }
                else
                {
                    Destroy(transform.parent.gameObject, 0.5f);
                }
            }
            else if (col.gameObject.GetComponent<Hero>() != null)
            {
                col.gameObject.GetComponent<Hero>().TakeDame(dame);
                Destroy(transform.parent.gameObject);
            }
            #endregion
            GetComponent<Collider2D>().enabled = false;
            // Nếu là tên độc thì thực hiện rút máu theo thời gian và sinh hiệu ứng
            if (isPoison)
            {
                GameObject poison = Instantiate(poisonEffect, col.gameObject.transform.position, Quaternion.identity) as GameObject;
                poison.GetComponent<Poison_Effect>().Dame = damepoison;
                poison.transform.parent = col.gameObject.transform;
            }
           // Destroy(transform.parent.gameObject);
        }
    }

}
