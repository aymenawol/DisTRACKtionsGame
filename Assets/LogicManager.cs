using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicManager : MonoBehaviour
{
    public Text scoreText;
    private float startTime;
    private int playerScore;
    private float scoreIncreaseRate = 50;
    private float speedIncreaseRate = 25f;
    private float currentSpeed = 1.0f;
    public GameObject gameOverScreen;
    public List<GameObject> objectsToHide;

    private bool isGameOver = false;

    private void Start()
    {
        // Ensure cursor is always visible and unlocked
        Cursor.lockState = CursorLockMode.None; // Don't lock the cursor
        Cursor.visible = true; // Ensure cursor is always visible

        startTime = Time.time;
    }

    private void Update()
    {
        if (!isGameOver)
        {
            float timeElapsed = Time.time - startTime;
            playerScore = Mathf.FloorToInt(timeElapsed * scoreIncreaseRate);
            scoreText.text = playerScore.ToString();
            currentSpeed = 1.0f + timeElapsed * speedIncreaseRate;
        }
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void gameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);

        // Ensure the cursor is visible when the game is over
        Cursor.lockState = CursorLockMode.None;  // Make sure it's unlocked
        Cursor.visible = true;  // Ensure it stays visible

        foreach (GameObject obj in objectsToHide)
        {
            obj.SetActive(false);
        }
    }

    public void goToStartScene()
    {
        SceneManager.LoadScene("StartScene");
        Time.timeScale = 1;
    }
}