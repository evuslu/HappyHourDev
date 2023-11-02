using Evu.Common;
using Evu.Common.UI;

namespace Evu.MainMenu.MainMenuStateMachine
{
    public class StateLoadLevel : StateBase
    {
        public override StateIds StateId() => StateIds.LoadLevel;

        public StateLoadLevel(StateMachine stateMachine) : base(stateMachine) { }

        public override bool IsTabState => true;

        public override void OnEnter(StateInfo info, StateBase oldState)
        {
            base.OnEnter(info, oldState);

            AlertController.Instance.ShowController("", "", "Ok", () => {
                LoadingController.Instance.ShowController(MenuAnim.MenuSlideAnimTypes.RightToCenter, () =>
                {
                    LevelSceneManager.LoadLevel();
                });
            });

            /*
            LoadingController.Instance.ShowController(MenuAnim.MenuSlideAnimTypes.RightToCenter, () =>
            {
                LevelSceneManager.LoadLevel();
            });
            */
        }
    }
}
