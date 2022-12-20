using System;
using UnityEngine;

namespace Features.Enemy.Scripts
{
    public class EnemyBody : MonoBehaviour
    {
        [SerializeField] private EnemyView enemyView;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Lava"))
            {
                enemyView.ChangeHealth(-100);
            }
        }
    }
}
