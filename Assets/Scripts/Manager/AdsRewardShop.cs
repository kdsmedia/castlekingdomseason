using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Replaces IAP with an ads-based gem reward system.
/// Players watch reward ads to accumulate progress toward gem packages.
///
/// Packages:
///   Slot 0 :  20 Gems  after  50 ads watched
///   Slot 1 :  50 Gems  after 100 ads watched
///   Slot 2 : 100 Gems  after 250 ads watched
/// </summary>
public class AdsRewardShop : MonoBehaviour
{
    public static AdsRewardShop Instance { get; private set; }

    // ── Package definitions ──────────────────────────────────────────────────
    private static readonly int[] GemsReward  = {  20,  50, 100 };
    private static readonly int[] AdsRequired = {  50, 100, 250 };

    private const string KeyPrefix = "AdsWatched_";

    // ── Optional UI (assign in Inspector) ───────────────────────────────────
    /// <summary>One Text per package slot showing "X / Y ADS".</summary>
    public Text[] progressTexts;

    /// <summary>One Text per package slot showing gem amount (e.g. "20 Gems").</summary>
    public Text[] gemLabels;

    // ── Internal state ───────────────────────────────────────────────────────
    private int pendingPackageIndex = -1;

    // ────────────────────────────────────────────────────────────────────────
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        RefreshAllUI();
    }

    // ── Public API (call from UI buttons) ────────────────────────────────────

    /// <summary>
    /// Call this from a UI Button for each package slot (0 = 20 gems, 1 = 50 gems, 2 = 100 gems).
    /// </summary>
    public void WatchAdForPackage(int packageIndex)
    {
        if (packageIndex < 0 || packageIndex >= GemsReward.Length)
        {
            Debug.LogWarning("AdsRewardShop: invalid package index " + packageIndex);
            return;
        }
        pendingPackageIndex = packageIndex;
        AdsControl.Instance.ShowRewardVideo();
    }

    /// <summary>
    /// Called by AdsControl.HandleShowResult when a reward ad finishes successfully.
    /// </summary>
    public void OnAdWatched()
    {
        if (pendingPackageIndex < 0) return;

        int idx     = pendingPackageIndex;
        string key  = KeyPrefix + idx;
        int watched = PlayerPrefs.GetInt(key, 0) + 1;

        if (watched >= AdsRequired[idx])
        {
            // Threshold reached — award gems and reset counter
            PlayerPrefs.SetInt(key, 0);
            Menu_Manager mm = FindObjectOfType<Menu_Manager>();
            if (mm != null) mm.UpdateGems(GemsReward[idx]);
            Debug.Log("AdsRewardShop: awarded " + GemsReward[idx] + " gems for package " + idx);
        }
        else
        {
            PlayerPrefs.SetInt(key, watched);
            Debug.Log("AdsRewardShop: package " + idx + " progress " + watched + "/" + AdsRequired[idx]);
        }

        PlayerPrefs.Save();
        RefreshUI(idx);
        pendingPackageIndex = -1;
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    public int GetWatchedCount(int packageIndex) =>
        PlayerPrefs.GetInt(KeyPrefix + packageIndex, 0);

    public int GetRequiredAds(int packageIndex) => AdsRequired[packageIndex];
    public int GetGemsReward(int packageIndex)  => GemsReward[packageIndex];

    void RefreshAllUI()
    {
        for (int i = 0; i < GemsReward.Length; i++)
            RefreshUI(i);
    }

    void RefreshUI(int index)
    {
        // Progress text: "X / Y ADS"
        if (progressTexts != null && index < progressTexts.Length && progressTexts[index] != null)
        {
            int watched  = GetWatchedCount(index);
            int required = AdsRequired[index];
            progressTexts[index].text = watched + " / " + required + " ADS";
        }

        // Gem label: "20 Gems"
        if (gemLabels != null && index < gemLabels.Length && gemLabels[index] != null)
        {
            gemLabels[index].text = GemsReward[index] + " Gems";
        }
    }
}
