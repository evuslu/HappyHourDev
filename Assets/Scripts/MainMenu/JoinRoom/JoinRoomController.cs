namespace Evu.MainMenu{

    using UnityEngine;

    using Evu.Network;
    using static Evu.MainMenu.MainMenuStateMachine.StateBase;
    using Evu.Common.UI;
    using DG.Tweening;
    using Evu.Common;
    using System.Threading.Tasks;

    [RequireComponent(typeof(JoinRoomUI))]
    public class JoinRoomController : Singleton<JoinRoomController>
    {
        [SerializeField]
        private JoinRoomUI ui = null;

        public void ShowController()
        {
            ui.ShowUI();

            ui.UpdateInfoText("Joining");

            Connect();
        }

        public void HideController(MenuAnim.MenuSlideAnimTypes slideAnimType)
        {
            ui.HideUI(slideAnimType);
        }

        private async void Connect()
        {
            ui.UpdateInfoText("Connecting");

            await PhotonManager.Instance.ConnectAsync(() =>
            {
                //success
                DOVirtual.DelayedCall(0.2f, () => MainMenuController.Instance.ChangeState(StateIds.LoadLevel));
            }, () =>
            {
                //fail
                ui.UpdateInfoText("");

                AlertController.Instance.ShowController("Error"
                    , "Failed To Connect"
                    , "Reload"
                    , () => LevelSceneManager.LoadMainMenu());
            });
        }

    }

}