using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private void OnMouseDown()
    {
        SoundTower._instance.Click();
        Manager._instance.ShowInterface(this);
    }
}
