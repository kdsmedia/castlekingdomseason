using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    public int dame;
    public List<GameObject> enemies = new List<GameObject>();
    public bool explosion1, explosion2;
    Animator _anim;
    void Start()
    {
        SoundTower._instance.CannonExplosion();
        _anim = GetComponent<Animator>();
        if (explosion2) { _anim.SetTrigger("switch"); }
        Invoke("GiveDame", 0.5f);
        Invoke("onDestroy", 1f);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
        //    print("da va cham vao enemy");
            if (!enemies.Contains(col.gameObject))
            {
                enemies.Add(col.gameObject);
            }
        }
    }
    void GiveDame()
    {
        foreach(GameObject enemy in enemies)
        {
            if (enemy != null)
            {
               enemy.GetComponent<EnemyController>().TakeDamePhysic(dame);
            }
        }
    }
    public void onDestroy()
    {
        Destroy(gameObject);
    }
}
