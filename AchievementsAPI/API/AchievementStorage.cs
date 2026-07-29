using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.IO;
using UnityEngine;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AchievementsAPI.API;

[Serializable]
public class AchievementData
{
    public string Name { get; set; }
    public bool Unlocked { get; set; }
    public int Progress { get; set; }
    public string Id { get; set; }
}


public class AchievementStorage
{
    public static List<AchievementData> BaseAchievements = new List<AchievementData>();
    public static string JsonPath => OperatingSystem.IsAndroid() ? Path.Combine(Environment.GetEnvironmentVariable("STAR_DATA_PATH"), "AchievementsAPIData/achievements.json") : Path.Combine(Application.persistentDataPath, "AchievementsAPIData/achievements.json");
    
    public static void AchievementStorageUpdate(BaseAchievement achievement, bool unlocked)
    {
        var data = GetData(achievement);
        data.Unlocked = unlocked;

        Save();
    }

    public static void AchievementStorageUpdate(CountAchievement achievement, int progress, bool unlocked)
    {
        var data = GetData(achievement);
        var newProgress = Math.Clamp(progress, 0, achievement.RequiredValue);
        data.Unlocked = unlocked;
        if (achievement.ProgressPersists is AchPersistence.ThroughoutSessions || data.Unlocked) data.Progress = newProgress;

        Save();
    }
    public static void AchievementDataReset(BaseAchievement achievement, bool? unlocked = false)
    {
        var data = GetData(achievement);
        if (unlocked != null)
        {
            achievement.Unlocked = unlocked.Value;
            data.Unlocked = unlocked.Value;
        }

        Save();
    }

    public static void AchievementDataReset(CountAchievement achievement, int progress = 0, bool? unlocked = null)
    {
        var data = GetData(achievement);
        data.Progress = Math.Clamp(progress, 0, achievement.RequiredValue);
        if (unlocked != null)
        {
            achievement.Unlocked = unlocked.Value;
            data.Unlocked = unlocked.Value;
        }
        else
        {
            data.Unlocked = data.Progress == achievement.RequiredValue;
            achievement.Unlocked = data.Progress == achievement.RequiredValue;
        }

        Save();
    }

    public static AchievementData GetData(BaseAchievement achievement)
    {
        var data =  BaseAchievements.Find(x =>
            x.Name == achievement.Name && x.Id == achievement.Id);
        if (data == null)
        {
            data = new AchievementData { Id = achievement.Assembly.GetName().Name + "_" + achievement.Name, Name = achievement.Name, Unlocked = false };
            BaseAchievements.Add(data);
        }
        return data;
    }
    
    public static AchievementData GetData(CountAchievement achievement)
    {
        var data = BaseAchievements.Find(x =>
            x.Name == achievement.Name && x.Id == achievement.Id);
        if (data == null)
        {
            // Bug 1 fixed: Id was missing, causing Find() to never match on reload
            data = new AchievementData
            {
                Id = achievement.Assembly.GetName().Name + "_" + achievement.Name,
                Name = achievement.Name,
                Unlocked = false,
                Progress = achievement.CurrentValue
            };
            BaseAchievements.Add(data);
        }
        return data;
    }

    public static void AchievementStorageGet(AchievementsTab tab)
    {
        foreach (var propInfo in tab.GetType().GetProperties().Where(x =>
                                 x.PropertyType.IsAssignableTo(typeof(BaseAchievement))))
        {
            var achievement = propInfo.GetValue(tab) as BaseAchievement;
            if (achievement == null) continue;
            var data = BaseAchievements.Find(x =>
                x.Name == achievement.Name && x.Id == achievement.Id);
            if (data == null) continue;
            achievement.Unlocked = data.Unlocked;
        }

        foreach (var propInfo in tab.GetType().GetProperties().Where(x =>
                                 x.PropertyType.IsAssignableTo(typeof(CountAchievement))))
        {
            var achievement = propInfo.GetValue(tab) as CountAchievement;
            if (achievement == null) continue;
            var data = BaseAchievements.Find(x =>
                x.Name == achievement.Name && x.Id == achievement.Id);
            if (data == null) continue;
            achievement.CurrentValue = data.Progress;
            achievement.Unlocked = data.Unlocked;
        }
    }

    public static void Save()
    {
        var directory = Path.GetDirectoryName(JsonPath);

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        File.WriteAllText(
            JsonPath,
            JsonSerializer.Serialize(
                BaseAchievements,
                new JsonSerializerOptions { WriteIndented = true }));
    }

    public static void Load()
    {
        if (!File.Exists(JsonPath))
        {
            BaseAchievements = new List<AchievementData>();
            return;
        }

        var json = File.ReadAllText(JsonPath);

        if (string.IsNullOrWhiteSpace(json))
        {
            BaseAchievements = new List<AchievementData>();
            return;
        }

        BaseAchievements =
            JsonSerializer.Deserialize<List<AchievementData>>(json)
            ?? new List<AchievementData>();
    }
}