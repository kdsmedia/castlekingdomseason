using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom_Explosion : MonoBehaviour
{

    public GameObject[] booms;
    public float speed;
    public float radius;
    public GameObject effect;
    public int dame;
	void Start ()
    {
        Invoke("onDestroy", 1f);
	}
	
	// Update is called once per frame
	void Update ()
    {
		foreach(GameObject boom in booms)
        {
            Boom_move(boom);
        }
	}
    private void Boom_move(GameObject boom)
    {
        if (boom == null) return;
        Vector3 dir = boom.transform.position - transform.position;
        boom.transform.Translate(dir.normalized * Time.deltaTime * speed);
        if(Vector3.Distance(transform.position,boom.transform.position) >= radius)
        {
           GameObject effection= Instantiate(effect,boom.transform.position,Quaternion.identity) as GameObject;
            effection.GetComponent<Explosion>().dame = dame;
            Destroy(boom);
        }
    }
    private void onDestroy()
    {
        Destroy(gameObject);
    }
}
