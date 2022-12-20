using System;
using DG.Tweening;
using Features.Hand_Tracking.LightningSpell;
using Features.Hand_Tracking.PoisonSpell;
using Oculus.Interaction;
using TMPro;
using UnityEngine;

public class SpellShooter : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Lightning lightningPrefab;
    [SerializeField] private Poison poisonPrefab;
    [SerializeField] private float fireSpeed;
    [SerializeField] private IceBalObj icePrefab;
    [SerializeField] private float iceSpeed;
    [SerializeField] private int maxMana;
    [SerializeField] private SpellAim aim;
    [SerializeField] private TMP_Text manaCounter;
    [SerializeField, Interface(typeof(ISelector))]
    private MonoBehaviour fireSelector;
    [SerializeField, Interface(typeof(ISelector))]
    private MonoBehaviour iceSelector;

    private ISelector _iceSelector;
    private ISelector _fireSelector;
    private int _currentMana;

    private const int FireCost = 20;
    private const int IceCost = 20;
    private const int LightningCost = 20;
    private const int PoisonCost = 20;

    public int CurrentMana => _currentMana;
    public int MaxMana => maxMana;

    public event EventHandler<int> ManaChangeEvent; 
    
    private void Start()
    {
        _currentMana = maxMana;
        _iceSelector = iceSelector as ISelector;
        _fireSelector = fireSelector as ISelector;
        _fireSelector.WhenSelected += ShootFire;
        _iceSelector.WhenSelected += ShootIce;
        manaCounter.text = $"{_currentMana}/{maxMana}";
        ManaChangeEvent += (sender, i) => { manaCounter.text = $"{_currentMana}/{maxMana}"; };
    }

    public void UpdateMana()
    {
        _currentMana = maxMana;
        ManaChangeEvent?.Invoke(this,_currentMana);
    }

    private float _currentTime;

    private void Update()
    {
        if (_currentMana < maxMana)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= 0.5f)
            {
                _currentTime = 0;
                _currentMana++;
                ManaChangeEvent?.Invoke(this,_currentMana);
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Poison");
            _currentMana -= PoisonCost;
            ManaChangeEvent?.Invoke(this, _currentMana);
            var poison = Instantiate(poisonPrefab);
            poison.transform.position = Vector3.zero;
        }
    }

    private void ShootFire()
    {
        if (aim.IsShape && aim.IsLevel)
        {
            if (_currentMana - FireCost >= 0)
            {
                _currentMana -= FireCost;
                ManaChangeEvent?.Invoke(this, _currentMana);
                var fireBall = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
                var tween = 
                    fireBall.transform
                        .DOMove(
                            aim.EndPosition,
                            fireSpeed * Vector3.Distance(aim.EndPosition, fireBall.transform.position)).
                        SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        DOVirtual.DelayedCall(0.5f, () =>
                        {
                            fireBall.transform.DOScale(new Vector3(0, 0, 0), 1f).SetEase(Ease.InBounce)
                                .OnComplete(() =>
                                {
                                    Destroy(fireBall.gameObject);
                                }); 
                        });
                    });
                DOVirtual.DelayedCall(15f, () =>
                {
                    if (fireBall != null)
                    {
                        tween.Kill();
                        Destroy(fireBall.gameObject);
                    }
                });
            }
        }
    }

    private void ShootIce()
    {
        if (aim.IsShape && aim.IsLevel)
        {
            if (_currentMana - FireCost >= 0)
            {
                _currentMana -= IceCost;
                ManaChangeEvent?.Invoke(this, _currentMana);
                var ice = Instantiate(icePrefab, transform.position, Quaternion.identity);
                var tween = 
                    ice.transform
                        .DOMove(
                            aim.EndPosition,
                            iceSpeed * Vector3.Distance(aim.EndPosition, ice.transform.position)).
                        SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            ice.MakeFloor();
                            DOVirtual.DelayedCall(15f, () =>
                            {
                                ice.transform.DOScale(new Vector3(0, 0, 0), 1f).SetEase(Ease.Linear)
                                    .OnComplete(() =>
                                    {
                                        Destroy(ice.gameObject);
                                    }); 
                            });
                        });
                DOVirtual.DelayedCall(15f, () =>
                {
                    if (ice != null)
                    {
                        tween.Kill();
                        Destroy(ice.gameObject);
                    }
                });
            }
        }
    }

    public void ShootLightning()
    {
        if (aim.IsShape && aim.IsLevel)
        {
            if (_currentMana - LightningCost >= 0)
            {
                _currentMana -= LightningCost;
                ManaChangeEvent?.Invoke(this, _currentMana);
                var lightning = Instantiate(lightningPrefab);
                lightning.transform.position = aim.EndPosition;
            }
        }
    }
    
    public void ShootPoison()
    {
        if (aim.IsShape && aim.IsLevel)
        {
            if (_currentMana - PoisonCost >= 0)
            {
                _currentMana -= PoisonCost;
                ManaChangeEvent?.Invoke(this, _currentMana);
                var poison = Instantiate(poisonPrefab);
                poison.transform.position = aim.EndPosition;
            }
        }
    }

    
}
