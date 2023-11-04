namespace Evu.Level.PlayerChacterStateMachine
{
    public abstract class StateBase
    {
        public enum StateIds { NetworkManaged = -2, None = -1, Idle = 0, MoveToTarget, CollectResource }
        public abstract StateIds StateId();

        public virtual bool IsTabState => false;

        public virtual void OnEnter(StateInfo info, StateBase oldState) { }
        public virtual void OnExit(StateInfo info, StateBase newState) { }
        public virtual void OnUpdate(StateInfo info) { }
        public virtual void OnFixedUpdate(StateInfo info) { }
        public virtual void OnFixedUpdateNetwork(StateInfo info) { }

        protected StateMachine stateMachine;

        protected StateBase(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
    }
}
