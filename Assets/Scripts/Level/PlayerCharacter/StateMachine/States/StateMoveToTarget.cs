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

            info.controller.EnableAStarAgent();
            info.controller.StartAStarAgent();

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
            info.aStarAgent.SetDestination(info.moveTargetPosition);

            if (!info.aStarAgent.IsPathValid)
            {
                stateMachine.ChangeState(StateIds.Idle);
                return;
            }

            Vector3 nextPos = info.aStarAgent.NextPosition;
            Vector3 dir = nextPos - info.controller.transform.position;
            dir.y = 0f;
            dir = dir.normalized;
            if (dir == Vector3.zero)
                return;

            info.controller.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

            nextPos = info.controller.transform.position + dir * info.speed * deltaTime;

            info.controller.transform.position = nextPos;

            info.aStarAgent.OnPositionUpdate(nextPos, dir);
        }

    }
}