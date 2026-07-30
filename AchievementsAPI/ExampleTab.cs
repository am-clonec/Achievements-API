using System.Reflection;
using AchievementsAPI.API;
using UnityEngine;
using VentLib.Localization;
using VentLib.Localization.Attributes;

namespace AchievementsAPI;

public class ExampleTab : AchievementsTab
{
    public override string Name => Translations.ExampleTabName;
    public override bool IsSelectable => AchievementsManager.Tabs.Count == 1; // if there are more tabs, those will be shown instead.
    public override Color GetTabColor()
    {
        return Color.red;
    }

    public BaseAchievement achievement { get; set; } = 
        new BaseAchievement(Translations.ExampleAchievementName, Translations.ExampleAchievementDescription, "AchievementsAPI.Resources.ExampleIcon.png");
    
    public override Sprite GetIcon()
    {
        return SpriteTools.LoadSpriteFromPath("AchievementsAPI.Resources.ExampleIcon.png", Assembly.GetCallingAssembly(), 100);
    }
    
    [Localized("ExampleTab")]
    public static class Translations
    {
        [Localized("ExampleTabName")]
        public static string ExampleTabName = "Example Tab";
        
        [Localized("ExampleAchievementName")]
        public static string ExampleAchievementName = "Getting Started!";
        
        [Localized("ExampleAchievementDescription")]
        public static string ExampleAchievementDescription = "Check out the achievements button on the main menu!";
    }
}