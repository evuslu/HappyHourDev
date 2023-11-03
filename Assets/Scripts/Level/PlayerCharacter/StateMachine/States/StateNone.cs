namespace Evu.Level.PlayerChacterStateMachine
{
    public class StateNone : StateBase
    {
        public override StateIds StateId() => StateIds.None;

        public StateNone(StateMachine stateMachine) : base(stateMachine) { }
    }
}
