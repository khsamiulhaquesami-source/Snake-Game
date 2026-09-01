using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class RegisterManager : MonoBehaviour
{
    public TMP_InputField firstNameInput;
    public TMP_InputField lastNameInput;
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text messageText;

    public void Register()
    {
        StartCoroutine(RegisterUser());
    }

    IEnumerator RegisterUser()
    {
        string json = JsonUtility.ToJson(new UserData
        {
            FirstName = firstNameInput.text,
            LastName = lastNameInput.text,
            Username = usernameInput.text,
            Password = passwordInput.text,
            ConfirmPassword = passwordInput.text
        });

        string url = "http://localhost:5000/api/Register";

        UnityWebRequest request = new UnityWebRequest(url, "POST");

        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            messageText.text = request.downloadHandler.text;
        }
        else
        {
            messageText.text = "Registration failed.";
            Debug.LogError(request.error);
            Debug.LogError(request.downloadHandler.text);
        }
    }

    [System.Serializable]
    public class UserData
    {
        public string FirstName;
        public string LastName;
        public string Username;
        public string Password;
        public string ConfirmPassword;
    }
}