using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager02 : MonoBehaviour
{
    [SerializeField] int score;

    public Text scoreText;

    public AudioManager audioManager;

    public GameObject restartButton;
    public GameObject adButton;
    public GameObject gameOver;

    public Player player;

    public Spawnner spawnner;

    public static GameManager02 instance;

    // Safe respawn position
    public Transform respawnPoint;

    public int NEXT_LEVEL_SCORE_REQ = 20;

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
                "Button component missing."
            );
            return;
        }

        if (AdManager.Instance == null)
        {
            Debug.LogWarning(
                "AdManager not available."
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

            Time.timeScale = 1f;

            player.enabled = true;
        }

        // Load score EARLY
        score = PlayerPrefs.GetInt("score", 0);

        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
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
        Debug.Log("IncreaseScore Called");

        audioManager =
            GameObject.FindAnyObjectByType<AudioManager>();

        if (audioManager != null)
        {
            audioManager.audioSource03.Play();
        }

        score++;

        // Save latest score immediately
        PlayerPrefs.SetInt("score", score);

        PlayerPrefs.Save();

        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }

        if (score >= NEXT_LEVEL_SCORE_REQ)
        {
            if (Application.CanStreamedLevelBeLoaded("Level03"))
            {
                SceneManager.LoadScene("Level03");
            }
            else
            {
                Debug.LogError(
                    "Level03 missing from Build Settings."
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
            player.transform.position =
                respawnPoint.position;
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