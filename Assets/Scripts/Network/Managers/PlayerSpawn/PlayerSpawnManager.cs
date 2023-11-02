namespace Evu.Network{

    using UnityEngine;
    using Evu.Common;
    using Fusion;

    public class PlayerSpawnManager : EternalSingleton<PlayerSpawnManager>, IPlayerJoined
    {
        [SerializeField] GameObject playerNetworkController = null;

        public void PlayerJoined(PlayerRef player)
        {
            Debug.Log("Player Joined");
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (player == runner.LocalPlayer)
            {
                runner.Spawn(playerNetworkController, new Vector3(0, 1, 0), Quaternion.identity, player);
            }
        }
    }

}