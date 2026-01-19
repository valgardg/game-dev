using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public Jump playerJump;
    public CheckPlayerInCrosshair checkPlayerInCrosshair;
    public FollowPlayer crosshairFollowPlayer;
    public LevelText levelText;
    public ScoreText scoreText;
    public GameOverPanel gameOverPanel;

    public int ShotsBetweenIncrease = 5;
    [SerializeField] private int shotIndex = 0;
    private int levelIndex = 1;
    private int playerScore = 0;
    private bool gameOver = false;

    void Start()
    {
        StartCoroutine(StartCrosshairLoop());
    }

    IEnumerator StartCrosshairLoop()
    {
        levelText.DisplayLevel(levelIndex);
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(CrosshairLoop());
    }

    IEnumerator CrosshairLoop()
    {
        while (!gameOver)
        {
            crosshairFollowPlayer.StartFollowPlayer();
            yield return new WaitForSeconds(5f);


            crosshairFollowPlayer.StopFollowPlayer();
            bool playerInCrossHair = checkPlayerInCrosshair.GetPlayerInCrosshair();
            
            // first check if player was shot 
            if(playerInCrossHair)
            {
                HandleGameOver();
                break;
            }

            shotIndex++; // COUNT THE SHOT

            // update game state after shot has been fired
            if (shotIndex >= ShotsBetweenIncrease)
            {
                crosshairFollowPlayer.IncreaseSpeed();
                shotIndex = 0;
                levelIndex += 1;
                AddScore(5);
                levelText.DisplayLevel(levelIndex);
            }

            // Update player score only if they were not shot
            AddScore(1);

            yield return new WaitForSeconds(2f);
        }
    }

    private void ReloadCurrentScene()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void AddScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        scoreText.DisplayScore(playerScore);
    }

    private void HandleGameOver()
    {
        gameOver = true;
        playerJump.SetAllowJump(false);
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.DisplayGameOverPanel(levelIndex, playerScore);
    }

    public void Reset()
    {
        ReloadCurrentScene();
    }

    public void CallHandleGameOver()
    {
        HandleGameOver();
    }
}
