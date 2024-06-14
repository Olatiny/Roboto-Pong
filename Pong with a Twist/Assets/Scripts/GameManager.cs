using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Game manager is a singleton to make it easy to access from other classes
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<GameManager>();
                    Debug.Log("Generating new GameManager");
                }
            }
            return _instance;
        }
    }

    // Public & Serialized private fields for the player & relevant gameplay fields
    [Header("Players & Gameplay")]
    public Paddle player1;
    public Paddle player2;

    public int player1Score = 0;
    public int player2Score = 0;

    public int winningScore = 11;

    [SerializeField]
    private GameObject ballPrefab;

    [SerializeField] 
    private List<GameObject> asteroidPrefabs;

    [SerializeField]
    private float restartTime = 3f;

    [SerializeField]
    private float maxAsteroids = 20f;

    // Serialized fields for UI objects
    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI player1ScoreText;

    [SerializeField]
    private TextMeshProUGUI player2ScoreText;

    [SerializeField]
    private TextMeshProUGUI gameOverText;

    public bool gameOver = false;

    private float nextAsteroidSpawnTime = 5f;

    private float timeSinceLastAsteroidSpawn = 0f;

    // Handles the spawning of asteroids. Don't spawn more than maxAsteroids every nextAstroidSpawnTime. 
    void Update()
    {
        if (gameOver)
            return;

        timeSinceLastAsteroidSpawn += Time.deltaTime;

        if (timeSinceLastAsteroidSpawn > nextAsteroidSpawnTime && GameObject.FindGameObjectsWithTag("Asteroid").Length < maxAsteroids)
        {
            int numToSpawn = Random.Range(2, 7);

            for (int i = 0; i < numToSpawn; i++)
            {
                // Instantiate an asteroid somewhere in a 4x4 grid of a random type
                Instantiate(
                    asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count - 1)], 
                    new Vector2(Random.Range(-4, 4), Random.Range(-4, 4)), 
                    transform.rotation
                );
            }

            timeSinceLastAsteroidSpawn = 0f;
        }
    }

    // Adds score to the relevant player based on the input player ID. 
    public void AddScore(int score, int playerId)
    {
        switch (playerId)
        {
            case 0:
                player1Score += score;
                break;
            case 1:
                player2Score += score;
                break;
        }

        // Destroy all asteroids currently in play
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        foreach (GameObject asteroid in asteroids)
        {
            Destroy(asteroid);
        }

        timeSinceLastAsteroidSpawn = 0f;

        UpdateScoreText();

        // Check if a player has won.
        if (!CheckWin())
            Instantiate(ballPrefab, Vector3.zero, transform.rotation);
    }

    // Getter to help with identifying the currently winning player.
    public int GetWinningPlayerId()
    {
        return player1Score > player2Score ? player1.playerId : player2.playerId;
    }

    // Checks if a player has won. If one has, GameOver is called.
    bool CheckWin()
    {
        if (player1Score >= winningScore)
        {
            GameOver(player1.playerId);
            return true;
        }
        else if (player2Score >= winningScore)
        {
            GameOver(player2.playerId);
            return true;
        }

        return false;
    }

    // Updates the score text for each of the players
    void UpdateScoreText()
    {
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }

    // Called when gameover is reached. Starts the game over coroutine. 
    void GameOver(int playerId)
    {
        StartCoroutine(GameOverCoroutine(playerId));
    }

    // Coroutine that's called on GameOver. Lets the players know who won and,
    // after restartTime seconds, resets the board and lets the players start over
    IEnumerator GameOverCoroutine(int playerId)
    {
        gameOver = true;

        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "Player " + (playerId + 1) + " wins!";

        yield return new WaitForSeconds(restartTime);

        gameOverText.gameObject.SetActive(false);
        player1Score = player2Score = 0;
        timeSinceLastAsteroidSpawn = 0;

        UpdateScoreText();
        Instantiate(ballPrefab, Vector3.zero, transform.rotation);

        gameOver = false;
    }
}
