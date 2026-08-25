using System.Linq;
using System.Collections.Generic;
using AchievementsAPI.API;
using HarmonyLib;

namespace AchievementsAPI.Patches;

// For resetting achievements that don't persist

[HarmonyPatch]
public static class AchievementPersistencePatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnGameEnd))]
    [HarmonyPatch(typeof(AchievementManager), nameof(AchievementManager.OnMatchStart))]
    [HarmonyPatch(typeof(AchievementManager), nameof(AchievementManager.OnMatchExited))]
    [HarmonyPatch(typeof(LobbyBehaviour), nameof(LobbyBehaviour.Start))]
    [HarmonyPatch(typeof(TutorialManager), nameof(TutorialManager.Awake))]
    [HarmonyPrefix]
    public static void OnGameEndAndStart()
    {
        foreach (var achievement in GetIncompleteAchievements())
        {
            if (achievement.ProgressPersists != AchPersistence.ThroughoutSessions)
            {
                AchievementStorage.AchievementDataReset(achievement);
            }
        }
    }
    [HarmonyPatch(typeof(AchievementManager), nameof(AchievementManager.OnMeetingCalled))]
    [HarmonyPrefix]
    public static void OnMeetingCalled()
    {
        foreach (var achievement in GetIncompleteAchievements())
        {
            if (achievement.ProgressPersists == AchPersistence.ResetOnMeetingStart)
            {
                AchievementStorage.AchievementDataReset(achievement);
            }
        }
    }
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.ReEnableGameplay))]
    [HarmonyPrefix]
    public static void OnRoundStart()
    {
        foreach (var achievement in GetIncompleteAchievements())
        {
            if (achievement.ProgressPersists == AchPersistence.ResetOnRoundStart)
            {
                AchievementStorage.AchievementDataReset(achievement);
            }
        }
    }

    private static List<CountAchievement> GetIncompleteAchievements()
    {
        var newAchievements = new List<CountAchievement>();
        foreach (var tab in AchievementsManager.Tabs)
        {
            foreach (var propInfo in tab.GetType().GetProperties().Where(x => x.PropertyType == typeof(CountAchievement)))
            {
                var achievement = propInfo.GetValue(tab) as CountAchievement;
                if (achievement == null) continue;
                if (achievement.CurrentValue != achievement.RequiredValue)
                {
                    newAchievements.Add(achievement);
                }
            }
        }

        return newAchievements;
    }
}