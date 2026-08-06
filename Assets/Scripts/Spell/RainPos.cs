using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainPos : MonoBehaviour {

    // Use this for initialization

    public GameObject FireRain;
    public float numberRain;
    private bool move = true;
    public bool active = false;
    private void Start()
    {
        if (PlayerPrefs.GetInt("Rainupgrade1") == 1)
        {
            numberRain += 2;
        }
    }
	void Update ()
    {
         if (move == false) return;
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }     
        if(Input.GetMouseButtonUp(0) && active)
        {
                Spell_manager.countTime = true;
            FindObjectOfType<Spell_manager>().ChangeImage();
                move = false;
            // StartCoroutine(MakeRain());
            Rain();
            
        }
    }
    private void Rain()
    {
        StartCoroutine(MakeRain());
    }
    IEnumerator MakeRain()
    {
       // print("da goi ham Rain");
        for (int i = 1; i <= numberRain; i++)
        {
            float h = Camera.main.orthographicSize;
            Vector2 positionOfThis = new Vector2(transform.position.x, transform.position.y);
            Vector2 offset = new Vector2(0, h);
            GameObject rain = Instantiate(FireRain, positionOfThis + offset, Quaternion.identity) as GameObject;
            rain.GetComponent<RainFire>().target = transform.position;
          //  print("da tao xong obj");
            yield return new WaitForSeconds(0.3f);
        }
        Destroy(gameObject, 0.2f);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "LimitSpell")
        {
            active = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "LimitSpell")
        {
            active = false;
        }
    }
}
