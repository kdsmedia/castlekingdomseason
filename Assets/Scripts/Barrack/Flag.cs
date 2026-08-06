using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

    private GameObject Zone;
	void Start ()
    {
        Zone = transform.parent.transform.parent.transform.Find("Zone").gameObject;
	}
	
    void OnMouseDown()
    {
        if (Zone.activeSelf == false)
        {
            Zone.SetActive(true);
        }
    }

}
