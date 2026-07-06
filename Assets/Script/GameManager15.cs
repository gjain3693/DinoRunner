using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager15 : MonoBehaviour
{
    [SerializeField] int score;

    public Text scoreText;

    public AudioManager audioManager;

    public GameObject restartButton;
    public GameObject adButton;
    public GameObject gameOver;

    public Player player;

    public DualSpawnner spawnner;
    public GameObject gameWin;
    public GameObject gameExit;

    public static GameManager15 instance;

    // Safe respawn position
    public Transform respawnPoint;

    public int NEXT_LEVEL_SCORE_REQ = 150;

    private void Start()
    {
        if (adButton != null)
        {
            Button btn = adButton.GetComponent<Button>();

            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();

                if (AdManager.Instance != null)
                {
                    btn.onClick.AddListener(
                        AdManager.Instance.ShowRewardedAdButton
                    );
                }
            }
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            gameObject.SetActive(true);

            Application.targetFrameRate = 60;

            if (restartButton != null)
                restartButton.SetActive(false);

            if (gameOver != null)
                gameOver.SetActive(false);

            if (adButton != null)
                adButton.SetActive(false);

            if (gameExit != null)
                gameExit.SetActive(false);

            Time.timeScale = 1f;

            if (player != null)
                player.enabled = true;
        }

        score = PlayerPrefs.GetInt("score", 0);

        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }

    public void Pause()
    {
        Time.timeScale = 0f;

        if (player != null)
        {
            player.enabled = false;
        }
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

        scoreText.text = score.ToString();

        if (score >= NEXT_LEVEL_SCORE_REQ)
        {
            GameWin();

            gameObject.SetActive(true);
            gameExit.SetActive(true);
            gameOver.SetActive(false);
            gameWin.SetActive(true);
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
            Debug.Log("Score ::: " + score);
        }



        // Resume game
        Time.timeScale = 1f;

        if (player != null)
        {
            player.enabled = true;
        }

        // Move player to respawn point
        if (respawnPoint != null && player != null)
        {
            player.transform.position =
                respawnPoint.position;
        }

        if (player != null)
        {
            // Reset physics
            Rigidbody2D rb =
                player.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;

                rb.angularVelocity = 0f;
            }
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

    public void GameWin()
    {
        audioManager =
            GameObject.FindAnyObjectByType<AudioManager>();

        if (audioManager != null)
        {
            if (audioManager.audioSource01 != null)
            {
                audioManager.audioSource01.Pause();
            }

            if (audioManager.audioSource04 != null)
            {
                audioManager.audioSource04.Play();
            }
        }

        if (gameWin != null)
            gameWin.SetActive(true);

        if (gameOver != null)
            gameOver.SetActive(false);

        if (restartButton != null)
            restartButton.SetActive(true);

        if (adButton != null)
            adButton.SetActive(true);

        Pause();
    }

}