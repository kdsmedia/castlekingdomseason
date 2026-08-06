using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDialog : MonoBehaviour {


	void Start ()
    {
        Destroy(transform.parent.gameObject, 0.1f);
	}
	

}
