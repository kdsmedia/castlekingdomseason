using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wingame : MonoBehaviour {
    public GameObject Star;
    public int number;
	// Use this for initialization
	void Start ()
    {
		for(int i = 0; i < number; i++)
        {
            Instantiate(Star, transform.position, Quaternion.identity);
        }
	}
}
