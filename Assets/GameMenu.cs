using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [Header("Game Panel")]
    public Text goldText;
    public ProgressBar healthBar;

    [Header("Lose Panel")]
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private Text _destroyedEnemy;

    [Header("Upgrade Panel")]
    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private Text _towerName;
    [SerializeField] private Text _towerDesctiption;
    [SerializeField] private Button upgradeButton;

    #region Upgrade panel
    public void ShowUpgradePanel()
    {
        Time.timeScale = 0;
        _upgradePanel.SetActive(true);

        ArrangeTowerProperties();
    }
    public void ArrangeTowerProperties()
    {
        var tower = GameManager.GetInstance().GetSelectedTower();

        var levelsCount = tower.towerProfile.levels.Count;

        TowerLevel currentLevel = tower.towerProfile.levels[tower.level];
        TowerLevel nextLevel = tower.level < levelsCount - 1 ? tower.towerProfile.levels[tower.level + 1] : null;

        _towerName.text = $"{tower.towerProfile.towerName} ({tower.level + 1}/{levelsCount})";
        _towerDesctiption.text = tower.towerProfile.towerDescription;
        _towerDesctiption.text += $"\n Attack {currentLevel.attack}";
        _towerDesctiption.text += $"\n Fire Speed {currentLevel.fireSpeed}";
        _towerDesctiption.text += $"\n Attack Distance {currentLevel.attackDistance}";

        var buttonTitle = nextLevel == null ? "Max level" : $"Upgrade \n {nextLevel?.price}";
        upgradeButton.GetComponentInChildren<Text>().text = buttonTitle;
        upgradeButton.interactable = tower.CanUpgrade();
    }
    public void CloseUpgradePanel()
    {
        Time.timeScale = 1;
        _upgradePanel.SetActive(false);
        GameManager.GetInstance().ClearSelectedTower();
    }

    #endregion

    #region Loose Panel
    public void ShowLosePanel()
    {
        _losePanel.SetActive(true);
        var title = $"Enemy destroyed: {GameManager.GetInstance().enemyDestroyed}";
        _destroyedEnemy.text = title;
    }
    #endregion
}
