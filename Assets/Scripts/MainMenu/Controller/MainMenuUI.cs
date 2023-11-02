namespace Evu.MainMenu
{
    using Common.UI;
    using UnityEngine;

    public class MainMenuUI : UIBase
    {
        [Space]
        [SerializeField] GameObject goCanvasMain = null;

        public void ShowUI()
        {
            goCanvasMain.SetActive(true);
        }
    }
}