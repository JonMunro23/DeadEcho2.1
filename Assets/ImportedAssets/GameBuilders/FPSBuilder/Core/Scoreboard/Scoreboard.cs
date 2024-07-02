using GameBuilders.FPSBuilder.Core.Managers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Scoreboard : MonoBehaviour
{
    //[Header("Keybinds")]
    //KeyCode openScoreboardKey = KeyCode.Tab;

    [Space]
    [SerializeField]
    GameObject scoreBoard;

    [SerializeField]
    Transform collectedUpgradesParent;
    [SerializeField]
    CollectedUpgradeUIElement collectedUpgradeUIElement;

    List<CollectedUpgradeUIElement> spawnedCollectedUpgradeUIElements = new List<CollectedUpgradeUIElement>();

    [SerializeField]
    PlayerScoreboardRow playerScoreboardRowPrefab;
    [SerializeField]
    PlayerScoreboardRow playerScoreboardRow;
    [SerializeField]
    TMP_Text levelNameText;

    [SerializeField]
    Transform scoreboardPlayerRowParent;

    public static bool isScoreboardOpen;

    public int numberOfPlayers = 1;

    private InputActionMap m_InputBindings;
    private InputAction m_OpenScoreboard;    


    private void OnEnable()
    {
        ZombieHealthController.onDeath += UpdateKills;
        PointsManager.onPointsAdded += UpdateScore;
    }

    private void OnDisable()
    {
        ZombieHealthController.onDeath -= UpdateKills;
        PointsManager.onPointsAdded -= UpdateScore;
    }

    private void Start()
    {
        // Input Bindings
        m_InputBindings = GameplayManager.Instance.GetActionMap("Menu");
        m_InputBindings.Enable();

        m_OpenScoreboard = m_InputBindings.FindAction("Scoreboard");

        //for (int i = 0; i < numberOfPlayers; i++)
        //{
            SpawnPlayerRow(1);
        //}

        UpdateLevelNameText();
    }

    void UpdateLevelNameText()
    {
        levelNameText.text = SceneManager.GetActiveScene().name;
    }

    void SpawnPlayerRow(int playerIndex)
    {
        playerScoreboardRow = Instantiate(playerScoreboardRowPrefab, scoreboardPlayerRowParent);
        playerScoreboardRow.SetName("Player " + playerIndex);
        playerScoreboardRow.AddScore(PointsManager.Instance.CurrentPoints);
    }

    void SpawnCollectedUpgrades()
    {
        List<Upgrade> collectedUpgrades = PlayerUpgrades.Instance.GetCollectedUpgrades();
        foreach (Upgrade upgrade in collectedUpgrades)
        {
            CollectedUpgradeUIElement clone = Instantiate(collectedUpgradeUIElement, collectedUpgradesParent);
            clone.Init(upgrade);
            spawnedCollectedUpgradeUIElements.Add(clone);
        }
    }

    void RemoveSpawnedCollectedUpgradeUIElements()
    {
        foreach (CollectedUpgradeUIElement UIElement in spawnedCollectedUpgradeUIElements)
        {
            Destroy(UIElement.gameObject);
        }
        spawnedCollectedUpgradeUIElements.Clear();
    }

    private void Update()
    {
        //if(!PauseMenu.isPaused)
        //{
            if(m_OpenScoreboard.triggered)
            {
                ToggleScoreboard();
            }
        //}
    }

    void ToggleScoreboard()
    {
        if(isScoreboardOpen)
        {
            CloseScoreboard();
        }
        else
            OpenScoreboard();
    }

    void OpenScoreboard()
    {
        isScoreboardOpen = true;
        scoreBoard.SetActive(true);
        SpawnCollectedUpgrades();
    }
    void CloseScoreboard()
    {
        isScoreboardOpen = false;
        RemoveSpawnedCollectedUpgradeUIElements();
        scoreBoard.SetActive(false);
    }
    public void UpdateScore(int scoreToAdd)
    {
        playerScoreboardRow.AddScore(scoreToAdd);
    }
    public void UpdateKills(bool wasHeadshot)
    {
        playerScoreboardRow.AddKill();
        if(wasHeadshot)
        {
            UpdateHeadshots();
        }
    }

    public void UpdateHeadshots()
    {
        playerScoreboardRow.AddHeadshot();
    }
}
