namespace Evu.Level.PlayerChacterStateMachine
{
    using System.Collections.Generic;
    using UnityEngine;
    using StateIds = StateBase.StateIds;

    public class StateMachine : MonoBehaviour
    {
        private List<StateBase> states = new List<StateBase>();
        private Dictionary<StateIds, StateBase> dictStates = new Dictionary<StateIds, StateBase>();

        public StateBase State { get; private set; } = null;
        private StateInfo stateInfo = null;

        #region Mono Behaviour

        private void Update()
        {
            if (State == null || stateInfo == null)
                return;

            State.OnUpdate(stateInfo);
        }

        private void FixedUpdate()
        {
            if (State == null || stateInfo == null)
                return;

            State.OnFixedUpdate(stateInfo);
        }

        #endregion

        #region Public Functions

        public void InitStateMachine(StateInfo stateInfo)
        {
            states.Add(new StateNetworkManaged(this));
            states.Add(new StateNone(this));
            states.Add(new StateIdle(this));
            
            this.stateInfo = stateInfo;

            dictStates.Clear();

            foreach (StateBase stateBase in states)
                dictStates.Add(stateBase.StateId(), stateBase);

            State = states[0];
        }

        
        private bool isChangeStateLocked = false;
        private List<StateIds> changeStateIds = new List<StateIds>();

        public void ChangeState(StateIds stateIdNew)
        {
            if (isChangeStateLocked)
            {
                // ChangeState called inside change state funcion(OnExit,OnEnter etc)
                // wait for other ChangeState function execution
                changeStateIds.Add(stateIdNew);
                return;
            }

            isChangeStateLocked = true;

            StateBase stateNew = StateBase(stateIdNew);
            if (stateNew == null)
                return;

            StateIds oldState = State.StateId();
            State.OnExit(stateInfo, stateNew);
            
            stateNew.OnEnter(stateInfo, State);

            State = stateNew;

            isChangeStateLocked = false;

            if (changeStateIds.Count > 0)
            {
                //we have a new ChangeState request during execution -> handle it
                StateIds changeStateId = changeStateIds[0];
                changeStateIds.RemoveAt(0);
                ChangeState(changeStateId);
            }
        }

        #endregion

        #region Private Functions

        private StateBase StateBase(StateIds stateId)
        {
            StateBase stateBase = null;
            dictStates.TryGetValue(stateId, out stateBase);

            if (stateBase == null)
            {
                Debug.LogError("StateMachine:StateBase missing state for state id : " + stateId.ToString());
            }

            return stateBase;
        }

        #endregion
    }
}

