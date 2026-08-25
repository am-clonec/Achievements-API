using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace AchievementsAPI.API;

/// <summary>
/// Achievement class for achievements that can increment.
/// </summary>
public class CountAchievement : BaseAchievement
{
    /// <summary>
    /// The current progress for this achievement
    /// </summary>
    public int CurrentValue;
    /// <summary>
    /// The required progress to unlock this achievement.
    /// </summary>
    public int RequiredValue;
    /// <summary>
    /// Defines how the progress persists between games.
    /// </summary>
    public AchPersistence ProgressPersists;
    /// <summary>
    /// Whether to hide the achievement's progress (if the achievement is hidden)
    /// </summary>
    public bool HideProgress;
    public CountAchievement(string name, string description, string iconPath, int currentValue, int requiredValue, AchPersistence progressPersists = AchPersistence.ThroughoutSessions, int rarity = 0, bool hidden = false, bool hideRarity = true, bool hideProgress = false) : base(name, description, iconPath, rarity, hidden, hideRarity, System.Reflection.Assembly.GetCallingAssembly())
    {
        CurrentValue = currentValue;
        RequiredValue = requiredValue;
        ProgressPersists = progressPersists;
        HideProgress = hideProgress;
    }
    public CountAchievement(string name, string description, Sprite icon, int currentValue, int requiredValue, AchPersistence progressPersists = AchPersistence.ThroughoutSessions, int rarity = 0, bool hidden = false, bool hideRarity = true, bool hideProgress = false) : base(name, description, icon, rarity, hidden, hideRarity, System.Reflection.Assembly.GetCallingAssembly())
    {
        CurrentValue = currentValue;
        RequiredValue = requiredValue;
        ProgressPersists = progressPersists;
        HideProgress = hideProgress;
    }
    public CountAchievement(string name, string description, int currentValue, int requiredValue, AchPersistence progressPersists = AchPersistence.ThroughoutSessions, int rarity = 0, bool hidden = false, bool hideRarity = true, bool hideProgress = false) : base(name, description, rarity, hidden, hideRarity, System.Reflection.Assembly.GetCallingAssembly())
    {
        CurrentValue = currentValue;
        RequiredValue = requiredValue;
        ProgressPersists = progressPersists;
        HideProgress = hideProgress;
    }
    /// <summary>
    /// Method to increment the progress of this achievement.
    /// </summary>
    /// <param name="count">The amount to increment by.</param>
    /// <param name="showOnUI">Shows an unlock animation on the hud.</param>
    public void Increment(int count, bool showOnUI = true)
    {
        SetValue(count + CurrentValue, showOnUI);
    }
    /// <summary>
    /// Method to set the progress of this achievement.
    /// </summary>
    /// <param name="value">The progress value.</param>
    /// <param name="showOnUI">Shows an unlock animation on the hud.</param>
    public void SetValue(int value, bool showOnUI = true)
    {
        CurrentValue = value;
        if (showOnUI && !Unlocked) AchievementToast.ShowAndDeleteToast(this, Unlocked);
        if (CurrentValue >= RequiredValue)
        {
            Unlock(false, false);
            AchievementStorage.AchievementStorageUpdate(this, value, true);
            if (showOnUI && !Hidden) AchievementToast.ShowAndDeleteToast(this, true);
            return;
        }
        AchievementStorage.AchievementStorageUpdate(this, value, Unlocked);
        
    }
}

public enum AchPersistence
{
    ThroughoutSessions,
    ThroughoutRounds,
    ResetOnRoundStart,
    ResetOnMeetingStart
}