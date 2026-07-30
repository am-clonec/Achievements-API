using VentLib.Localization.Attributes;
using VentLib.Utilities.Attributes;

namespace AchievementsAPI;

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