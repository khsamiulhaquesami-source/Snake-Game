using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;

    public TMP_Text messageText;

    public string apiUrl =
        "https://localhost:7216/api/Account/login";

    public void Login()
    {
        if (usernameInput.text == "" || passwordInput.text == "")
        {
            messageText.text = "Please enter username and password.";
            return;
        }

        StartCoroutine(LoginRequest());
    }

    IEnumerator LoginRequest()
    {
        LoginData data = new LoginData
        {
            username = usernameInput.text,
            password = passwordInput.text
        };

        string json = JsonUtility.ToJson(data);

        using UnityWebRequest request =
            new UnityWebRequest(apiUrl, "POST");

        byte[] body =
            System.Text.Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader(
            "Content-Type",
            "application/json"
        );

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Login successful!");

            LoginResponse response =
                JsonUtility.FromJson<LoginResponse>(
                    request.downloadHandler.text
                );

            PlayerPrefs.SetInt("UserId", response.userId);
            PlayerPrefs.SetString("FirstName", response.firstName);
            PlayerPrefs.SetString("LastName", response.lastName);
            PlayerPrefs.SetString("Username", response.username);
            PlayerPrefs.Save();

            SceneManager.LoadScene("Welcome");
        }
        else
        {
            Debug.LogError(request.error);
            Debug.LogError(request.downloadHandler.text);

            messageText.text =
                "Invalid username or password.";
        }
    }
}

[System.Serializable]
public class LoginData
{
    public string username;
    public string password;
}

[System.Serializable]
public class LoginResponse
{
    public bool success;
    public string message;
    public int userId;
    public string firstName;
    public string lastName;
    public string username;
}
