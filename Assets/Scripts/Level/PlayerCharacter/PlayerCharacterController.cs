namespace Evu.Level{

    using System.Collections;
    using System.Collections.Generic;
    using Evu.Network;
    using Fusion;
    using UnityEngine;
    using Evu.Level.PlayerChacterStateMachine;

    public class PlayerCharacterController : NetworkBehaviour
    {
        [Networked]
        public int CharacterIndex { get; set; } = -1;

        [SerializeField] NetworkObject networkObject = null;

        [Space]
        [Header("State Info")]
        [SerializeField] StateInfo stateInfo = new StateInfo();
        private StateMachine stateMachine = null;

        private int playerIndex = -1;

        #region Public Functions

        public void EnableNavMeshAgent() => stateInfo.navMeshAgent.enabled = true;
        public void DisableNavMeshAgent() => stateInfo.navMeshAgent.enabled = false;
        public void StartNavmeshAgent()
        {
            if (!HasStateAuthority)
                return;

            if (stateInfo.navMeshAgent.isActiveAndEnabled)
                stateInfo.navMeshAgent.isStopped = false;
        }
        public void StopNavmeshAgent()
        {
            if (!HasStateAuthority)
                return;

            if (stateInfo.navMeshAgent.isActiveAndEnabled)
                stateInfo.navMeshAgent.isStopped = true;
        }

        public void SetSelectionIndcatorActive(bool isActive)
        {
            stateInfo.goSelectionIndicator.SetActive(isActive);
        }

        public void MoveToTarget(Vector3 moveTargetPosition)
        {
            stateInfo.moveTargetPosition = moveTargetPosition;

            stateMachine.ChangeState(StateBase.StateIds.MoveToTarget);
        }

        public void MoveToTarget(ResourceController resource)
        {
            stateInfo.targetResource = resource;
            stateInfo.targetInctanceId = resource.gameObject.GetInstanceID();

            stateInfo.moveTargetPosition = resource.transform.position;

            stateMachine.ChangeState(StateBase.StateIds.MoveToTarget);
        }

        public void OnResourceCollect()
        {
            stateMachine.ChangeState(StateBase.StateIds.Idle);
        }

        #endregion

        #region Mono

        protected void Awake()
        {
            stateMachine = gameObject.AddComponent<StateMachine>();
            stateMachine.InitStateMachine(stateInfo);

            stateInfo.InitStateInfo();
        }

        private void Start()
        {
            stateInfo.navMeshAgent.updatePosition = false;
            stateInfo.navMeshAgent.updateRotation = false;

            StopNavmeshAgent();
            DisableNavMeshAgent();

            SetSelectionIndcatorActive(false);

            playerIndex = networkObject.InputAuthority.PlayerId % PhotonManager.MAX_PLAYER_COUNT;

            if (!HasStateAuthority)
            {
                // not a local player, let controlled by network
                stateMachine.ChangeState(StateBase.StateIds.NetworkManaged);

                StartCoroutine(UpdateVisualColor());

                return;
            }//if (!HasStateAuthority)

            transform.position = GameManager.Instance.CharacterSpawnPosition(playerIndex, CharacterIndex);

            StartCoroutine(UpdateVisualColor());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (stateInfo.targetResource == null)
                return;

            if (other.gameObject.GetInstanceID() != stateInfo.targetInctanceId)
                return;

            //we found a resource, collect it
            stateMachine.ChangeState(StateBase.StateIds.CollectResource);
        }

        public override void FixedUpdateNetwork()
        {
            stateMachine.FixedUpdateNetwork(Runner.DeltaTime);
            

            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;
            foreach (GameObject go in stateInfo.goVisuals)
            {
                go.transform.position = pos;
                go.transform.rotation = transform.rotation;
            }
        }

        #endregion

        #region Private Functions

        private IEnumerator UpdateVisualColor()
        {
            while (GameManager.Instance == null || !GameManager.Instance.IsLevelLoadCompleted)
                yield return null; // another player's character initialized before our level initialization

            Color playerColor = GameManager.Instance.PlayerColor(playerIndex, CharacterIndex);

            SkinnedMeshRenderer smr = stateInfo.skinnedMeshRenderer;
            MaterialPropertyBlock block = new MaterialPropertyBlock();

            for (int i = 0; i < smr.sharedMaterials.Length; i++)
            {
                smr.GetPropertyBlock(block, i);

                block.SetColor("_Color", playerColor);

                smr.SetPropertyBlock(block, i);
            }
        }

        #endregion

    }

}