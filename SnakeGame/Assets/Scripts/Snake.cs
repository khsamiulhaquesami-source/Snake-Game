using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;

public class Snake : MonoBehaviour
{
    public float moveSpeed = 0.15f;

    public Transform food;
    public Transform bodyPrefab;
    public Sprite tailSprite;

    public TextMeshProUGUI scoreText;
    public GameObject gameOverText;
    public GameObject restartButton;

    int directionX = 1;
    int directionY = 0;

    float timer = 0f;
    int score = 0;

    bool gameOver = false;

    List<Transform> bodyParts = new List<Transform>();

    void Start()
    {
        score = 0;

        if (scoreText != null)
            scoreText.text = "Score: 0";

        if (gameOverText != null)
            gameOverText.SetActive(false);

        if (restartButton != null)
            restartButton.SetActive(false);
    }

    void Update()
    {
        if (gameOver)
            return;

        HandleInput();

        timer += Time.deltaTime;

        if (timer >= moveSpeed)
        {
            MoveSnake();
            timer = 0f;
        }
    }

    void HandleInput()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame ||
            Keyboard.current.wKey.wasPressedThisFrame)
        {
            if (directionY != -1)
            {
                directionX = 0;
                directionY = 1;
            }
        }

        if (Keyboard.current.downArrowKey.wasPressedThisFrame ||
            Keyboard.current.sKey.wasPressedThisFrame)
        {
            if (directionY != 1)
            {
                directionX = 0;
                directionY = -1;
            }
        }

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame ||
            Keyboard.current.aKey.wasPressedThisFrame)
        {
            if (directionX != 1)
            {
                directionX = -1;
                directionY = 0;
            }
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame ||
            Keyboard.current.dKey.wasPressedThisFrame)
        {
            if (directionX != -1)
            {
                directionX = 1;
                directionY = 0;
            }
        }
    }

    void MoveSnake()
    {
        Vector3 oldHeadPosition = transform.position;

        // Move body from back to front
        for (int i = bodyParts.Count - 1; i > 0; i--)
        {
            bodyParts[i].position = bodyParts[i - 1].position;
        }

        // First body piece follows the old head position
        if (bodyParts.Count > 0)
        {
            bodyParts[0].position = oldHeadPosition;
        }

        // Move head by 0.5 units
        transform.position += new Vector3(
        directionX * 0.5f,
        directionY * 0.5f,0);

        UpdateHeadVisual();
        UpdateTailVisual();

        CheckCollision();
        if (gameOver)
            return;

        CheckFood();
    }

    void CheckCollision()
    {
        // Check body collision
        foreach (Transform bodyPart in bodyParts)
        {
            if (transform.position == bodyPart.position)
            {
                GameOver();
                return;
            }
        }

        // Check walls
        if (transform.position.x < -7.5f ||
            transform.position.x > 7.5f ||
            transform.position.y < -3.5f ||
            transform.position.y > 3.5f)
        {
            GameOver();
        }
    }

    void CheckFood()
    {
        if (food == null)
        {
            Debug.LogError("Food is not assigned!");
            return;
        }

        float distance = Vector3.Distance(
            transform.position,
            food.position
        );

        if (distance < 0.6f)
        {
            score++;

            if (scoreText != null)
                scoreText.text = "Score: " + score;

            GrowSnake();

            food.position = new Vector3(
                Random.Range(-7, 8) * 0.5f,
                Random.Range(-3, 4) * 0.5f,
                0
            );

            Debug.Log("Food eaten! Score: " + score);
        }
    }

    void GrowSnake()
    {
        Transform newBodyPart = Instantiate(
            bodyPrefab,
            transform.position,
            Quaternion.identity
        );

        bodyParts.Add(newBodyPart);

        UpdateTailVisual();
    }

    void UpdateTailVisual()
    {
        if (bodyParts.Count == 0 || tailSprite == null)
            return;

        SpriteRenderer bodyRenderer =
            bodyPrefab.GetComponent<SpriteRenderer>();

        if (bodyRenderer == null)
            return;

        Sprite bodySprite = bodyRenderer.sprite;

        // Make all pieces normal body pieces
        foreach (Transform part in bodyParts)
        {
            SpriteRenderer sr =
                part.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                sr.sprite = bodySprite;
                sr.transform.rotation = Quaternion.identity;
            }
        }

        // Last piece becomes the tail
        Transform tail =
            bodyParts[bodyParts.Count - 1];

        SpriteRenderer tailRenderer =
            tail.GetComponent<SpriteRenderer>();

        if (tailRenderer == null)
            return;

        tailRenderer.sprite = tailSprite;

        Vector3 direction;

        if (bodyParts.Count == 1)
        {
            direction =
                tail.position - transform.position;
        }
        else
        {
            direction =
                tail.position -
                bodyParts[bodyParts.Count - 2].position;
        }

        RotateTail(tail, direction);
    }

    void RotateTail(Transform tail, Vector3 direction)
    {
        if (direction.x > 0)
        {
            tail.rotation =
                Quaternion.Euler(0, 0, 0);
        }
        else if (direction.x < 0)
        {
            tail.rotation =
                Quaternion.Euler(0, 0, 180);
        }
        else if (direction.y > 0)
        {
            tail.rotation =
                Quaternion.Euler(0, 0, 90);
        }
        else if (direction.y < 0)
        {
            tail.rotation =
                Quaternion.Euler(0, 0, -90);
        }
    }

    void GameOver()
    {
        gameOver = true;

        if (gameOverText != null)
            gameOverText.SetActive(true);

        if (restartButton != null)
            restartButton.SetActive(true);

        Debug.Log("Game Over!");
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
    void UpdateHeadVisual()
    {
        if (directionX == 1)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (directionX == -1)
            transform.rotation = Quaternion.Euler(0, 0, 180);
        else if (directionY == 1)
            transform.rotation = Quaternion.Euler(0, 0, 90);
        else if (directionY == -1)
            transform.rotation = Quaternion.Euler(0, 0, -90);
    }
}
