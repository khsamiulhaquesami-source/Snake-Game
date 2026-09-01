using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameModeManager : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text timeUpText;

    private float timeLeft = 120f;
    private bool timedMode;
    private bool gameEnded;

    void Start()
    {
        Time.timeScale = 1f;

        timedMode = PlayerPrefs.GetString("GameMode", "Classic") == "Timed";

        timeUpText.gameObject.SetActive(false);

        if (timedMode)
        {
            timerText.gameObject.SetActive(true);
            UpdateTimer();
        }
        else
        {
            timerText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!timedMode || gameEnded)
            return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            gameEnded = true;

            UpdateTimer();

            timeUpText.gameObject.SetActive(true);

            Time.timeScale = 0f;

            Debug.Log("TIME'S UP!");
            return;
        }

        UpdateTimer();
    }

    void UpdateTimer()
    {
        timerText.text = "TIME: " + Mathf.CeilToInt(timeLeft);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void GoToModeSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("ModeSelect");
    }
}