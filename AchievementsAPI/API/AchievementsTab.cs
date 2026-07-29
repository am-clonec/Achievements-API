using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

namespace AchievementsAPI.API;

/// <summary>
/// Abstract class for implementing an achievements tab.
/// </summary>
public abstract class AchievementsTab
{
    /// <summary>
    /// The name of the Achievements Tab, displayed in UI.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Whether the tab should be selectable in the Achievements Menu. Handy for custom UI's or internal achievements.
    /// </summary>
    public virtual bool IsSelectable => true;

    /// <summary>
    /// A method which gets the color of the Achievements Menu background when switching to this tab.
    /// </summary>
    public virtual Color GetTabColor()
    {
        return new Color32(255, 255, 150, 255);
    }
    /// <summary>
    /// The path to the tabs icon, for if you don't have it as a Sprite
    /// </summary>
    public virtual string IconPath => "AchievementsAPI.Resources.ExampleIcon.png";
    
    
    /// <summary>
    /// A Method which gets the icon of the tab, used for its icon in the Achievements Menu. 
    /// </summary>
    /// <returns></returns>
    public virtual Sprite GetIcon()
    {
        return SpriteTools.LoadSpriteFromPath(IconPath, Assembly.GetCallingAssembly(), 100);
    }
}