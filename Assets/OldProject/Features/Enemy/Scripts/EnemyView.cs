using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Features.Enemy.Scripts
{
    public class EnemyView : MonoBehaviour
    {
        public EventHandler DeathEvent;
        
        public bool IsDead;
        public bool IsWalking;
        
        [SerializeField] private ParticleSystem fireParticle;
        [SerializeField] private float speed;
        [SerializeField] private Animator charAnimator;
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private int health;

        private List<Vector3> _pathData = new List<Vector3>();
        private int _currentIndex;
        private Tween _walkTween;

        private void Start()
        {
            DeathEvent += (sender, args) =>
            {
                DOVirtual.DelayedCall(20f, () =>
                {
                    transform.DOScale(Vector3.zero, 3f).SetEase(Ease.InBounce)
                        .OnComplete(() =>
                        {
                            Destroy(gameObject);
                        });
                });
            };
        }

        public void ChangeHealth(int delta)
        {
            health += delta;
            if (health <= 0)
            {
                if (!IsDead)
                {
                    IsDead = true;
                    Fall();
                    DeathEvent?.Invoke(this,EventArgs.Empty);
                }
            }
        }
    
        public void SetPathData(PathWayPoints data)
        {
            foreach (var dataWayPoint in data.wayPoints)
            {
                _pathData.Add(dataWayPoint.position);
            }
        }

        public void Walk()
        {
            _walkTween.Kill();
            if (!IsWalking)
            {
                charAnimator.Play("Walk");
            }
            IsWalking = true;
            transform.LookAt(_pathData[_currentIndex]);
            _walkTween = transform
                .DOMove(_pathData[_currentIndex], speed * Vector3.Distance(transform.position,_pathData[_currentIndex]) ).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _currentIndex++;
                    if (_currentIndex >= _pathData.Count)
                    {
                        _currentIndex = 0;
                        IsWalking = false;
                    }
                    else
                    {
                        Walk();
                    }
                });
        }

        public void Fall(float punchForce = 0f)
        {
            _walkTween.Kill();
            IsWalking = false;
            RagdollSetActive(true);
            rigidBody.AddRelativeForce(Vector3.up * punchForce);
            DOVirtual.DelayedCall(5f, () =>
            {
                StandUp();
            });
        }

        private void StandUp()
        {
            if (!IsDead)
            {
                if (!IsWalking)
                {
                    IsWalking = true;

                    var pos = rigidBody.position;
                    pos.y = transform.position.y;
                    transform.position = pos;
                    RagdollSetActive(false);
                    charAnimator.Play("StandUp");
                    DOVirtual.DelayedCall(3f, () =>
                    {
                        if (IsWalking)
                        {
                            charAnimator.Play("Walk");
                            Walk();
                        }
                    });
                }
            }
        }
        public void ActiveFire()
        {
            fireParticle.gameObject.SetActive(true);
            fireParticle.Play();

            DOVirtual.DelayedCall(3f, () =>
            {
                fireParticle.gameObject.SetActive(false);
            });
        }

        private void RagdollSetActive(bool value)
        {
            charAnimator.enabled = !value;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Hand"))
            {
                ChangeHealth(-100);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Walk();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Fall(150f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                charAnimator.enabled = true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                rigidBody.AddRelativeForce(Vector3.back * 5000);
            }
        }
    }
}
