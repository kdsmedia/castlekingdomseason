using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderRun : MonoBehaviour {
    public float speed;
    private float x;
    private new string name=null;
	// Use this for initialization
	void Start ()
    {
        x = 0;
        if (transform.parent.transform.parent.GetComponent<SpriteRenderer>().sprite.name == "BarrackBuild")
        {
            name = "Barrack";
        }
        else if (transform.parent.transform.parent.GetComponent<SpriteRenderer>().sprite.name == "ArcherBuild")
        {
            name = "Archer";
        }
        else if(transform.parent.transform.parent.GetComponent<SpriteRenderer>().sprite.name == "CannonBuild")
        {
            name = "Cannon";
        }
        else if(transform.parent.transform.parent.GetComponent<SpriteRenderer>().sprite.name == "MageBuild")
        {
            name = "Mage";
        }
        transform.parent.transform.parent.transform.parent.GetChild(0).gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (x >= 1f)
        {
            x = 1f;
            Manager._instance.ShowTower(name, transform.parent.transform.parent.transform.position, transform.parent.transform.parent.transform.parent.gameObject);
            Destroy(transform.parent.transform.parent.gameObject);

        }
        else
        {
            x += Time.deltaTime * (1/speed);
            transform.localScale = new Vector3(x, 1, 1);
        }
	}

}
