using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFire : MonoBehaviour {


    public Vector3 target;
    private Vector3 offset;
    public GameObject Effect;
    public int dame;
    public float speed;
	// Use this for initialization
	void Start ()
    {
        transform.rotation = Quaternion.Euler(0, 0, -90);
        offset = new Vector3(target.x + Random.Range(-0.2f, 0.2f), target.y + Random.Range(-0.2f, 0.2f), 0);
        if (PlayerPrefs.GetInt("Rainupgrade2") == 1)
        {
            dame += 5;
        }
        if (PlayerPrefs.GetInt("Rainupgrad52") == 1)
        {
            dame += 10;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position != offset)
        {
            //print("dang bay den ");
            //transform.Translate(Vector3.forward * Time.deltaTime,Space.World);
            transform.position = Vector3.MoveTowards(transform.position, offset, Time.deltaTime* speed);
        }
        else
        {
          GameObject effect=  Instantiate(Effect, transform.position, Quaternion.identity) as GameObject;
            effect.GetComponent<Explosion>().dame = dame;
            Destroy(gameObject);
        }
	}
}
