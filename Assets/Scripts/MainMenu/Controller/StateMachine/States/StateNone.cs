namespace Evu.MainMenu.MainMenuStateMachine
{
    public class StateNone : StateBase
    {
        public override StateIds StateId() => StateIds.None;

        public StateNone(StateMachine stateMachine) : base(stateMachine) { }
    }
}
