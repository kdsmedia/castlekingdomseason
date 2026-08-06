using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneClick : MonoBehaviour {

    public GameObject Point;// điểm đặt cowf Flag mới
    public Transform flag;
    private List<KnightController> _knights = new List<KnightController>();
	void Start ()
    {
        transform.GetComponent<SpriteRenderer>().color = Color.blue;
        Point = transform.GetChild(0).gameObject;
     //   InvokeRepeating("SlowUpdate", 0, 0.2f);
        if (transform.parent.gameObject.GetComponent<Barrack>() != null)
            flag = transform.parent.gameObject.GetComponent<Barrack>().Flag;
        else if (transform.parent.gameObject.GetComponent<Mage_Golem>() != null)
            flag = transform.parent.gameObject.GetComponent<Mage_Golem>().Flag;

    }
	
	// Update is called once per frame

    void OnMouseDown()
    {
        Point.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        flag.transform.position = Point.transform.position;
        Instantiate(Resources.Load("Flag"), Point.transform.position, Quaternion.identity);
        for(int i = 0; i < transform.parent.childCount;i++)
        {
            if (transform.parent.GetChild(i).GetComponent<KnightController>() != null)
            {
                _knights.Add(transform.parent.GetChild(i).GetComponent<KnightController>());
            }
        }
        foreach (KnightController _knight in _knights)
        {
            if (_knight.target != null)
            {
                _knight.target.GetComponent<EnemyController>()._KnightToFight = null;
                _knight.target = null;
                _knight.allowMove = false;
                _knight.MustReturn = true;
            }
        }  
        gameObject.SetActive(false);
    }
}
