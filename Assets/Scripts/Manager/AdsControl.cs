using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.SocialPlatforms;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;

public class AdsControl : MonoBehaviour
{
	
	
	protected AdsControl ()
	{
	}

	private static AdsControl _instance;


	ShowOptions options;
	InterstitialAd interstitial;


	// Ad unit IDs — configured for ALTOMEDIA / CastleKingdomSeason
	private const string ADMOB_INTERSTITIAL_ANDROID  = "ca-app-pub-6881903056221433/1893694801";
	private const string ADMOB_INTERSTITIAL_IOS      = "ca-app-pub-6881903056221433/1893694801";
	private const string ADMOB_REWARD_ANDROID        = "ca-app-pub-6881903056221433/2929896144";
	private const string UNITY_GAME_ID_ANDROID       = "6170475";
	private const string UNITY_GAME_ID_IOS           = "6170475";
	private const string UNITY_REWARDED_PLACEMENT    = "rewardedVideo";

	// Legacy serialized fields kept for backward compatibility (values overridden in Awake)
	public string AdmobID_Android, AdmobID_IOS, UnityID_Android, UnityID_IOS, UnityZoneID;

	public static AdsControl Instance { get { return _instance; } }

	void Awake ()
	{
		// Override serialized values with hardcoded constants
		AdmobID_Android = ADMOB_INTERSTITIAL_ANDROID;
		AdmobID_IOS     = ADMOB_INTERSTITIAL_IOS;
		UnityID_Android = UNITY_GAME_ID_ANDROID;
		UnityID_IOS     = UNITY_GAME_ID_IOS;
		UnityZoneID     = UNITY_REWARDED_PLACEMENT;

		if (FindObjectsOfType (typeof(AdsControl)).Length > 1) {
			Destroy (gameObject);
			return;
		}
		
		_instance = this;
		MakeNewInterstial ();

		
		DontDestroyOnLoad (gameObject); //Already done by CBManager



		if (Advertisement.isSupported) { // If the platform is supported,
			#if UNITY_IOS
			Advertisement.Initialize (UnityID_IOS); // initialize Unity Ads.
			#endif

			#if UNITY_ANDROID
			Advertisement.Initialize (UnityID_Android); // initialize Unity Ads.
			#endif
		}
		options = new ShowOptions ();
		options.resultCallback = HandleShowResult;



	}


	public void HandleInterstialAdClosed (object sender, EventArgs args)
	{
	


		if (interstitial != null)
			interstitial.Destroy ();
		MakeNewInterstial ();
	

		
	}

	void MakeNewInterstial ()
	{


#if UNITY_ANDROID
		interstitial = new InterstitialAd (AdmobID_Android);
#endif
#if UNITY_IPHONE
		interstitial = new InterstitialAd (AdmobID_IOS);
#endif
		interstitial.OnAdClosed += HandleInterstialAdClosed;
		AdRequest request = new AdRequest.Builder ().Build ();
		interstitial.LoadAd (request);

	
	}


	public void showAds ()
	{
		

		interstitial.Show ();
	
	

	}


	public bool GetRewardAvailable ()
	{
		return Advertisement.IsReady (UnityZoneID);
	}

	public void ShowRewardVideo ()
	{

		Advertisement.Show (UnityZoneID, options);
	
		
	}

	public void HideBannerAds ()
	{
	}

	public void ShowBannerAds ()
	{
	}

	private void HandleShowResult (ShowResult result)
	{
		switch (result) {
		case ShowResult.Finished:
			// Route reward to AdsRewardShop so it can track per-package progress
			if (AdsRewardShop.Instance != null)
				AdsRewardShop.Instance.OnAdWatched ();
			break;
		case ShowResult.Skipped:
			Debug.Log ("AdsControl: reward ad skipped.");
			break;
		case ShowResult.Failed:
			Debug.Log ("AdsControl: reward ad failed.");
			break;
		}
	}

}

