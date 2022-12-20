using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace Features.Enemy.Scripts
{
    public class EnemyStorage : MonoBehaviour
    {
        [SerializeField] private EnemyView enemyPrefab;
        [SerializeField] private EnemyPig enemyPigPrefab;
        [SerializeField] private EnemyGhost enemyGhostPrefab;
        [SerializeField] private PathData config;
        [SerializeField] private Transform startPoint;
        [SerializeField] private int waveLength = 10;
        [SerializeField] private float offset = 5f;
        [SerializeField] private TMP_Text waveCount;
        [SerializeField] private TMP_Text killsCount;
        [SerializeField] private Transform container;
        private int _currentWave = 1;
        private Random _rnd = new Random();
        private int _kills;
        private int _enemyCount;
        private float _currentOffset = 5f;

        public bool IsGameOver;
        public event EventHandler<int> KillsChangeEvent;
        public event EventHandler<int> WaveChangeEvent;
        private List<EnemyView> _enemys = new List<EnemyView>();
        private List<EnemyPig> _pigs = new List<EnemyPig>();
        private List<EnemyGhost> _ghosts = new List<EnemyGhost>();

        private void Start()
        {
            _currentOffset = offset;
            KillsChangeEvent += (sender, i) => { killsCount.text = $"{i}"; };
            WaveChangeEvent += (sender, i) => { waveCount.text = $"{i}"; };
        }

        public void Clear()
        {
            foreach (var enemy in _enemys)
            {
                if (enemy != null)
                {
                    Destroy(enemy.gameObject);
                }
            }
            foreach (var enemyGhost in _ghosts)
            {
                if (enemyGhost != null)
                {
                    Destroy(enemyGhost.gameObject);
                }
            }
            foreach (var enemyPig in _pigs)
            {
                if (enemyPig != null)
                {
                    Destroy(enemyPig.gameObject);
                }
            }
            _enemys.Clear();
            _currentOffset = offset;
            _kills = 0;
            _currentWave = 0;
            _enemyCount = 0;
            KillsChangeEvent?.Invoke(this, _kills);
            WaveChangeEvent?.Invoke(this, _currentWave);
        }
        public void StartGame()
        {
            _enemyCount++;
            SpawnEnemy();
            if (_enemyCount < waveLength)
            {
                DOVirtual.DelayedCall(_currentOffset, () =>
                {
                    if (!IsGameOver)
                    {
                        StartGame();
                    }
                });
            }
        }

        private void SpawnEnemy()
        {
            var chance = _rnd.Next(0, 100);
            if (chance <= 70)
            {
                SpawnCommonEnemy();
            }
            else if (chance > 70 && chance <= 90)
            {
                SpawnPigEnemy();
            }
            else
            {
                SpawnGhostEnemy();
            }
        }

        public void SpawnCommonEnemy()
        {
            var enemy = Instantiate(enemyPrefab, startPoint.position, new Quaternion());
            enemy.transform.SetParent(container);
            _enemys.Add(enemy);
            enemy.DeathEvent += (sender, args) => 
            { 
                _kills++;
                KillsChangeEvent?.Invoke(this, _kills);
                _enemys.Remove(enemy);
            };
            if (_enemyCount >= waveLength)
            {
                enemy.DeathEvent += (sender, args) =>
                {
                    _currentWave++;
                    WaveChangeEvent?.Invoke(this, _currentWave);
                    _enemyCount = 0;

                    if (_currentOffset > 1f)
                    {
                        _currentOffset--;
                    }
                };
            }
            enemy.SetPathData(config.Paths[_rnd.Next(0,2)]);
            enemy.Walk();
        }
    
        public void SpawnPigEnemy()
        {
            var enemy = Instantiate(enemyPigPrefab, startPoint.position, new Quaternion());
            enemy.transform.SetParent(container);
            _pigs.Add(enemy);
            enemy.DeathEvent += (sender, args) => 
            { 
                _kills++;
                KillsChangeEvent?.Invoke(this, _kills);
                _pigs.Remove(enemy);
            };
            if (_enemyCount >= waveLength)
            {
                enemy.DeathEvent += (sender, args) =>
                {
                    _currentWave++;
                    WaveChangeEvent?.Invoke(this, _currentWave);
                    _enemyCount = 0;

                    if (_currentOffset > 1f)
                    {
                        _currentOffset--;
                    }
                };
            }
            enemy.SetPathData(config.Paths[_rnd.Next(0,2)]);
            enemy.Walk();
        }

        public void SpawnGhostEnemy()
        {
            var enemy = Instantiate(enemyGhostPrefab, startPoint.position, new Quaternion());
            enemy.transform.SetParent(container);
            _ghosts.Add(enemy);
            enemy.DeathEvent += (sender, args) => 
            { 
                _kills++;
                KillsChangeEvent?.Invoke(this, _kills);
                _ghosts.Remove(enemy);
            };
            if (_enemyCount >= waveLength)
            {
                enemy.DeathEvent += (sender, args) =>
                {
                    _currentWave++;
                    WaveChangeEvent?.Invoke(this, _currentWave);
                    _enemyCount = 0;

                    if (_currentOffset > 1f)
                    {
                        _currentOffset--;
                    }
                };
            }
            enemy.Goal = config.Paths[0].wayPoints[config.Paths[0].wayPoints.Count - 1].position;
            enemy.Walk();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SpawnPigEnemy();
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SpawnGhostEnemy();
            }
        }
    }
}
