using Evu.Common;
using UnityEditor;

namespace Evu.MainMenu.MainMenuStateMachine
{
    public class StateWelcome : StateBase
    {
        public override StateIds StateId() => StateIds.Welcome;

        public StateWelcome(StateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter(StateInfo info, StateBase oldState)
        {
            base.OnEnter(info, oldState);

            WelcomeController.Instance.ShowController();
        }

        public override void OnExit(StateInfo info, StateBase newState)
        {
            base.OnExit(info, newState);

            WelcomeController.Instance.HideController();
        }
    }
}