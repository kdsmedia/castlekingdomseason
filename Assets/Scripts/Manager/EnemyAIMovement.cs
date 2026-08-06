using UnityEngine;
using System.Collections;

/// <summary>
/// EnemyAIMovement — moves this GameObject toward the nearest target on layer 8.
/// Formerly named "Test.cs"; renamed to reflect its actual purpose.
/// </summary>
public class EnemyAIMovement : MonoBehaviour
{
    bool haveEnemy;
    int valueMask = 1 << 8;

    void Update()
    {
        AutoFindPlayer();
    }
    void AutoFindPlayer()
    {


        Collider2D[] enemyCollider = Physics2D.OverlapCircleAll(transform.position, 10f, valueMask);
        Collider2D enemy = MinDistance(enemyCollider);

        if (enemy != null)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(enemy.transform.position.x, enemy.transform.position.y), 5 * Time.deltaTime);
            Vector3 dir = enemy.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            transform.Translate(5 * Vector3.right * Time.deltaTime);
        }
    }
    Collider2D MinDistance(Collider2D[] enemies)
    {
        Collider2D enemy = null;
        int i = 0;
        if (enemies.Length == 0)
        {
            haveEnemy = false;
            return enemy;
        }
        else if (enemies.Length == 1)
        {
            haveEnemy = true;
            enemy = enemies[0];
            return enemy;
        }
        else
        {
            enemy = enemies[0];
            while (i <= enemies.Length - 2)
            {
                if (Vector3.Distance(enemy.transform.position, transform.position) > Vector3.Distance(enemies[i + 1].transform.position, transform.position))
                    enemy = enemies[i + 1];
                i++;
            }
            haveEnemy = true;
        }
        return enemy;
    }
}
