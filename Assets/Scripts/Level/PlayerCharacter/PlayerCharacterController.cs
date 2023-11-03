namespace Evu.Level{

    using System.Collections;
    using System.Collections.Generic;
    using Evu.Network;
    using Fusion;
    using UnityEngine;

    public class PlayerCharacterController : NetworkBehaviour
    {
        [SerializeField] NetworkObject networkObject = null;

        private int playerIndex = -1;
        private int characterIndex = -1;
        private void Start()
        {
            if (!HasStateAuthority)
                return;

            characterIndex = PlayerSpawnManager.Instance.OnLocalPlayerCharacterSpawn(this);

            PlayerRef playerRef = networkObject.Runner.LocalPlayer;
            playerIndex = playerRef.PlayerId % PhotonManager.MAX_PLAYER_COUNT;

#warning config / const ???
            transform.position = new Vector3(playerIndex * 2f, 0f, characterIndex * 2f);
        }
    }

}