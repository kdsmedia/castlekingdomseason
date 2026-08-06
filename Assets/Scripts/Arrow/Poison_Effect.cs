using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison_Effect : MonoBehaviour {
    [Header("Dame in 3s")]
    public int Dame;
    private float _timer;
    public Color _color; // màu hiệu ứng rút máu
	void Start ()
    {
        StartCoroutine(PoisonDame());
        transform.parent.GetComponent<SpriteRenderer>().color = _color;
	}
    IEnumerator PoisonDame()// thực hiện việc rút máu
    {
        yield return new WaitForSeconds(1f);
        GiveDame();
        yield return new WaitForSeconds(1f);
        GiveDame();
        yield return new WaitForSeconds(1f);
        GiveDame();
        transform.parent.GetComponent<SpriteRenderer>().color = Color.white;
        Destroy(gameObject);
    }
    private void GiveDame()
    {
        if (transform.parent.GetComponent<EnemyController>() != null)
        {
            transform.parent.GetComponent<EnemyController>().TakeDamePhysic(Dame / 3);
        }
        else if(transform.parent.GetComponent<FlyController>() != null)
        {
            transform.parent.GetComponent<FlyController>().TakeDamePhysic(Dame / 3);
        }
    }
}
