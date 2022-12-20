using DG.Tweening;
using Features.Enemy.Scripts;
using UnityEngine;

namespace Features.Hand_Tracking.PoisonSpell
{
    public class Poison : MonoBehaviour
    {
        private void OnEnable()
        {
            transform.DOScale(Vector3.one, 0.5f);
            DOVirtual.DelayedCall(3f, (() =>
            {
                Destroy(gameObject);
            }));
        }

        private void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponent<EnemyView>();
            if (enemy != null)
            {
                enemy.ChangeHealth(enemy.IsWalking? -35 : -50);
            }
            var pig = other.GetComponent<EnemyPig>();
            if (pig != null)
            {
                pig.ChangeHealth(-35);
                return;
            }
            var gh = other.GetComponent<EnemyGhost>();
            if (gh != null)
            {
                gh.ChangeHealth(-35);
            }
        }
    }
}
