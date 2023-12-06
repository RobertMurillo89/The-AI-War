using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("----GameStats----")]
    private int enemyCount;
    private int totalWaves;
    private int completedWaves;

    [Header("-----Components----")]
    public GameObject WinPanel;
    public GameObject LosePanel;
    public GameObject PauseMenu;

    #region Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public void StartLevel()
    {
        enemyCount = 0;
        totalWaves = 0;
        completedWaves = 0;
    }

    public void RegisterSpawner(Spawner spawner)
    {
        totalWaves += spawner.GetTotalWaves();
    }
    public void NotifySpawnerCompleted(Spawner spawner)
    {
        completedWaves += spawner.GetTotalWaves();
        CheckForWin();
    }

    public void IncrementEnemyCount()
    {
        enemyCount++;
    }
    public void DecrementEnemyCount()
    {
        enemyCount--;
        CheckForWin();
    }

    public void SetTotalWaves(int waves)
    {
        totalWaves = waves;     
    }
 

    private void CheckForWin()
    {
        if (enemyCount <=0 && completedWaves == totalWaves)
        {
            Debug.Log("You fuckin did it!");
            WinGame();
        }
    }

    public void WinGame()
    {
        StartCoroutine(HandleWin());
    }

    public void LoseGame()
    {
        StartCoroutine(HandleLoss());
    }

    IEnumerator HandleWin()
    {
        PauseGame();
        WinPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(4);
        WinPanel.SetActive(false);
        OpenPauseMenu();
    }
    IEnumerator HandleLoss()
    {
        PauseGame();
        LosePanel.SetActive(true);
        yield return new WaitForSecondsRealtime(4); // Wait for 2 seconds
        LosePanel.SetActive(false);
        OpenPauseMenu();
    }
    private void PauseGame()
    {
        Time.timeScale = 0; // Pauses the game
    }
    private void OpenPauseMenu()
    {
        PauseMenu.SetActive(true);
    }
}
