# CastleKingdomSeason

A Unity 2017.1.0f3 mobile tower-defense game developed by **ALTOMEDIA**.

## Project overview

- **App name:** CastleKingdomSeason
- **Package name:** com.altomedia.castlekingdomseason
- **Developer:** ALTOMEDIA
- **Contact:** altomediaindonesia@gmail.com

## Ad configuration

| SDK | Placement | ID |
|-----|-----------|-----|
| AdMob | App ID | ca-app-pub-6881903056221433~6071064804 |
| AdMob | Interstitial (Android) | ca-app-pub-6881903056221433/1893694801 |
| AdMob | Rewarded video | ca-app-pub-6881903056221433/2929896144 |
| Unity Ads | Game ID (Android) | 6170475 |
| Unity Ads | Rewarded placement | rewardedVideo |

Ad IDs are hardcoded as constants in `Assets/Scripts/Manager/AdsControl.cs`.  
The AdMob App ID is declared in `Assets/Plugins/Android/GoogleMobileAdsPlugin/AndroidManifest.xml`.

## Android SDK requirements (Play Console compliant)

| Setting | Value |
|---------|-------|
| `minSdkVersion` | 24 (Android 7.0) |
| `targetSdkVersion` | 35 (Android 15) |
| Google Mobile Ads (AdMob) | 23.2.0 — declared in `mainTemplate.gradle` |
| Unity Ads | 4.12.1 — declared in `mainTemplate.gradle` |

Dependencies are pulled from Maven at build time via `Assets/Plugins/Android/mainTemplate.gradle`.  
Old static AARs (play-services-ads 11.0.4, support library 25.x, UnityAds 2.x) have been removed.

> **⚠️ One manual step remaining:** `Assets/UnityAds/*.dll` are the C# wrapper DLLs for Unity Ads 2.x and must be replaced with the Unity Ads 4.x unitypackage. Download from [https://github.com/Unity-Technologies/unity-ads-android/releases](https://github.com/Unity-Technologies/unity-ads-android/releases) and import into the Unity project.

## Running the project

This is a Unity project and must be opened in **Unity 2017.1.0f3** (or a compatible version).  
Replit is used for browsing and editing source files only — builds happen in the Unity Editor.

### First-time build setup
1. Open the project in Unity 2017.1.0f3
2. Go to **Player Settings → Publishing Settings** and enable **Custom Gradle Template**
3. Run **Assets → Play Services Resolver → Android Resolver → Force Resolve** to download Maven dependencies
4. Replace `Assets/UnityAds/*.dll` with Unity Ads 4.x (see note above)
5. Build APK — Gradle will pull AdMob 23.2.0 and Unity Ads 4.12.1 automatically

## Key files

- `Assets/Scripts/Manager/AdsControl.cs` — ad initialization and display logic
- `Assets/Scripts/Manager/EnemyAIMovement.cs` — enemy AI movement (renamed from Test.cs)
- `Assets/Scripts/Manager/` — game managers (menu, level, etc.)
- `Assets/Scenes/` — Unity scenes
- `Assets/Plugins/Android/mainTemplate.gradle` — Android build config and SDK dependencies
- `ProjectSettings/ProjectSettings.asset` — app name, bundle ID, company name

## User preferences

- Keep existing Unity project structure and stack unchanged.
- Do not migrate or restructure the project.
