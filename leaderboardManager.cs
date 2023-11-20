using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LeaderboardManager : MonoBehaviour
{
    private const string leaderboardURL = "http://127.0.0.1:5000/leaderboard";

    public void GetLeaderboard(System.Action<string> callback)
    {
        StartCoroutine(GetRequest(leaderboardURL, callback));
    }

    public void AddScore(string playerName, int score, System.Action<string> callback)
    {
        StartCoroutine(PostRequest(leaderboardURL, playerName, score, callback));
    }

    private IEnumerator GetRequest(string url, System.Action<string> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            callback($"Error: {request.error}");
        }
        else
        {
            callback(request.downloadHandler.text);
        }
    }

    private IEnumerator PostRequest(string url, string playerName, int score, System.Action<string> callback)
    {
        if (playerName == null)
        {
            // Set a default for the absence of playerName
            playerName = "DefaultPlayer";
        }

        string json = $"{{\"player_name\":\"{playerName}\",\"score\":{score}}}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = UnityWebRequest.PostWwwForm(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            callback($"Error: {request.error}");
        }
        else
        {
            callback(request.downloadHandler.text);
        }
    }

}
