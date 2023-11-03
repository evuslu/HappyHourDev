namespace Evu.Level.PlayerChacterStateMachine
{
    public class StateNetworkManaged : StateBase
    {
        public override StateIds StateId() => StateIds.NetworkManaged;

        public StateNetworkManaged(StateMachine stateMachine) : base(stateMachine) { }
    }
}
