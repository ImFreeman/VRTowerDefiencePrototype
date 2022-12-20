using Features.Enemy.Scripts;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<EnemyView>();
        if (enemy != null)
        {
            enemy.ChangeHealth(enemy.IsWalking? -15 : -5);
            Destroy(gameObject);
            return;
        }
        var pig = other.GetComponent<EnemyPig>();
        if (pig != null)
        {
            pig.ChangeHealth(-5);
            Destroy(gameObject);
            return;
        }
        var gh = other.GetComponent<EnemyGhost>();
        if (gh != null)
        {
            gh.ChangeHealth(-5);
            Destroy(gameObject);
        }
    }
}
