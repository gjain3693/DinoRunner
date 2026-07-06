using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdManager : MonoBehaviour,
    IUnityAdsInitializationListener,
    IUnityAdsLoadListener,
    IUnityAdsShowListener
{
    public static AdManager Instance;

    [Header("Settings")]
    public string AndroidGameId = "6122428";

    public bool TestMode = true;

    [Header("Ad Units")]
    public string InterstitialAdId =
        "Interstitial_Android";

    public string RewardedAdId =
        "Rewarded_Android";

    private bool isInterstitialLoaded = false;

    private bool isRewardedLoaded = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        if (string.IsNullOrEmpty(AndroidGameId))
        {
            Debug.LogError(
                "Unity Ads Game ID is missing!"
            );

            return;
        }

        Advertisement.Initialize(
            AndroidGameId,
            TestMode,
            this
        );
    }

    // =========================
    // INITIALIZATION
    // =========================

    public void OnInitializationComplete()
    {
        // Debug.Log("Ads Initialized");

        LoadInterstitialAd();

        LoadRewardedAd();
    }

    public void OnInitializationFailed(
        UnityAdsInitializationError error,
        string message)

    {
        Invoke(nameof(ReInitializeAds), 10f);
        Debug.LogError(
            "Init Failed: " +
            error +
            " | " +
            message
        );
    }

    // =========================
    // LOAD ADS
    // =========================

    private void LoadInterstitialAd()
    {
        if (!string.IsNullOrEmpty(
            InterstitialAdId))
        {
            Advertisement.Load(
                InterstitialAdId,
                this
            );
        }
    }

    private void LoadRewardedAd()
    {
        if (!string.IsNullOrEmpty(
            RewardedAdId))
        {
            Advertisement.Load(
                RewardedAdId,
                this
            );
        }
    }

    public void OnUnityAdsAdLoaded(
        string adUnitId)
    {
        Debug.Log(
            "Loaded Ad: " + adUnitId
        );

        if (adUnitId == InterstitialAdId)
        {
            isInterstitialLoaded = true;
        }

        if (adUnitId == RewardedAdId)
        {
            isRewardedLoaded = true;
        }
    }


    public void OnUnityAdsFailedToLoad(
    string adUnitId,
    UnityAdsLoadError error,
    string message)
    {
        Debug.LogError(
            "Load Failed: " +
            adUnitId +
            " | " +
            error +
            " | " +
            message
        );

        if (adUnitId == RewardedAdId)
        {
            Invoke(
                nameof(LoadRewardedAd),
                5f
            );
        }
    }

    // =========================
    // BUTTON METHOD
    // =========================

    public void ShowRewardedAdButton()
    {
        Debug.Log(":::::Continue Button Clicked:::");
        ShowRewardAd();
    }

    // =========================
    // INTERSTITIAL AD
    // =========================

    public void ShowInterstitialAd()
    {
        if (!Advertisement.isInitialized)
        {
            Debug.LogWarning(
                "Ads not initialized yet"
            );

            return;
        }

        if (isInterstitialLoaded)
        {
            Debug.Log(
                "Showing Interstitial Ad"
            );

            Advertisement.Show(
                InterstitialAdId,
                this
            );

            isInterstitialLoaded = false;
        }
        else
        {
            Debug.Log(
                "Interstitial NOT ready yet"
            );
        }
    }

    // =========================
    // REWARDED AD
    // =========================


    public void ShowRewardAd()
{
    Debug.Log("Ads Initialized: " +
        Advertisement.isInitialized);

    Debug.Log("Rewarded Loaded: " +
        isRewardedLoaded);

    if (!Advertisement.isInitialized)
    {
        Debug.Log("Ads not initialized");

        Advertisement.Initialize(
            AndroidGameId,
            TestMode,
            this
        );

        // Fallback: continue game even if ads aren't ready
        ContinueCurrentLevel();

        return;
    }

    if (isRewardedLoaded)
    {
        Debug.Log(
            "Showing Rewarded Ad"
        );

        Advertisement.Show(
            RewardedAdId,
            this
        );

        isRewardedLoaded = false;
    }
    else
    {
        Debug.Log(
            "Rewarded Ad not ready. Reloading..."
        );

        LoadRewardedAd();

        // Fallback: continue game when no ad is available
        ContinueCurrentLevel();
    }
}

    // =========================
    // AD COMPLETED
    // =========================

    public void OnUnityAdsShowComplete(
        string adUnitId,
        UnityAdsShowCompletionState state)
    {
        // Rewarded Ad Completed
        if (adUnitId == RewardedAdId &&
            state ==
            UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log(
                "Rewarded Ad Completed"
            );

            ContinueCurrentLevel();
        }

        // Reload Ads
        if (adUnitId == RewardedAdId)
        {
            LoadRewardedAd();
        }

        if (adUnitId == InterstitialAdId)
        {
            LoadInterstitialAd();
        }
    }

    // =========================
    // CONTINUE GAME
    // =========================

    private void ContinueCurrentLevel()
    {
        Debug.Log("ContinueCurrentLevel Called");
        string currentScene =
            SceneManager
            .GetActiveScene()
            .name;

        Debug.Log(
            "Current Scene: " +
            currentScene
        );

        if (currentScene == "Level02")
        {
            FindFirstObjectByType<GameManager02>()
                .ContinueGame();
        }
        else if (currentScene == "Level03")
        {
            FindFirstObjectByType<GameManager03>()
                .ContinueGame();
        }
        else if (currentScene == "Level04")
        {
            FindFirstObjectByType<GameManager04>()
                .ContinueGame();
        }
        else if (currentScene == "Level05")
        {
            FindFirstObjectByType<GameManager05>()
                .ContinueGame();
        }
        else if (currentScene == "Level06")
        {
            FindFirstObjectByType<GameManager06>()
                .ContinueGame();
        }
        else if (currentScene == "Level07")
        {
            FindFirstObjectByType<GameManager07>()
                .ContinueGame();
        }
        else if (currentScene == "Level08")
        {
            FindFirstObjectByType<GameManager08>()
                .ContinueGame();
        }
        else if (currentScene == "Level09")
        {
            FindFirstObjectByType<GameManager09>()
                .ContinueGame();
        }
        else if (currentScene == "Level10")
        {
            FindFirstObjectByType<GameManager10>()
                .ContinueGame();
        }
        else if (currentScene == "Level11")
        {
            FindFirstObjectByType<GameManager11>()
                .ContinueGame();
        }
        else if (currentScene == "Level12")
        {
            FindFirstObjectByType<GameManager12>()
                .ContinueGame();
        }
        else if (currentScene == "Level13")
        {
            FindFirstObjectByType<GameManager13>()
                .ContinueGame();
        }
        else if (currentScene == "Level14")
        {
            FindFirstObjectByType<GameManager14>()
                .ContinueGame();
        }
        else if (currentScene == "Level15")
        {
            FindFirstObjectByType<GameManager15>()
                .ContinueGame();
        }
        else
        {
            FindFirstObjectByType<GameManager>()
                .ContinueGame();
        }
    }

    // =========================
    // SHOW EVENTS
    // =========================


    public void OnUnityAdsShowFailure(
    string adUnitId,
    UnityAdsShowError error,
    string message)
    {
        Debug.LogError(
            "Show Failed: " +
            adUnitId +
            " | " +
            error +
            " | " +
            message
        );

        if (adUnitId == RewardedAdId)
        {
            isRewardedLoaded = false;

            LoadRewardedAd();
        }
    }

    public void OnUnityAdsShowStart(
        string adUnitId)
    {
        Debug.Log(
            "Ad Started: " +
            adUnitId
        );
    }

    public void OnUnityAdsShowClick(
        string adUnitId)
    {
        Debug.Log(
            "Ad Clicked: " +
            adUnitId
        );
    }

    private void ReInitializeAds()
    {
        Advertisement.Initialize(
            AndroidGameId,
            TestMode,
            this
        );
    }
}