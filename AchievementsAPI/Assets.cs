using System;
using System.Reflection;
using AchievementsAPI.Reactor.Embedded;
using UnityEngine;

namespace AchievementsAPI;

public class Assets
{
    public static AssetBundle assetBundle { get; set; } = EmbeddedBundleManager.Load("achievements");
    public static GameObject achievementPrefab { get; set; } =
        assetBundle.LoadAsset<GameObject>("AchievementsMenu")
            ?.DontDestroy()
        ?? throw new InvalidOperationException("Asset 'AchievementsMenu' not found in bundle");

    public static GameObject achievementToastCanvasPrefab { get; set; } =
        assetBundle.LoadAsset<GameObject>("ToastCanvas")?
            .DontDestroy() 
        ?? throw new InvalidOperationException("Asset 'ToastCanvas' not found in bundle");
    public static GameObject achievementToastPrefab { get; set; } = 
        assetBundle.LoadAsset<GameObject>("Toast")?
            .DontDestroy()
        ?? throw new InvalidOperationException("Asset 'Toast' not found in bundle");
    
    public static Sprite StarSprite { get; set; } = assetBundle.LoadAsset<Sprite>("Star");
}