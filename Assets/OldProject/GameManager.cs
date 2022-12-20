using Features.Enemy.Scripts;
using Oculus.Interaction;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject startWindow;
    [SerializeField] private GameObject gameOverWindow;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject midWave; 
    [SerializeField] private EnemyStorage enemyStorage;
    [SerializeField] private GameFinish gameFinish;

    private ISelector _selector;

    private void Start()
    {
        _selector = GameObject.Find("PlayPose").GetComponent<ActiveStateSelector>() as ISelector;
        _selector.WhenSelected += StartGame;
        enemyStorage.WaveChangeEvent += (sender, i) =>
        {
            if (i != 0)
            {
                midWave.SetActive(true);
                hud.SetActive(false);
                _selector.WhenSelected += StartGame;
            }
        };
        gameFinish.GameOverEvent += (sender, args) =>
        {
            if (midWave.gameObject.activeSelf)
            {
                midWave.gameObject.SetActive(false);
            }
            else
            {
                _selector.WhenSelected += StartGame;
            }
            enemyStorage.Clear();
            hud.SetActive(false);
            gameFinish.Clear();
            gameOverWindow.SetActive(true);
        };
    }

    private void StartGame()
    {
        GameObject.Find("r_handMeshNode").GetComponent<SpellShooter>().UpdateMana();
        startWindow.SetActive(false);
        gameOverWindow.SetActive(false);
        midWave.SetActive(false);
        hud.SetActive(true);
        _selector.WhenSelected -= StartGame;
        enemyStorage.IsGameOver = false;
        enemyStorage.StartGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartGame();
        }
    }
}
