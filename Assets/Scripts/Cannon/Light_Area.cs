using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Area : MonoBehaviour {

    private GameObject target;
    private Vector3 offset;
    bool setactive = true;
    private Vector3 pre_pos;
	void Start ()
    {
        target = transform.parent.gameObject;
        offset = transform.position - target.transform.position;
        pre_pos = transform.position;
        
	}
	
    private void MoveToTarget()
    {
        if (setactive == true)
        {
            transform.position = pre_pos;
            setactive = false;
            Invoke("Disactive", 0.5f);
        }
        transform.Translate(offset.normalized * Time.deltaTime * 0.2f);
        Vector3 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x);
        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.back);

    }
	void Update ()
    {
        MoveToTarget();
	}
    private void Disactive()
    {
        setactive = true;
        transform.gameObject.SetActive(false);
    }
}
