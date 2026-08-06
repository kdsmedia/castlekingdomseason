using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage_Golem : MonoBehaviour {

    public GameObject Golem_Pref;
    public Transform Flag;
    private GameObject Golem1=null;
    private Golem _golem=null;
    public int levelgolem;
    public int timeRespawn;
    //private Vector3 offset1;

    public float TimeToMove;
    void Start ()
    {
       // offset1 = new Vector3(-0.2f, 0, 0);
        Flag = this.transform.gameObject.transform.Find("Flag");
        InvokeRepeating("UpdateLate", 0, 0.5f);
        SpawnFirstTime();
    }
    void UpdateLate()
    {
        SetPosFlag();
    }
    private void SetPosFlag()
    {
        if (_golem != null)
            _golem.positionFlag = Flag.transform.position ;
        else
        {
           Invoke("SpawnFirstTime", timeRespawn);
        }
    }
    private void SpawnFirstTime()
    {
        if (Golem1 == null && !transform.Find("Knight1"))
        {
            Golem1 = Instantiate(Golem_Pref, Flag.position, Quaternion.identity) as GameObject;
            Golem1.name = "Knight1";
            Golem1.transform.parent = this.transform;
            _golem = Golem1.GetComponent<Golem>();
        }
    }
    void Update ()
    {
        if (_golem != null)
        {
            if (Golem1.activeSelf && _golem.target == null) // Nếu Knight được sinh ra và được phép đk thì di chuyển nó về phía Flag
            {
              //  print("dang chuan bi goi den ham di chuyen");
                if (Golem1.transform.position != (Flag.transform.position )) // Nếu ko ở vị trí của Flag
                {
                  //  print("dang goi den ham di chuyen");
                    Golem1.transform.position = Vector3.MoveTowards(Golem1.transform.position, Flag.transform.position , Time.deltaTime / TimeToMove);
                    _golem._anim.SetInteger("Change", 1);
                    _golem.CheckFlipWith(Flag.gameObject);
                }
            }
        }
    }
}
