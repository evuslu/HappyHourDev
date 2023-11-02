using System.Collections;
using Evu.Common;
using Evu.Common.UI;
using Evu.Network;

namespace Evu.MainMenu.MainMenuStateMachine
{
    public class StateJoinRoom : StateBase
    {
        public override StateIds StateId() => StateIds.JoinRoom;

        public StateJoinRoom(StateMachine stateMachine) : base(stateMachine) { }

        public override bool IsTabState => true;

        public override void OnEnter(StateInfo info, StateBase oldState)
        {
            base.OnEnter(info, oldState);

            JoinRoomController.Instance.ShowController();
        }

        public override void OnExit(StateInfo info, StateBase newState)
        {
            base.OnExit(info, newState);

            if(newState.StateId() == StateIds.Welcome)
                JoinRoomController.Instance.HideController(MenuAnim.MenuSlideAnimTypes.CenterToRight);
            else
                JoinRoomController.Instance.HideController(MenuAnim.MenuSlideAnimTypes.CenterToLeft);
        }
    }
}
