using UnityEngine;
using System.Collections;
using Il2CppInterop.Runtime.InteropTypes.Fields;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using Rewired.Utils;
using UnityEngine.UI;

namespace AchievementsAPI.API;
public class AchievementToast
{
    private static Transform currentToast;

    public static void ShowAndDeleteToast(BaseAchievement achievement)
    {
        Coroutines.Start(CoShowAndDeleteToast(achievement));
    }

    public static void ShowAndDeleteToast(CountAchievement achievement, bool unlocked = false)
    {
        Coroutines.Start(CoShowAndDeleteToast(achievement, unlocked));
    }

    private static Transform GetOrCreateToast()
    {
        GameObject canvas = GameObject.Find("ToastCanvas");
        if (canvas == null)
        {
            var toastCanvas = UnityEngine.Object.Instantiate(Assets.achievementToastCanvasPrefab);
            return toastCanvas.transform.FindChild("Toast");
        }
        else
        {
            var toastGO = UnityEngine.Object.Instantiate(Assets.achievementToastPrefab);
            var toast = toastGO.transform;
            toast.SetParent(canvas.transform);
            foreach (Transform t in canvas.transform)
            {
                t.position += new Vector3(0, -5f, 10f);
            }
            return toast;
        }
    }

    private static void PopulateToast(Transform toast, Sprite icon, System.Reflection.Assembly assembly, string title, string subtitle, Vector3 titleOffset, Vector3 subtitleOffset, Vector3 iconOffset)
    {
        var icoObj = toast.FindChild("AchievementIcon").gameObject.GetComponent<Image>();
        icoObj.sprite = icon;
        icoObj.transform.localPosition += iconOffset;
        var titleObj = toast.FindChild("AchievementName").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        titleObj.text = title;
        titleObj.transform.localPosition += titleOffset;
        var subtitleObj = toast.FindChild("AchievementObtainedText").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        subtitleObj.text = subtitle;
        subtitleObj.transform.localPosition += subtitleOffset;
    }

    private static IEnumerator CoAnimateAndDestroyToast()
    {
        Vector3 onScreenPos = currentToast.localPosition;
        Vector3 offScreenRight = onScreenPos + new Vector3(1500, 0, 0);
        
        TransitionFade.Instance.StartCoroutine(
            Effects.Slide2D(currentToast, offScreenRight, onScreenPos, 0.7f));

        float time = 0;
        while (time <= 3)
        {
            time += Time.deltaTime;
            yield return null;
        }
        
        yield return TransitionFade.Instance.StartCoroutine(
            Effects.Slide2D(currentToast, onScreenPos, offScreenRight, 0.3f));

        time = 0;
        while (time <= 0.5)
        {
            time += Time.deltaTime;
            yield return null;
        }
        
        currentToast.gameObject.Destroy();
        yield break;
    }

    public static IEnumerator CoShowAndDeleteToast(BaseAchievement achievement)
    {
        while (!currentToast.IsNullOrDestroyed())
        {
            yield return null;
        }

        currentToast = GetOrCreateToast();
        PopulateToast(currentToast, achievement.Icon, achievement.Assembly,
            "Achievement Obtained!",
            achievement.Name,
            achievement.ToastTitleOffset,
            achievement.ToastObtainedOffset,
            achievement.ToastIconOffset);
        var img = currentToast.GetComponent<Image>();
        if (achievement.ToastBgSprite != null)
        {
            img.m_Sprite = achievement.ToastBgSprite;
        }

        yield return Coroutines.Start(CoAnimateAndDestroyToast());
    }

    public static IEnumerator CoShowAndDeleteToast(CountAchievement achievement, bool unlocked = false)
    {
        while (!currentToast.IsNullOrDestroyed())
        {
            yield return null;
        }

        currentToast = GetOrCreateToast();
        if (achievement.Hidden && achievement.HideProgress && !unlocked)
        {
            PopulateToast(currentToast, achievement.Icon, achievement.Assembly,
                "Achievement Progressed!",
                "Hidden Achievement",
                achievement.ToastTitleOffset,
                achievement.ToastObtainedOffset,
                achievement.ToastIconOffset);
        }
        else
        {
            PopulateToast(currentToast, achievement.Icon, achievement.Assembly,
                unlocked ? "Achievement Obtained!" : "Achievement Progressed!",
                $"{achievement.Name} ({achievement.CurrentValue}/{achievement.RequiredValue})",
                achievement.ToastTitleOffset,
                achievement.ToastObtainedOffset,
                achievement.ToastIconOffset);
        }
        var img = currentToast.GetComponent<Image>();
        if (achievement.ToastBgSprite != null)
        {
            img.m_Sprite = achievement.ToastBgSprite;
        }

        yield return Coroutines.Start(CoAnimateAndDestroyToast());
    }
}