using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    public GameObject target { get; set; }
    public int dame { get; set; }
    public float movespeed;
    private Animator _anim;
    private bool isrotate=false;
    private bool move_switch = false;
    public GameObject effect;

    void Start()
    {
        _anim = GetComponent<Animator>();
        Invoke("movetotarget", 1.5f);
    }
    void Update()
    {
        if (move_switch == false)
        {
            MoveToSky();
        }
        else
            movetotarget();

        if (isrotate == false)
        {
            isrotate = true;
            Invoke("Rotation", 0.5f);
        }
        
    }
    private void MoveToSky()
    {
        transform.Translate(Vector3.up* Time.deltaTime * movespeed);
        print("Move to sky dang chay");
    }
    private void Rotation()
    {
        if (transform.position.x <= target.transform.position.x)
            _anim.SetTrigger("left");
        else
            _anim.SetTrigger("right");
    }
    private void movetotarget()
    {
        _anim.enabled = false;
        if (move_switch == false)
        {
            move_switch = true;
            transform.GetChild(0).transform.localRotation = Quaternion.AngleAxis(90, Vector3.back);
        }
        if (target == null) { Destroy(gameObject); }
        if (transform.position != target.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * movespeed);
            Vector3 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == target)
        {
            if (target.GetComponent<EnemyController>() != null)
            {
                target.transform.GetComponent<EnemyController>().TakeDamePhysic(dame);
            }
            else if (target.GetComponent<FlyController>() != null)
            {
                target.transform.GetComponent<FlyController>().TakeDamePhysic(dame);
            }
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
