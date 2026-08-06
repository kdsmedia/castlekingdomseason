using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Circle_Spawn : MonoBehaviour
{
    public bool Right, Left, Up, Down;
    public GameObject Direction;
    private Vector3 pos_text;
    private GameObject goldtext;
    void Awake()
    {
        goldtext = GameObject.Find("UI").transform.GetChild(14).gameObject;
        if (Right == true)
        {
            Direction = GameObject.Find("UI").transform.GetChild(12).gameObject;
            pos_text = transform.position + new Vector3(0, 0.5f, 0);
        }
        else if (Left == true)
        {
            Direction = GameObject.Find("UI").transform.GetChild(13).gameObject;
            pos_text = transform.position + new Vector3(0, 0.5f, 0);
        }
        else if (Up == true)
        {
            Direction = GameObject.Find("UI").transform.GetChild(10).gameObject;
            pos_text = transform.position + new Vector3(0, -0.5f, 0);
        }
        else if (Down == true)
        {
            Direction = GameObject.Find("UI").transform.GetChild(11).gameObject;
            pos_text = transform.position + new Vector3(0, 0.5f, 0);
        }
    }
    public void Active_Direction()
    {
        Direction.SetActive(true);
    }
    public void Disactive_Direction()
    {
        Direction.SetActive(false);
    }
    public void Spawn_Now()
    {
        Direction.SetActive(false);
        if (WaveSpawnManager._instance.firstTime == false)
        {
            goldtext.transform.position = pos_text;
            goldtext.SetActive(true);
            float goldnumber = Mathf.Abs((WaveSpawnManager._instance.countToNextWave - WaveSpawnManager._instance._time)) * 15f;
            goldtext.transform.GetChild(0).GetComponent<Text>().text ="+ "+ Mathf.RoundToInt(goldnumber).ToString();
            Manager.money +=Mathf.RoundToInt(goldnumber);
            Manager._instance.setcoin();
        }
        WaveSpawnManager._instance.SpawnNow();
    }
    void Update()
    {
        float distance = Vector2.Distance(transform.position, Direction.transform.position);      
      //  Debug.Log("Distace=" + distance);
        if (distance <= 0.5f)
        {
            if(Direction.activeSelf==true)
            Direction.SetActive(false);
        }
    }
}
