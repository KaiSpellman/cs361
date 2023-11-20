# cs361
Software Engineering 1
Fall term 2023<br>

To make HTTP requests to converter.py, implement the code below: <br>

```
import requests

def convert_currency(amount, from_currency, to_currency):
    url = "http://localhost:5000/convert"
    data = {
        "amount": amount,
        "from_currency": from_currency,
        "to_currency": to_currency
    }
    response = requests.post(url, json=data)
    if response.ok:
        return response.json()['converted_amount']
    else:
        return "Error: " + response.json().get('error', 'Unknown error')

def main():
    amount = float(input("Enter the amount you want to convert: "))
    from_currency = input("From Currency (e.g., 'USD', 'EUR'): ").upper()
    to_currency = input("To Currency (e.g., 'USD', 'EUR'): ").upper()
    result = convert_currency(amount, from_currency, to_currency)
    print(f"{amount} {from_currency} is {result} in {to_currency}")

if __name__ == "__main__":
    main()

```

To use online leaderboard microservice:

1. Make sure Flask is installed with: pip install Flask
2. Make sure Flask-SQLAlchemy is installed with: pip install Flask-SQLAlchemy
3. Add the provided leaderboardManager.cs file to the scripts folder
4. Make the provided modifications to the LogicScript.cs file in the scripts folder
5. Run the provided app.py file to start the server.

Example code for using microservice:
```
public class YourScript : MonoBehaviour
{
    private LeaderboardManager leaderboardManager;

    private void Start()
    {
        leaderboardManager = GetComponent<LeaderboardManager>();

        // Example: Get Leaderboard
        leaderboardManager.GetLeaderboard((response) =>
        {
            Debug.Log("Leaderboard Response: " + response);
            // Process the leaderboard data as needed
        });

        // Example: Add Score
        string playerName = "Player123";
        int score = 150;

        leaderboardManager.AddScore(playerName, score, (response) =>
        {
            Debug.Log("Add Score Response: " + response);
            // Process the response after adding the score
        });
    }
}
```
UML Sequence Diagram:
![cs361_UML](https://github.com/KaiSpellman/cs361/assets/128665815/f021bbfc-a0dc-4612-aa06-d7d5348c981e)
