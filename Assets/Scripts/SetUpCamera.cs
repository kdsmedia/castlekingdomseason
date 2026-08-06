using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUpCamera : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
