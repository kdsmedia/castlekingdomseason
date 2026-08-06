using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disactive : MonoBehaviour {

    private bool wasclick;
	void Start ()
    {
        wasclick = true;
        TurnOff();
        InvokeRepeating("Update1s",0f,0.5f);
	}
	
	// Update is called once per frame
    private void Update1s()
    {
        TurnOff();
    }
    void TurnOff()
    {
        if (transform.gameObject.activeSelf)
        {
            if (wasclick == true)
            {
                wasclick = false;
                Invoke("Off", 1f);
            }
        }
    }
    void Off()
    {
        wasclick = true;
        gameObject.SetActive(false);           
    }
}
