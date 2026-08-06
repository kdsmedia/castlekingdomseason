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

## Running the project

This is a Unity project and must be opened in **Unity 2017.1.0f3** (or a compatible version).  
Replit is used for browsing and editing source files only — builds happen in the Unity Editor.

## Key files

- `Assets/Scripts/Manager/AdsControl.cs` — ad initialization and display logic
- `Assets/Scripts/Manager/` — game managers (menu, level, etc.)
- `Assets/Scenes/` — Unity scenes
- `ProjectSettings/ProjectSettings.asset` — app name, bundle ID, company name

## User preferences

- Keep existing Unity project structure and stack unchanged.
- Do not migrate or restructure the project.
