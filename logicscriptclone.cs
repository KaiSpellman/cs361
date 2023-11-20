using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class LogicScript : MonoBehaviour
{
    public int playerScore;
    public Text scoreText;
    public GameObject gameOverScreen;
    public bool isGameOver = false;
    public AudioSource gameOverAudio;

    [ContextMenu("Increase Score")]
    public void addScore(int scoreToAdd)
    {
        playerScore = playerScore + scoreToAdd;
        scoreText.text = playerScore.ToString();
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOver()
    {
        gameOverScreen.SetActive(true);
        gameOverAudio.Play();
        isGameOver = true;

        SaveHighScore();
    }

    private void SaveHighScore()
    {
        int highScore = PlayerPrefs.GetInt("Highscore", 0);
        if (playerScore > highScore)
        {
            PlayerPrefs.SetInt("Highscore", playerScore);
            PlayerPrefs.Save();

            // Send new high score to db
            StartCoroutine(SendScoreToDatabase(playerScore, (response) =>
            {
                Debug.Log(response);
                // Handle the response as needed
            }));
        }

    }

    // Coroutine to send score to db
    private const string serverURL = "http://127.0.0.1:5000/leaderboard"; // Test server URL

    private IEnumerator SendScoreToDatabase(int score, System.Action<string> callback)
    {
        // Create a JSON object with only the score
        string json = $"{{\"score\":{score}}}";

        // Create a POST request to send the score to the server
        UnityWebRequest request = UnityWebRequest.PostWwwForm(serverURL, "POST");
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();

        // Set content type header
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error sending score: {request.error}");
            callback($"Error: {request.error}");
        }
        else
        {
            Debug.Log("Score sent successfully");
            callback("Score sent successfully");

            Start(); //call test function
        }
    }

    private LeaderboardManager leaderboardManager;

    // Test function
    private void Start()
    {
        leaderboardManager = FindObjectOfType<LeaderboardManager>();

        if (leaderboardManager != null)
        {
            // Example: Get Leaderboard
            leaderboardManager.GetLeaderboard((response) =>
            {
                Debug.Log("Leaderboard Response: " + response);
                // Process the leaderboard data as needed
            });
        }
        else
        {
            Debug.LogError("LeaderboardManager not found in the scene.");
        }
    }

}