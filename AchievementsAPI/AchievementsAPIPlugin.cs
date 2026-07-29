using System.Collections.Generic;
using AchievementsAPI.API;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using VentLib;

namespace AchievementsAPI;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
[BepInDependency(Vents.Id)]
public partial class AchievementsAPIPlugin : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    public override void Load()
    {
        AchievementsManager.Initialize();
        Harmony.PatchAll();
    }
}