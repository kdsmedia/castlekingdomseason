using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject ball_effect;
    public GameObject target;
    public int dame;
    private Vector3 latepos;
    public float Speed = 1.5f;
    void Start()
    {
        SoundTower._instance.MageBall();
        if (target == null) { Destroy(gameObject); }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        else
            latepos = target.transform.position;
        if (transform.position != latepos)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * Speed);
            Vector3 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == target)
        {
            if (col.gameObject.GetComponent<EnemyController>() != null)
                col.gameObject.GetComponent<EnemyController>().TakeDamageMagic(dame);
            else if (col.gameObject.GetComponent<FlyController>() != null)
                col.gameObject.GetComponent<FlyController>().TakeDamageMagic(dame);
            GameObject ball = Instantiate(ball_effect, target.transform.position, Quaternion.identity) as GameObject;
            ball.transform.parent = target.transform;
            Destroy(gameObject, 0.1f);
        }
    }
}
