namespace Evu.MainMenu.MainMenuStateMachine
{
    public abstract class StateBase
    {
        public enum StateIds { None = -1, Welcome = 0, LoadLevel, JoinRoom }
        public abstract StateIds StateId();

        public virtual bool IsTabState => false;

        public virtual void OnEnter(StateInfo info, StateBase oldState) { }
        public virtual void OnExit(StateInfo info, StateBase newState) { }
        public virtual void OnUpdate(StateInfo info) { }
        public virtual void OnFixedUpdate(StateInfo info) { }

        protected StateMachine stateMachine;

        protected StateBase(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
    }
}
