namespace Evu.Level{

    using System.Collections;
    using System.Collections.Generic;
    using Evu.Network;
    using Fusion;
    using UnityEngine;
    using Evu.Level.PlayerChacterStateMachine;

    public class PlayerCharacterController : NetworkBehaviour
    {
        public int characterIndex = -1;

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

        #endregion

        #region Mono

        protected void Awake()
        {
            stateMachine = gameObject.AddComponent<StateMachine>();
            stateMachine.InitStateMachine(stateInfo);
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

            transform.position = GameManager.Instance.CharacterSpawnPosition(playerIndex, characterIndex);

            StartCoroutine(UpdateVisualColor());
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

            stateMachine.FixedUpdateNetwork();

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

            Color playerColor = GameManager.Instance.PlayerColor(playerIndex);

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