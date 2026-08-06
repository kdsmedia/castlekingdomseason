using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CLick_to_move : MonoBehaviour {

  //  public GameObject FireRain;
   // public float numberRain;
    private bool move = true;
    public bool active = false;
    private Hero _hero;
    private SpriteRenderer _sprite;
    private Animator _anim;
    public GameObject X;
    private void Start()
    {
        _hero = FindObjectOfType<Hero>();
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.enabled = false;
        _anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (move == false) return;
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);
            GameObject.Find("Canvas_Info").GetComponent<Canvas>().enabled=false;
            GameObject.Find("Canvas_Info").GetComponent<Canvas>().transform.GetChild(4).gameObject.SetActive(false);
            GameObject.Find("Canvas_Info").GetComponent<Canvas>().transform.GetChild(5).gameObject.SetActive(false);
        }
        if (Input.GetMouseButtonUp(0) && active)
        {
            move = false;
            _hero.positionFlag = transform.position;

            if (_hero.target != null)
            {
                _hero.target.GetComponent<EnemyController>()._KnightToFight = null;
                _hero.target = null;
                _hero.MustReturn = true;
                _hero.allowMove = false;
            }
            _sprite.enabled = true;
            _anim.enabled = true;
            Destroy(gameObject, 0.5f);
        }
       else if (Input.GetMouseButtonUp(0) && !active)
        {
            X.SetActive(true);
            Invoke("Disactive", 0.2f);
        }
    }
    void Disactive()
    {
        X.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "LimitSpell")
        {
            active = true;
        }
    }

}
