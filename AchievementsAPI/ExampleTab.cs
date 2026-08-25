using System.Reflection;
using AchievementsAPI.API;
using UnityEngine;

namespace AchievementsAPI;

public class ExampleTab : AchievementsTab
{
    public override string Name => "Example Tab";
    public override bool IsSelectable => AchievementsManager.Tabs.Count == 1; // if there are more tabs, those will be shown instead.
    public override Color GetTabColor()
    {
        return Color.red;
    }

    public BaseAchievement achievement { get; set; } = new BaseAchievement("Getting Started!", "Check out the achievements button on the main menu!", "AchievementsAPI.Resources.ExampleIcon.png");
    public override Sprite GetIcon()
    {
        return SpriteTools.LoadSpriteFromPath("AchievementsAPI.Resources.ExampleIcon.png", Assembly.GetCallingAssembly(), 100);
    }
}