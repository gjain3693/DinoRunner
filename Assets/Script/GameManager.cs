using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int score = 0;

    public Text scoreText;

    public GameObject playButton;
    public GameObject restartButton;
    public GameObject adButton;
    public GameObject gameOver;

    public static GameManager instance;

    public Player player;
    public AudioManager audioManager;
    public Spawnner spawnner;

    // Safe respawn position
    public Transform respawnPoint;

    public int NEXT_LEVEL_SCORE_REQ = 10;



    private void Start()
    {
        if (adButton == null)
        {
            Debug.LogError(
                "Ad Button reference missing."
            );
            return;
        }

        Button btn =
            adButton.GetComponent<Button>();

        if (btn == null)
        {
            Debug.LogError(
                "Button component missing on Ad Button."
            );
            return;
        }

        if (AdManager.Instance == null)
        {
            Debug.LogWarning(
                "AdManager not found."
            );
            return;
        }

        btn.onClick.RemoveAllListeners();

        btn.onClick.AddListener(
            AdManager.Instance.ShowRewardedAdButton
        );
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            gameObject.SetActive(true);

            Application.targetFrameRate = 60;

            restartButton.SetActive(false);
            gameOver.SetActive(false);
            adButton.SetActive(false);

            Pause();
        }
        else
        {
            instance = this;
        }
    }

    public void Play()
    {
        score = 0;

        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }

        Time.timeScale = 1f;

        player.enabled = true;

        playButton.SetActive(false);
        gameOver.SetActive(false);
        restartButton.SetActive(false);
        adButton.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        if (Application.CanStreamedLevelBeLoaded("MainMenu"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.LogError(
                "MainMenu missing from Build Settings."
            );
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;

        player.enabled = false;
    }

    public void IncreaseScore()
    {
        audioManager = GameObject.FindAnyObjectByType<AudioManager>();

        if (audioManager != null)
        {
            audioManager.audioSource03.Play();
        }

        score++;

        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }

        if (score == NEXT_LEVEL_SCORE_REQ)
        {
            PlayerPrefs.SetInt("score", score);
            PlayerPrefs.Save();

            if (Application.CanStreamedLevelBeLoaded("Level02"))
            {
                SceneManager.LoadScene("Level02");
            }
            else
            {
                Debug.LogError(
                    "Level02 missing from Build Settings."
                );
            }

            gameObject.SetActive(true);

            gameOver.SetActive(false);
        }
    }



    public void GameOver()
    {
        // Save latest score
        PlayerPrefs.SetInt("score", score);

        PlayerPrefs.SetInt(
            "level",
            SceneManager.GetActiveScene().buildIndex
        );

        PlayerPrefs.Save();

        audioManager =
            GameObject.FindAnyObjectByType<AudioManager>();

        if (audioManager != null)
        {
            audioManager.audioSource01.Pause();

            audioManager.audioSource02.Play();
        }

        // Stop spawner safely
        if (spawnner != null)
        {
            spawnner.gameObject.SetActive(false);
        }

        gameOver.SetActive(true);

        restartButton.SetActive(true);

        adButton.SetActive(true);

        Pause();
    }

    public void ContinueGame()
    {
       
        // Restore latest saved score
        int savedScore =
            PlayerPrefs.GetInt("score", 0);

        score = savedScore;

        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }

        Debug.Log("Score ::: " + score);

        // Resume game
        Time.timeScale = 1f;

        player.enabled = true;

        // Move player to respawn point
        if (respawnPoint != null)
        {
            Debug.Log("Before Respawn: " + player.transform.position);
            player.transform.position =
                respawnPoint.position;
            Debug.Log("After Respawn: " + player.transform.position);
        }

        // Reset physics
        Rigidbody2D rb =
            player.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;

            rb.angularVelocity = 0f;
        }

        // Resume background music
        audioManager =
            GameObject.FindAnyObjectByType<AudioManager>();

        if (audioManager != null)
        {
            audioManager.audioSource01.UnPause();
        }

        // Hide UI
        gameOver.SetActive(false);

        adButton.SetActive(false);

        restartButton.SetActive(false);

        // Temporary protection
        StartCoroutine(TemporaryInvincibility());

        // Clear old obstacles
        if (spawnner != null)
        {
            spawnner.ClearObstacles();


            StartCoroutine(
                EnableSpawnerAfterDelay()
            );
        }
    }
    private IEnumerator EnableSpawnerAfterDelay()
    {
        // Wait 2 seconds
        yield return new WaitForSecondsRealtime(2f);

        if (spawnner != null)
        {
            // Re-enable spawner
            spawnner.gameObject.SetActive(true);

            // Skip immediate obstacle
            spawnner.SkipNext();
        }
    }

    private IEnumerator TemporaryInvincibility()
    {
        player.isInvincible = true;

        yield return new WaitForSeconds(2f);

        player.isInvincible = false;
    }
}