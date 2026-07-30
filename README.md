An Among Us API that allows mods to add their own achievements!

# Features:
- Easy implementation
- Progressable achievements
- An achievement menu
- Lots of customization (coming soon)
<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/672fa262-1b7c-416a-9ba4-e26f4c82a10b" />
<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/fc0b6b90-e3d1-4753-92b3-a348b8add78f" />

For support, or just chatting, join the [discord](https://discord.gg/RJyNm9UaT7)!

>[!NOTE]
>**This mod is not affiliated with Among Us or Innersloth LLC, and the content contained therein is not endorsed or otherwise sponsored by Innersloth LLC. Portions of the materials contained herein are property of Innersloth LLC. © Innersloth LLC.**

> Will probably hopefully be used in:
> - Stargazer
> - NewMod

> Thanks [pix](https://github.com/wanderingpix) for the help!

# Get Started
To start using Achievements API, you need to:
- Add a reference to AchievementsAPI either through a DLL or project reference.
- Add a BepInDependency or SoftDependency on your plugin class like this:
 - You will also need to add a Dependency to VentFramework, due to this branches' use of it.
> `[BepInDependency(AchievementsAPIPlugin.Id)]`\
or <br />
> `[BepInDependency(AchievementsAPIPlugin.Id, BepInDependency.DependencyFlags.SoftDependency)]`
> and <br />
> `[BepInDependency(Vents.Id)]` (VentFramework)


## Creating An Achievements Tab
To have a tab for your achievements to be stored in, you need to create a class implementing `AchievementsTab`.\
`AchievementsTab` has the following members that need to be implemented for your Achievements to function correctly:

| Member                       | Required | Default                                                                        | Description                                                                                                    |
|------------------------------|:--------:|--------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------|
| `string Name { get; }`       |   Yes    | —                                                                              | The name of the Achievements Tab, displayed in UI.                                                             |
| `bool IsSelectable { get; }` |    No    | `true`                                                                         | Whether the tab should be selectable in the Achievements Menu. Handy for custom UI's or internal achievements. |
| `Color GetTabColor()`        |    No    | `new Color32(255, 255, 150, 255)`                                              | A method which gets the color of the Achievements Menu background when switching to this tab.                  |
| `string IconPath { get; }`   |    No    | `"AchievementsAPI.Resources.ExampleIcon.png"`                                  | The path to the tab's icon, for if you don't have it as a Sprite.                                              |
| `Sprite GetIcon()`           |    No    | `SpriteTools.LoadSpriteFromPath(IconPath, Assembly.GetCallingAssembly(), 100)` | A method which gets the icon of the tab, used for its icon in the Achievements Menu.                           |

### Example:
```cs
using AchievementsAPI.API;
using UnityEngine;

namespace ExampleAssembly.Achievements;

public class ExampleAchievementsTab : AchievementsTab
{
    public override string Name => "Example";
    public override bool IsSelectable => true;
    
    public override Color GetTabColor() => return new Color32(255, 255, 150, 255);
    public override Sprite GetIcon() => "AchievementsAPI.Resources.ExampleIcon.png";
}
```

## Creating Achievements
### Single Unlock Achievements:
For each single unlock achievement (requires one thing to unlock, no progress) you need to create a `BaseAchievement`.
`BaseAchievement` has the following members that need to be implemented for your Achievement to function correctly:

| Member                                                           | Required | Default                                | Description                                                                                                                                                                              |
|------------------------------------------------------------------|----------|----------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `string Name`                                                    | Yes      | —                                      | The achievement's name.                                                                                                                                                                  |
| `string Description`                                             | Yes      | —                                      | The achievement's description.                                                                                                                                                           |
| `string IconPath`                                                | Yes*     | —                                      | The achievement's icon's path. *Use this constructor overload or provide a `Sprite Icon` directly.                                                                                       |
| `Sprite Icon`                                                    | Yes*     | —                                      | The achievement's icon. *Use this constructor overload or provide an `IconPath` directly.                                                                                                |
| `int Rarity`                                                     | No       | `0`                                    | The achievement's rarity: `0` = common (default), `1` = rare (blue), `2` = epic (purple), `3` = legendary (yellow).                                                                      |
| `bool Hidden`                                                    | No       | `false`                                | Whether the achievement is hidden or not (hidden achievements get the default icon and have their name and description set to "Hidden Achievement" until unlocked).                      |
| `bool HideRarity`                                                | No       | `true`                                 | Whether to hide the achievement's rarity (if the achievement is hidden).                                                                                                                 |
| `Assembly? Assembly`                                             | No       | `Assembly.GetCallingAssembly()`        | The assembly associated with the achievement, used to generate its `Id`.                                                                                                                 |
| `bool Unlocked { get; }`                                         | No       | `false`                                | Whether the achievement has been unlocked.                                                                                                                                               |
| `string Id { get; }`                                             | No       | `Assembly.GetName().Name + "_" + Name` | The achievement's unique identifier.                                                                                                                                                     |
| `void Unlock(bool showOnUI = true, bool doStorageUpdate = true)` | No       | —                                      | Unlocks the achievement. `showOnUI` shows an unlock animation on the HUD. `doStorageUpdate` indicates whether to update storage again, used to make `CountAchievements` properly update. |

**Example:**
```cs
public BaseAchievement Welcome { get; set; } = new BaseAchievement(
    "Welcome!", "Launch the game with Achievements API installed.", "AchievementsAPI.Resources.ExampleIcon.png"
);
```

### Count Unlock Achievements:
For each count unlock achievement (requires progression to unlock) you need to create a `CountAchievement`.
`CountAchievement` has the following members that need to be implemented for your Achievement to function correctly:

| Member                                            | Required | Default | Description                                                                                                                                                         |
|---------------------------------------------------|----------|---------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `int CurrentValue`                                | Yes      | —       | The current progress for this achievement.                                                                                                                          |
| `int RequiredValue`                               | Yes      | —       | The required progress to unlock this achievement.                                                                                                                   |
| `bool ProgressPersists`                           | No       | `true`  | Defines if the progress persists between games.                                                                                                                     |
| `int Rarity`                                      | No       | `0`     | The achievement's rarity: `0` = common (default), `1` = rare (blue), `2` = epic (purple), `3` = legendary (yellow).                                                 |
| `bool Hidden`                                     | No       | `false` | Whether the achievement is hidden or not (hidden achievements get the default icon and have their name and description set to "Hidden Achievement" until unlocked). |
| `bool HideRarity`                                 | No       | `true`  | Whether to hide the achievement's rarity (if the achievement is hidden).                                                                                            |
| `bool HideProgress`                               | No       | `false` | Whether to hide the achievement's progress (if the achievement is hidden).                                                                                          |
| `void Increment(int count, bool showOnUI = true)` | No       | —       | Increments the progress of this achievement by `count`. `showOnUI` shows an unlock animation on the HUD.                                                            |
| `void SetValue(int value, bool showOnUI = true)`  | No       | —       | Sets the progress of this achievement to `value`. `showOnUI` shows an unlock animation on the HUD.                                                                  |

**Example:**
```cs
public CountAchievement Taskmaster { get; set; } = new CountAchievement(
    "Taskmaster", "Do 5 tasks", "AchievementsAPI.Resources.ExampleIcon.png", 0, 5
);
```

## Awarding Achievements
To award an achievement, you need to find your achievements tab, via `AchievementsTabSingleton<ToexAchievementsTab>.Instance`.\
Once you have this instance, you can use it to access your achievements:

### Unlocking `BaseAchievement`s
To unlock a `BaseAchievement`, you can use `achievement.Unlock();`, for example:
`AchievementsTabSingleton<ToexAchievementsTab>.Instance.Welcome.Unlock();`

### Progressing `CountAchievement`s
To unlock a `CountAchievement`, you can use `achievement.Increment(Amount);` or `chievement.SetValue(Amount);`, for example:
`AchievementsTabSingleton<ToexAchievementsTab>.Instance.Taskmaster.Increment(1);` would add 1 to the achievement progress\
and\
`AchievementsTabSingleton<ToexAchievementsTab>.Instance.Taskmaster.SetValue(10);` would immediately unlock the achievement

**Examples:**
```cs
// Unlocking BaseAchievements
[HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Awake))]
[HarmonyPatch(Priority.Last)]
[HarmonyPostfix]
public static void OnMainMenuAwakePostfix(MainMenuManager __instance)
{
    AchievementsTabSingleton<ExampleTab>.Instance.Welcome.Unlock();
}

// Progressing CountAchievements
[HarmonyPostfix]
[HarmonyPatch(nameof(PlayerControl.CompleteTask))]
public static void PlayerCompleteTaskPostfix(PlayerControl __instance, uint idx)
{
    AchievementsTabSingleton<ExampleTab>.Instance.Taskmaster.Increment(1);
}
```

## Localization
Localization is done through `VentLib.Localization`, for more information, see [VentFramework's documentation](https://github.com/Lotus-AU/VentFramework-Continued#localization).

There are a few ways that you can set up localization for your project:
- Creating a class specifically for Translations:
```csharp
[Localized("Strings")]
public static class Localization
{
    [Localized(nameof(AchievementObtained))] 
    public static string AchievementObtained = "Achievement Obtained!";
    
    [Localized(nameof(AchievementProgressed))] 
    public static string AchievementProgressed = "Achievement Progressed!";
    
    [Localized(nameof(HiddenAchievement))] 
    public static string HiddenAchievement = "Hidden Achievement";
    
    [Localized(nameof(HiddenAchievementDescription))] 
    public static string HiddenAchievementDescription = "This achievement is hidden, discover more information about it by unlocking it!";
}
```

- Adding the `[Localized]` attribute directly to the class you want to localize:
```csharp
[Localized(nameof(ExampleTab))]
public class ExampleTab : AchievementsTab
{
    [Localized("ExampleTabName")]
    public static string ExampleTabName = "Example Tab";
        
    [Localized("ExampleAchievementName")]
    public static string ExampleAchievementName = "Getting Started!";
        
    [Localized("ExampleAchievementDescription")]
    public static string ExampleAchievementDescription = "Check out the achievements button on the main menu!";
    
    public override string Name => ExampleTabName;
    public override bool IsSelectable => AchievementsManager.Tabs.Count == 1; // if there are more tabs, those will be shown instead.
    public override Color GetTabColor()
    {
        return Color.red;
    }

    public BaseAchievement achievement { get; set; } = 
        new BaseAchievement(ExampleAchievementName, ExampleAchievementDescription, "AchievementsAPI.Resources.ExampleIcon.png");
    
    public override Sprite GetIcon()
    {
        return SpriteTools.LoadSpriteFromPath("AchievementsAPI.Resources.ExampleIcon.png", Assembly.GetCallingAssembly(), 100);
    }
}
```
- Or manually shipping a language file, then using `Localizer.Translate()` to get the localized string:
```csharp
public class ExampleTab : AchievementsTab
{
    private Localizer VentLocalizer => Localizer.Get();
    
    public override string Name => VentLocalizer.Translate("ExampleTab.Name");
    
    public override Color GetTabColor()
    {
        return Color.red;
    }
    
    public BaseAchievement achievement { get; set; } = 
        new BaseAchievement(VentLocalizer.Translate("ExampleTab.ExampleAchievement.Name"), VentLocalizer.Translate("ExampleTab.ExampleAchievement.Description"), "AchievementsAPI.Resources.ExampleIcon.png");
}
```
```yaml
# Languages/[YourAssembly]/lang_English.yaml
Name: English
Authors:
- You
Translations:
  ExampleTab:
    Name: "Example Tab"
    ExampleAchievement:
      Name: "Example Achievement"
      Description: "This is an example achievement."
```