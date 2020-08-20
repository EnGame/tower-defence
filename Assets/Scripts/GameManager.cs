using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("User info")]
    private int _userGold = 0;
    [SerializeField] private int _userHealth = 30;

    [Header("UI")]
    public GameMenu gameUi;

    [Header("Level")]
    public int enemyDestroyed;
    public Transform[] wayPoints = new Transform[0];

    [Header("Enemy")]
    public List<Enemy> enemyList = new List<Enemy>();
    [SerializeField] private GameObject[] enemyPrefabs = new GameObject[0];
    [SerializeField] private Transform enemySpawnPoint;

    [Header("Prefabs")]
    [SerializeField] private GameObject _enemyPrefab;

    //WaveSettings
    private int _wave = 0;
    private float _waveTimer = 2;
    private bool _waitNextWave = false;

    private Tower _selectedTower;
    
    #region Unity Methods
    private void Start()
    {
        gameUi.healthBar.maxValue = _userHealth;
        gameUi.healthBar.Value = _userHealth;
    }
    private void Update()
    {
        if (IsWaveEnded())
        {
            _waveTimer = 2;
            _wave++;
            _waitNextWave = true;
        }

        if (!_waitNextWave) return;

        _waveTimer -= Time.deltaTime;

        if (_waveTimer <= 0)
            StartCoroutine(EGenerateWave());
    }

    //Draw Enemy path
    public void OnDrawGizmos()
    {
        if (wayPoints.Length <= 1) return;
        for (int i = 0; i < wayPoints.Length - 1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(wayPoints[i].position, wayPoints[i + 1].position);
        }
    }
    #endregion

    #region Wave
    private bool IsWaveEnded()
    {
        return enemyList.Count <= 0 && _waitNextWave == false;
    }
    public IEnumerator EGenerateWave()
    {
        _waitNextWave = false;
        var count = Random.Range(_wave, _wave + 10);
        for (int i = 0; i < count; i++)
        {
            var enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            var go = Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);
            var enemy = go.GetComponent<Enemy>();
            enemy.InitEnemy(_wave);
            enemyList.Add(enemy);
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion

    #region Gold And Tower Upgrade
    public int GetGoldCount()
    {
        return _userGold;
    }
    public void AddGold(int gold)
    {
        _userGold += gold;
        gameUi.goldText.text = _userGold.ToString();
    }
    public void SubtractGold(int gold)
    {
        _userGold -= gold;
        gameUi.goldText.text = _userGold.ToString();
    }
    public void UpgradeTower()
    {
        if (GetSelectedTower().CanUpgrade())
        {
            GetSelectedTower().level++;
            SubtractGold(GetSelectedTower().towerProfile.levels[GetSelectedTower().level].price);
            gameUi.ArrangeTowerProperties();
        }
    }
    public Tower GetSelectedTower()
    {
        return _selectedTower;
    }
    public void ClearSelectedTower()
    {
        _selectedTower = null;
    }
    public void Select(Tower tower)
    {
        _selectedTower = tower;
    }
    #endregion

    #region Main Tower And Lose
    public void ReceiveDamage(int damage)
    {
        _userHealth -= damage;
        gameUi.healthBar.Value = _userHealth;

        if (_userHealth <= 0)
        {
            Lose();
        }
    }
    private void Lose()
    {
        Time.timeScale = 0;
        gameUi.ShowLosePanel();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    #endregion
}
