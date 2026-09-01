using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WelcomeManager : MonoBehaviour
{
    public TMP_Text welcomeMessage;

    void Start()
    {
        string firstName = PlayerPrefs.GetString("FirstName", "Player");

        welcomeMessage.text = "Hello, " + firstName + "!";
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("ModeSelect");
    }
}