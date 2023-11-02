namespace Evu.Common
{

    using UnityEngine.SceneManagement;

    public static class LevelSceneManager
    {
        const string SN_SPLASH = "Splash";
        const string SN_MAIN_MENU = "MainMenu";
        const string SN_LEVEL = "Level";

        public enum Scenes
        {
            UnityStartup,
            Splash,
            MainMenu,
            Level
        }

        public static Scenes CurrentScene { get; private set; } =
#if UNITY_EDITOR
                    Scenes.UnityStartup;
#else
                    Scenes.Splash;
#endif

        public static void LoadSplash()
        {
            CurrentScene = Scenes.Splash;

            SceneManager.LoadScene(SN_SPLASH);
        }

        public static void LoadMainMenu()
        {
            CurrentScene = Scenes.MainMenu;

            SceneManager.LoadScene(SN_MAIN_MENU);
        }

        public static void LoadLevel()
        {
            CurrentScene = Scenes.Level;

            SceneManager.LoadScene(SN_LEVEL);
        }

    }

}