using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SocialPlatforms;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;

/// <summary>
/// Manages Unity Ads (rewarded video) and AdMob (interstitial) ad display.
/// Updated for Unity Ads 4.x API: uses IUnityAdsInitializationListener,
/// IUnityAdsLoadListener, and IUnityAdsShowListener instead of the
/// deprecated ShowOptions / resultCallback pattern.
/// </summary>
public class AdsControl : MonoBehaviour,
    IUnityAdsInitializationListener,
    IUnityAdsLoadListener,
    IUnityAdsShowListener
{
    protected AdsControl() { }

    private static AdsControl _instance;

    InterstitialAd interstitial;

    // Ad unit IDs — configured for ALTOMEDIA / CastleKingdomSeason
    private const string ADMOB_INTERSTITIAL_ANDROID = "ca-app-pub-6881903056221433/1893694801";
    private const string ADMOB_INTERSTITIAL_IOS     = "ca-app-pub-6881903056221433/1893694801";
    private const string ADMOB_REWARD_ANDROID       = "ca-app-pub-6881903056221433/2929896144";
    private const string UNITY_GAME_ID_ANDROID      = "6170475";
    private const string UNITY_GAME_ID_IOS          = "6170475";
    private const string UNITY_REWARDED_PLACEMENT   = "rewardedVideo";

    // Legacy serialized fields kept for inspector compatibility
    public string AdmobID_Android, AdmobID_IOS, UnityID_Android, UnityID_IOS, UnityZoneID;

    // Tracks whether a rewarded ad has been loaded and is ready to show
    private bool _rewardedLoaded = false;

    public static AdsControl Instance { get { return _instance; } }

    void Awake()
    {
        // Override serialized values with hardcoded constants
        AdmobID_Android = ADMOB_INTERSTITIAL_ANDROID;
        AdmobID_IOS     = ADMOB_INTERSTITIAL_IOS;
        UnityID_Android = UNITY_GAME_ID_ANDROID;
        UnityID_IOS     = UNITY_GAME_ID_IOS;
        UnityZoneID     = UNITY_REWARDED_PLACEMENT;

        if (FindObjectsOfType(typeof(AdsControl)).Length > 1) {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        MakeNewInterstial();
        DontDestroyOnLoad(gameObject);

        // Initialize Unity Ads 4.x
        if (Advertisement.isSupported) {
#if UNITY_IOS
            Advertisement.Initialize(UnityID_IOS, false, this);
#elif UNITY_ANDROID
            Advertisement.Initialize(UnityID_Android, false, this);
#endif
        }
    }

    // ─── AdMob interstitial ───────────────────────────────────────────────

    public void HandleInterstialAdClosed(object sender, EventArgs args)
    {
        if (interstitial != null)
            interstitial.Destroy();
        MakeNewInterstial();
    }

    void MakeNewInterstial()
    {
#if UNITY_ANDROID
        interstitial = new InterstitialAd(AdmobID_Android);
#elif UNITY_IOS || UNITY_IPHONE
        interstitial = new InterstitialAd(AdmobID_IOS);
#endif
        if (interstitial != null) {
            interstitial.OnAdClosed += HandleInterstialAdClosed;
            AdRequest request = new AdRequest.Builder().Build();
            interstitial.LoadAd(request);
        }
    }

    public void showAds()
    {
        if (interstitial != null)
            interstitial.Show();
    }

    // ─── Unity Ads 4.x — rewarded video ──────────────────────────────────

    /// <summary>Called by Unity Ads when initialization completes successfully.</summary>
    public void OnInitializationComplete()
    {
        Debug.Log("AdsControl: Unity Ads initialized.");
        LoadRewardedAd();
    }

    /// <summary>Called by Unity Ads when initialization fails.</summary>
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogWarning("AdsControl: Unity Ads init failed — " + error + ": " + message);
    }

    private void LoadRewardedAd()
    {
        _rewardedLoaded = false;
        Advertisement.Load(UNITY_REWARDED_PLACEMENT, this);
    }

    /// <summary>Called by Unity Ads when a placement finishes loading.</summary>
    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == UNITY_REWARDED_PLACEMENT) {
            Debug.Log("AdsControl: rewarded ad loaded.");
            _rewardedLoaded = true;
        }
    }

    /// <summary>Called by Unity Ads when a placement fails to load.</summary>
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogWarning("AdsControl: failed to load " + placementId + " — " + error + ": " + message);
        _rewardedLoaded = false;
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
        _rewardedLoaded = false;
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
        _rewardedLoaded = false;

        if (placementId == UNITY_REWARDED_PLACEMENT
            && showCompletionState == UnityAdsShowCompletionState.COMPLETED) {
            // Route reward to AdsRewardShop so it can track per-package progress
            if (AdsRewardShop.Instance != null)
                AdsRewardShop.Instance.OnAdWatched();
        } else {
            Debug.Log("AdsControl: ad " + showCompletionState + " — no reward granted.");
        }

        // Pre-load the next ad
        LoadRewardedAd();
    }

    // ─── Banner (not used in this game) ──────────────────────────────────
    public void HideBannerAds() { }
    public void ShowBannerAds() { }
}
