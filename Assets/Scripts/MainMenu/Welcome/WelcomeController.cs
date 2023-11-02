namespace Evu.MainMenu{

    using UnityEngine;

    using Evu.Common;
    using Evu.Network;

    [RequireComponent(typeof(WelcomeUI))]
    public class WelcomeController : Singleton<WelcomeController>
    {
        [SerializeField]
        private WelcomeUI ui = null;

        protected override void Awake()
        {
            base.Awake();

            ui.HideController();
        }

        public void ShowController()
        {
            ui.ShowController();
        }

        public void HideController()
        {
            ui.HideController();
        }

        #region Button Events

        public void JoinRandomRoom()
        {
            MainMenuController.Instance.ChangeState(MainMenuStateMachine.StateBase.StateIds.JoinRoom);
        }

        #endregion
    }

}