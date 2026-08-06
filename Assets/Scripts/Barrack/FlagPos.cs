using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPos : MonoBehaviour {

    private GameObject barrack;
    private Vector3 neareastPath;
	void Start ()
    {
        barrack = transform.parent.gameObject;
        GameObject [] Path = GameObject.FindGameObjectsWithTag("Path");
        float nearest = 100f;
        for(int i = 0; i < Path.Length; i++)
        {
           // print("da chay vong lap");
            float distance = Vector3.Distance(barrack.transform.position, Path[i].transform.position);
           // print("Distance=" + distance);
            if (distance < nearest)
            {
                nearest=distance;
                neareastPath = Path[i].transform.position;
            }            
        }
       // print("childcount=" + Path.transform.childCount);
        transform.position = neareastPath;
	}
}
