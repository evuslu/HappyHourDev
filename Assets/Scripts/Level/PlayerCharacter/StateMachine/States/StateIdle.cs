using Evu.Common;
using UnityEditor;

namespace Evu.Level.PlayerChacterStateMachine
{
    public class StateIdle : StateBase
    {
        public override StateIds StateId() => StateIds.Idle;

        public StateIdle(StateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter(StateInfo info, StateBase oldState)
        {
            base.OnEnter(info, oldState);

            
        }
    }
}