namespace Evu.MainMenu{

    using UnityEngine;

    public class WelcomeUI : MonoBehaviour
    {

        [SerializeField] GameObject goRoot = null;

        private WelcomeController Controller => WelcomeController.Instance;

        public void ShowController()
        {
            goRoot.SetActive(true);
        }

        public void HideController()
        {
            goRoot.SetActive(false);
        }

        #region Button Events

        public void OnButtonJoinRandomRoomClick()
        {
            Controller.JoinRandomRoom();
        }

        #endregion

    }

}