using Features.Enemy.Scripts;
using UnityEngine;

public class IceFloor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<EnemyView>();
        if (enemy != null)
        {
            enemy.ChangeHealth(enemy.IsWalking? -15 : -5);
            enemy.Fall(150f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        var enemy = other.gameObject.GetComponent<EnemyView>();
        if (enemy != null)
        {
            enemy.ChangeHealth(enemy.IsWalking? -15 : -5);
            enemy.Fall(125f);
        }
    }
}
