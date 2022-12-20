using System;
using Features.Enemy.Scripts;
using TMPro;
using UnityEngine;

public class GameFinish : MonoBehaviour
{
    [SerializeField] private EnemyStorage enemyStorage;
    [SerializeField] private TMP_Text text;
    [SerializeField] private int gameOverLimit;

    private int _count;

    public event EventHandler GameOverEvent;

    private void Start()
    {
        text.text = $"{_count}/{gameOverLimit}";
    }

    public void Clear()
    {
        _count = 0;
        text.text = $"{_count}/{gameOverLimit}";
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<EnemyView>();
        if (enemy != null)
        {
            Destroy(enemy.gameObject);
            _count++;
            text.text = $"{_count}/{gameOverLimit}";
            if (_count >= gameOverLimit)
            {
                enemyStorage.IsGameOver = true;
                GameOverEvent?.Invoke(this, EventArgs.Empty);
            }
            return;
        }
        var pig = other.GetComponent<EnemyPig>();
        if (pig != null)
        {
            Destroy(pig.gameObject);
            _count++;
            text.text = $"{_count}/{gameOverLimit}";
            if (_count >= gameOverLimit)
            {
                enemyStorage.IsGameOver = true;
                GameOverEvent?.Invoke(this, EventArgs.Empty);
            }
            return;
        }
        var gh = other.GetComponent<EnemyGhost>();
        if (gh != null)
        {
            Destroy(gh.gameObject);
            _count++;
            text.text = $"{_count}/{gameOverLimit}";
            if (_count >= gameOverLimit)
            {
                enemyStorage.IsGameOver = true;
                GameOverEvent?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
