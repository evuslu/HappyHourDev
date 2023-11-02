namespace Evu.Splash
{

    using UnityEngine;
    using Evu.Common;

    public class SplashController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            LoadingController.Instance.ShowController(Common.UI.MenuAnim.MenuSlideAnimTypes.None, () => {
                    LevelSceneManager.LoadMainMenu();
                });   
        }

    }

}