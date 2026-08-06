using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon_Anim : MonoBehaviour {
    public GameObject bullet;
    public void TurnOnBullet()
    {
        bullet.SetActive(true);
    }
}
