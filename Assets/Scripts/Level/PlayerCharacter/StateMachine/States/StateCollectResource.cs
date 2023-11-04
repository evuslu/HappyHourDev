using Evu.Common;
using UnityEngine;

namespace Evu.Level.PlayerChacterStateMachine
{
    public class StateMoveCollectResource : StateBase
    {
        public override StateIds StateId() => StateIds.CollectResource;

        public StateMoveCollectResource(StateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter(StateInfo info, StateBase oldState)
        {
            base.OnEnter(info, oldState);

            info.controller.StopNavmeshAgent();
            info.controller.DisableNavMeshAgent();

            if (info.targetResource == null)
            {
                //just for caution
                stateMachine.ChangeState(StateIds.Idle);

                return;
            }

            info.targetResource.MoveToPlayer(info.controller);
        }

    }
}