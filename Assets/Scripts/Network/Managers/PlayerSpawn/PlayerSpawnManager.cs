namespace Evu.Network{

    using UnityEngine;
    using Evu.Common;
    using Fusion;

    public class PlayerSpawnManager : EternalSingleton<PlayerSpawnManager>
    {
        [SerializeField] GameObject playerNetworkController = null;

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log("player id " + player.PlayerId);
            if (player == runner.LocalPlayer)
            {
                runner.Spawn(playerNetworkController, new Vector3(0, 1, 0), Quaternion.identity, player);
            }
        }
    }

}