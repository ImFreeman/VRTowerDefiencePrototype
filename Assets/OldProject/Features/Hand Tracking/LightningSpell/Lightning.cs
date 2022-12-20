using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Features.Enemy.Scripts;
using UnityEngine;

namespace Features.Hand_Tracking.LightningSpell
{
    public class Lightning : MonoBehaviour
    {
        private List<EnemyView> _enemyList = new List<EnemyView>();
        private List<EnemyPig> _pigs = new List<EnemyPig>();
        private List<EnemyGhost> _ghs = new List<EnemyGhost>();

        private void OnEnable()
        {
            StartCoroutine(TakeDamage());
            DOVirtual.DelayedCall(5f, () =>
            {
                Destroy(gameObject);
            });
        }

        private void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponent<EnemyView>();
            if (enemy != null)
            {
                _enemyList.Add(enemy);
                return;
            }
            var pig = other.GetComponent<EnemyPig>();
            if (pig != null)
            {
                _pigs.Add(pig);
                return;
            }
            var gh = other.GetComponent<EnemyGhost>();
            if (gh != null)
            {
                _ghs.Add(gh);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            var enemy = other.GetComponent<EnemyView>();
            if (enemy != null)
            {
                _enemyList.Remove(enemy);
                return;
            }
            var pig = other.GetComponent<EnemyPig>();
            if (pig != null)
            {
                _pigs.Remove(pig);
                return;
            }
            var gh = other.GetComponent<EnemyGhost>();
            if (gh != null)
            {
                _ghs.Remove(gh);
            }
        }

        private IEnumerator TakeDamage()
        {
            yield return new WaitForSeconds(1);
            foreach (var enemy in _enemyList)
            {
                enemy.ChangeHealth(enemy.IsWalking? -35 : -50);
            }
            foreach (var pig in _pigs)
            {
                pig.ChangeHealth(-35);
            }

            foreach (var gh in _ghs)
            {
                gh.ChangeHealth(-35);
            }
            StartCoroutine(TakeDamage());
        }
    }
}
