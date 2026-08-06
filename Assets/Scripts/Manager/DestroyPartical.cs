using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPartical : MonoBehaviour {
    public float timelife;
	// Use this for initialization
    void Awake()
    {
        InvokeRepeating("onDestroy", 1, 10f);
    }
	void Start ()
    {
        Invoke("onDestroy", timelife);

	}
    void onDestroy()
    {
       // print("da goi ham destroy");
        Destroy(this.gameObject);
    }
}
