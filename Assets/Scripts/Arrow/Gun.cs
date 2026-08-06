using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public GameObject enemy { get; set; }
    public float speed;
    public int dame { get; set; }

	// Update is called once per frame
	void Update ()
    {
        FollowEnemy();
	}

    private void FollowEnemy() // Đuối theo Enemy
    {
        if (enemy != null) // Nếu Enemy vẫn còn sống thì sẽ đuổi theo
        {
            transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, Time.deltaTime * speed);
            Vector3 dir = enemy.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            // khi chạm vào enemy sẽ gây dame
            if(Mathf.Abs( transform.position.x-enemy.transform.position.x)<=0.1f && Mathf.Abs(transform.position.y - enemy.transform.position.y)<=0.1f)
            {
                if (enemy.GetComponent<EnemyController>() != null)
                {
                    enemy.GetComponent<EnemyController>().TakeDamePhysic(dame);
                    Destroy(gameObject);
                }
                else if(enemy.GetComponent<FlyController>() != null)
                {
                    enemy.GetComponent<FlyController>().TakeDamePhysic(dame);
                    Destroy(gameObject);
                }
            }

        }
        else
            Destroy(gameObject);
    }
}
