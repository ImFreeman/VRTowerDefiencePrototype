using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyPig : MonoBehaviour
{
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private float speed;
    [SerializeField] private Animator charAnimator;
    [SerializeField] private int health;
    [SerializeField] private PathData pathData;
    [SerializeField] private ParticleSystem fireParticle;
    
    private List<Vector3> _pathData = new List<Vector3>();
    private int _currentIndex;
    private bool _isWalking;
    private Tween _walkTween;
    private bool _isDead;
    private bool _inStun;

    public EventHandler DeathEvent;

    private void Start()
    {
        if (pathData != null)
        {
            foreach (var data in pathData.Paths[0].wayPoints)
            {
                _pathData.Add(data.position);
            }
        }

        DeathEvent += (sender, args) =>
        {
            _walkTween.Kill();
            charAnimator.Play("Dead");
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
            if (!_isDead)
            {
                _isDead = true;
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
    public void ActiveFire()
    {
        fireParticle.gameObject.SetActive(true);
        fireParticle.Play();

        DOVirtual.DelayedCall(3f, () =>
        {
            fireParticle.gameObject.SetActive(false);
        });
    }
    

    public void Walk()
    {
        _walkTween.Kill();
        if (bodyTransform != null)
        {
            if (!_isWalking)
            {
                charAnimator.Play("Walk");
            }
            _isWalking = true;
            bodyTransform.LookAt(_pathData[_currentIndex]);
            _walkTween = bodyTransform
                .DOMove(_pathData[_currentIndex], speed * Vector3.Distance(bodyTransform.position,_pathData[_currentIndex]) ).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _currentIndex++;
                    if (_currentIndex >= _pathData.Count)
                    {
                        _currentIndex = 0;
                        _isWalking = false;
                    }
                    else
                    {
                        Walk();
                    }
                });
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_isDead)
        {
            if (other.gameObject.CompareTag("Hand"))
            {
                if (!_inStun)
                {
                    _inStun = true;
                    _walkTween.Kill();
                    _isWalking = false;
                    charAnimator.Play("Hit");
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        _inStun = false;
                        Walk();
                    });
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Walk();
        }
    }
}
