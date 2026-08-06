using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public GameObject Target;
    public int dame { get; set; }
    private float size;
	SpriteRenderer sprite;
    void Awake()
    {
		sprite = GetComponent<SpriteRenderer> ();
        size = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
		sprite.enabled = false;
    }
    void Start()
    {
		if (Target == null) {
			Destroy (gameObject);
		}
		else 
		{
			ScaleObj();
			GiveDame();
			onDestroy();
			sprite.enabled = true;
		}

    }
    private void ScaleObj()
    {

        float distance = Vector2.Distance(transform.position, Target.transform.position);
        transform.localScale = new Vector3(distance / size, distance / size, 1);
    }
    private void GiveDame()
    {
        if (Target.GetComponent<EnemyController>() != null)
        {
            Target.GetComponent<EnemyController>().TakeDamageMagic(dame);
        }
        else if (Target.GetComponent<FlyController>() != null)
            Target.GetComponent<FlyController>().TakeDamageMagic(dame);
    }
    void onDestroy()
    {
        Destroy(gameObject, 1f);
    }
    void Update()
    {
        if (Target == null) 
		{ 
			Destroy(gameObject); return;
		}
        else
        {
            ScaleObj();
            Vector3 dir = Target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
