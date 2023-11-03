namespace Evu.Network{

    using UnityEngine;
    using Evu.Common;
    using Evu.Level;
    using Fusion;
    using System.Collections.Generic;

    public class PlayerSpawnManager : EternalSingleton<PlayerSpawnManager>
    {
        [SerializeField] GameObject playerNetworkPrefab = null;

        private List<PlayerCharacterController> localCharacters = new List<PlayerCharacterController>();

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (player != runner.LocalPlayer)
                return;

            runner.Spawn(playerNetworkPrefab, Vector3.zero, Quaternion.identity, player);
        }

    }

}