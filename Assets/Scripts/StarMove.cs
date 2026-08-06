using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMove : MonoBehaviour {
    private int random;
    // Use this for initialization
    Rigidbody2D _rigid;
	void Start ()
    {
        _rigid = GetComponent<Rigidbody2D>();
        int _random = Random.Range(0, 2);
        if (_random == 0) random = 1;
        else random = -1;

        _rigid.velocity = new Vector2(random * Random.Range(3,6), Random.Range(4, 8));
        Destroy(gameObject, 1f);
    }
}
