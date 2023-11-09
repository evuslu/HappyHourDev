namespace Evu.Network{

    using UnityEngine;
    using Evu.Common;
    using Evu.Level;
    using Fusion;
    using Unity.VisualScripting;
    using Evu.AStar;

    public class PlayerNetworkController : NetworkBehaviour
    {
        [SerializeField] NetworkObject networkObject = null;
        [SerializeField] PlayerCharacterController characterPrefab = null;
        
        private bool isInitCompleted = false;
        private bool IsLevelLoadCompleted => GameManager.Instance?.IsLevelLoadCompleted ?? false;

        private void Awake()
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            GameManager.OnLevelLoad += CheckInitialization;
        }

        private void OnDestroy()
        {
            GameManager.OnLevelLoad -= CheckInitialization;
        }

        protected void Start()
        {
            CheckInitialization();
        }

        /// <summary>
        /// Checks the initialization. Waits for game manager level load progress.
        /// Triggers on Start and GameManager LevelLoad
        /// </summary>
        private void CheckInitialization()
        {
            if (isInitCompleted)
                return;

            //if this is not my player do not init !!!
            //if level complete not completed do not init !!!
            if (!HasStateAuthority || !IsLevelLoadCompleted)
                return;

            PlayerRef playerRef = networkObject.Runner.LocalPlayer;
            int characterCount = UserManager.GameSettingsVM.PlayerCharacterCount;

            for (int i = 0; i < characterCount; i++)
            {
                PlayerCharacterController spawnedCharacter = networkObject.Runner.Spawn(characterPrefab, Vector3.zero, Quaternion.identity, playerRef);
                spawnedCharacter.CharacterIndex = i;
            }

            PlayerSpawnManager.Instance.HandleResourceSpawns(networkObject, playerRef);

            isInitCompleted = true;
        }
    }

}