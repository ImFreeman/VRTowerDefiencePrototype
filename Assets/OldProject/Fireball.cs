using Features.Enemy.Scripts;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<EnemyView>();
        if (enemy != null)
        {
            enemy.ChangeHealth(enemy.IsWalking? -35 : -50);
            enemy.ActiveFire();
            Destroy(gameObject);
            return;
        }
        var pig = other.GetComponent<EnemyPig>();
        if (pig != null)
        {
            pig.ChangeHealth(-35);
            pig.ActiveFire();
            Destroy(gameObject);
            return;
        }
        var gh = other.GetComponent<EnemyGhost>();
        if (gh != null)
        {
            gh.ChangeHealth(-35);
            gh.ActiveFire();
            Destroy(gameObject);
        }
    }
}
