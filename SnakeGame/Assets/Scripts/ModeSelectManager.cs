using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectManager : MonoBehaviour
{
    public void ClassicMode()
    {
        PlayerPrefs.SetString("GameMode", "Classic");
        PlayerPrefs.Save();

        SceneManager.LoadScene("Game");
    }

    public void TimedMode()
    {
        PlayerPrefs.SetString("GameMode", "Timed");
        PlayerPrefs.Save();

        SceneManager.LoadScene("Game");
    }

    public void Back()
    {
        SceneManager.LoadScene("Welcome");
    }
}