using UnityEngine;
using UnityEngine.Advertisements;

/// <summary>
/// Manages interstitial and rewarded video ads through Unity Ads 4.x.
/// Uses IUnityAdsInitializationListener, IUnityAdsLoadListener and
/// IUnityAdsShowListener instead of the deprecated ShowOptions / resultCallback
/// pattern.
/// </summary>
public class AdsControl : MonoBehaviour,
    IUnityAdsInitializationListener,
    IUnityAdsLoadListener,
    IUnityAdsShowListener
{
    protected AdsControl() { }

    private static AdsControl _instance;

    // Unity Ads configuration for ALTOMEDIA / CastleKingdomSeason
    private const string UNITY_GAME_ID_ANDROID        = "6170475";
    private const string UNITY_GAME_ID_IOS            = "6170475";
    private const string UNITY_REWARDED_PLACEMENT     = "rewardedVideo";
    private const string UNITY_INTERSTITIAL_PLACEMENT = "Interstitial_Android";

    // Legacy serialized fields kept for inspector compatibility
    public string AdmobID_Android, AdmobID_IOS, UnityID_Android, UnityID_IOS, UnityZoneID;

    // Tracks whether each placement is loaded and ready to show
    private bool _rewardedLoaded = false;
    private bool _interstitialLoaded = false;

    public static AdsControl Instance { get { return _instance; } }

    void Awake()
    {
        UnityID_Android = UNITY_GAME_ID_ANDROID;
        UnityID_IOS     = UNITY_GAME_ID_IOS;
        UnityZoneID     = UNITY_REWARDED_PLACEMENT;

        if (FindObjectsOfType(typeof(AdsControl)).Length > 1) {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        if (Advertisement.isSupported) {
#if UNITY_IOS
            Advertisement.Initialize(UnityID_IOS, false, this);
#elif UNITY_ANDROID
            Advertisement.Initialize(UnityID_Android, false, this);
#endif
        }
    }

    // ─── Initialization ──────────────────────────────────────────────────

    /// <summary>Called by Unity Ads when initialization completes successfully.</summary>
    public void OnInitializationComplete()
    {
        Debug.Log("AdsControl: Unity Ads initialized.");
        LoadRewardedAd();
        LoadInterstitialAd();
    }

    /// <summary>Called by Unity Ads when initialization fails.</summary>
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogWarning("AdsControl: Unity Ads init failed — " + error + ": " + message);
    }

    // ─── Loading ─────────────────────────────────────────────────────────

    private void LoadRewardedAd()
    {
        _rewardedLoaded = false;
        Advertisement.Load(UNITY_REWARDED_PLACEMENT, this);
    }

    private void LoadInterstitialAd()
    {
        _interstitialLoaded = false;
        Advertisement.Load(UNITY_INTERSTITIAL_PLACEMENT, this);
    }

    /// <summary>Called by Unity Ads when a placement finishes loading.</summary>
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("AdsControl: ad loaded — " + placementId);

        if (placementId == UNITY_REWARDED_PLACEMENT)
            _rewardedLoaded = true;
        else if (placementId == UNITY_INTERSTITIAL_PLACEMENT)
            _interstitialLoaded = true;
    }

    /// <summary>Called by Unity Ads when a placement fails to load.</summary>
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogWarning("AdsControl: failed to load " + placementId + " — " + error + ": " + message);

        if (placementId == UNITY_REWARDED_PLACEMENT)
            _rewardedLoaded = false;
        else if (placementId == UNITY_INTERSTITIAL_PLACEMENT)
            _interstitialLoaded = false;
    }

    // ─── Showing ─────────────────────────────────────────────────────────

    /// <summary>Shows an interstitial between levels, if one is ready.</summary>
    public void showAds()
    {
        if (_interstitialLoaded)
            Advertisement.Show(UNITY_INTERSTITIAL_PLACEMENT, this);
        else
            LoadInterstitialAd();
    }

    /// <returns>True if a rewarded ad is loaded and ready to show.</returns>
    public bool GetRewardAvailable()
    {
        return _rewardedLoaded;
    }

    public void ShowRewardVideo()
    {
        if (!_rewardedLoaded) {
            Debug.LogWarning("AdsControl: rewarded ad not ready yet.");
            return;
        }
        Advertisement.Show(UNITY_REWARDED_PLACEMENT, this);
    }

    // ─── IUnityAdsShowListener callbacks ─────────────────────────────────

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogWarning("AdsControl: show failed — " + error + ": " + message);

        if (placementId == UNITY_INTERSTITIAL_PLACEMENT)
            LoadInterstitialAd();
        else
            LoadRewardedAd();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("AdsControl: ad started — " + placementId);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("AdsControl: ad clicked — " + placementId);
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == UNITY_INTERSTITIAL_PLACEMENT) {
            LoadInterstitialAd();
            return;
        }

        _rewardedLoaded = false;

        if (placementId == UNITY_REWARDED_PLACEMENT
            && showCompletionState == UnityAdsShowCompletionState.COMPLETED) {
            // Route reward to AdsRewardShop so it can track per-package progress
            if (AdsRewardShop.Instance != null)
                AdsRewardShop.Instance.OnAdWatched();
        } else {
            Debug.Log("AdsControl: ad " + showCompletionState + " — no reward granted.");
        }

        LoadRewardedAd();
    }

    // ─── Banner (not used in this game) ──────────────────────────────────
    public void HideBannerAds() { }
    public void ShowBannerAds() { }
}
