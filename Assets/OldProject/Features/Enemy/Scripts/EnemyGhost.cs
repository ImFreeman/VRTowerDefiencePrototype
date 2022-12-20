using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyGhost : MonoBehaviour
{
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private float speed;
    [SerializeField] private Animator charAnimator;
    [SerializeField] private int health;
    [SerializeField] private ParticleSystem fireParticle;
    
    private bool _isWalking;
    private Tween _walkTween;
    private bool _isDead;
    private Vector3 _goal;

    public EventHandler DeathEvent;

    public Vector3 Goal
    {
        get => _goal;
        set => _goal = value;
    }

    private void Start()
    {
        DeathEvent += (sender, args) =>
        {
            _walkTween.Kill();
            transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InBounce)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                });
        };
    }

    public void ActiveFire()
    {
        fireParticle.gameObject.SetActive(true);
        fireParticle.Play();

        DOVirtual.DelayedCall(6f, () =>
        {
            fireParticle.gameObject.SetActive(false);
        });
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
            bodyTransform.LookAt(_goal);
            _walkTween = bodyTransform.DOMove(_goal, speed * Vector3.Distance(bodyTransform.position,_goal) ).SetEase(Ease.Linear);
        }
    }
}
