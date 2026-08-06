using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraDrag : MonoBehaviour
{

    public float dragSpeed = 0.5f;
    private Vector3 dragOrigin; // vị trí của chuột
    public GameObject map { get; set; }
    public GameObject[] maplist;
    public float SizeXmap, SizeYmap;
    public float h, w;
    public float distanceX, distanceY;
    Camera cam;
    private Vector3 oldPos, panOrigin;
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            map = maplist[SpawnLevel.level - 1];
            map.SetActive(true);
        }
        else
        {
            map = maplist[0];
            map.SetActive(true);
        }
        SizeXmap = map.GetComponent<SpriteRenderer>().sprite.bounds.size.x * 0.5f;
        SizeYmap = map.GetComponent<SpriteRenderer>().sprite.bounds.size.y * 0.5f;
        cam = Camera.main;
        h = cam.orthographicSize; // chiều cao của Camera
        w = h * cam.aspect;// chiều ngang của camera
        distanceX = Mathf.Abs(SizeXmap - w); // khoảng cách giữa camera và map theo chiều X
        distanceY = Mathf.Abs(SizeYmap - h);// khoảng cách giữa camera và map theo chiều Y
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, map.transform.position.x - distanceX, map.transform.position.x + distanceX), Mathf.Clamp(transform.position.y, map.transform.position.y - distanceY, map.transform.position.y + distanceY), -10f);//Move the position of the camera to simulate a drag, speed * 10 for screen to worldspace conversion
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //bDragging = true;
            oldPos = transform.position;
            panOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);                    //Get the ScreenVector the mouse clicked
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - panOrigin;    //Get the difference between where the mouse clicked and where it moved
            transform.position = oldPos + -pos * dragSpeed;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, map.transform.position.x - distanceX, map.transform.position.x + distanceX), Mathf.Clamp(transform.position.y, map.transform.position.y - distanceY, map.transform.position.y + distanceY), -10f);//Move the position of the camera to simulate a drag, speed * 10 for screen to worldspace conversion
        }

    }
}
