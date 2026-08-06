using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    public GameObject _cam;
    // Use this for initialization
    void Update()
    {
        transform.position = new Vector2(_cam.transform.position.x, _cam.transform.position.y);
    }
}
