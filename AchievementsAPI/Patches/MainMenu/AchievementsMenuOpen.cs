namespace AchievementsAPI.Patches.MainMenu;

public static class AchievementsMenuOpen
{
    public static void OpenMenu(MainMenuManager mainMenuManager)
    {
        var menu = UnityEngine.Object.Instantiate(Assets.achievementPrefab).GetComponent<AchievementsMenu>();
        menu.mainMenuManager = mainMenuManager;
        menu.gameObject.SetActive(true);
    }
    public static void OpenMenu(OptionsMenuBehaviour optionsMenuBehaviour)
    {
        var menu = UnityEngine.Object.Instantiate(Assets.achievementPrefab).GetComponent<AchievementsMenu>();
        menu.OptionsMenuBehaviour = optionsMenuBehaviour;
        menu.gameObject.SetActive(true);
    }
}