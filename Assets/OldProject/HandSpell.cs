using Features.Enemy.Scripts;
using UnityEngine;

public class HandSpell : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<EnemyView>();
        if (enemy != null)
        {
            enemy.ChangeHealth(100);
        }
    }
}
