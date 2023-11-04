using Evu.Common;
using UnityEngine;

namespace Evu.Level.PlayerChacterStateMachine
{
    public class StateMoveToTarget : StateBase
    {
        public override StateIds StateId() => StateIds.MoveToTarget;

        public StateMoveToTarget(StateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter(StateInfo info, StateBase oldState)
        {
            base.OnEnter(info, oldState);

            info.controller.EnableNavMeshAgent();
            info.controller.StartNavmeshAgent();

            if (info.targetResource != null)
                info.targetResource.RequestStateAuthority();
        }

        public override void OnExit(StateInfo info, StateBase newState)
        {
            base.OnExit(info, newState);

            info.controller.StopNavmeshAgent();
            //info.controller.DisableNavMeshAgent();
        }

        public override void OnFixedUpdateNetwork(StateInfo info, float deltaTime)
        {
            info.navMeshAgent.destination = info.moveTargetPosition;

            Vector3 nextPos = info.navMeshAgent.nextPosition;
            Vector3 dir = nextPos - info.controller.transform.position;
            dir.y = 0f;
            dir = dir.normalized;
            if (dir != Vector3.zero)
                info.controller.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

            info.controller.transform.position = info.navMeshAgent.nextPosition;
        }

    }
}