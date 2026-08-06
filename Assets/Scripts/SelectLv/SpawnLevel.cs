using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLevel : MonoBehaviour {

    public static int level;
    public Transform[] _pos_Hero;
    public GameObject[] HeroPref;
    public static SpawnLevel _instace;
    void Awake()
    {
        if (_instace == null)
        {
            _instace = this;
        }
    }
	void Start ()
    {
        InstanceLevel();
        InstanceHero(0);
    }	
    private void InstanceLevel()
    {
      Instantiate(Resources.Load("Level/Level" + level));       
    }
    public void InstanceHero(int time)
    {
        StartCoroutine(ChooseHero(time));
    }

    IEnumerator ChooseHero(int time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 1; i <= HeroPref.Length; i++)
        {
            if (PlayerPrefs.GetInt("Hero" + i) == 1)
            {
                SpawnHero(i);
                break;
            }
        }
    }
    private void SpawnHero(int number)
    {
        Instantiate(HeroPref[number - 1], _pos_Hero[level - 1].position, Quaternion.identity);
    }
}
