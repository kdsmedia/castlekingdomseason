using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelePortEnemy : MonoBehaviour
{
    [System.Serializable]
    public class Gate
    {
        public Transform gate;
        public GameObject Path;
    }
    public Gate[] _gate;
    // Use this for initialization
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            if (col.gameObject.GetComponent<EnemyController>() != null)
            {
                col.gameObject.SetActive(false);
                int _random = Random.Range(0, _gate.Length);
                col.transform.position = _gate[_random].gate.position;
                col.transform.GetComponent<EnemyController>().Path = _gate[_random].Path;
                col.transform.GetComponent<EnemyController>().FindPath();
                StartCoroutine(ActiveAgain(col.gameObject));
            }
        }
    }
    private IEnumerator ActiveAgain(GameObject enemy)
    {
      //  print("da goi ham active Again");
        yield return new WaitForSeconds(2f);
        enemy.SetActive(true);
    }
}
