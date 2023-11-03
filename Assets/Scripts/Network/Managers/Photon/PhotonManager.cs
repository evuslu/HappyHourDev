namespace Evu.Network
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Evu.Common;
    using Fusion;
    using Fusion.Sockets;
    using System.Threading.Tasks;

    public class PhotonManager : EternalSingleton<PhotonManager>, INetworkRunnerCallbacks
    {
        private const string GameVersion = "1";
        public const int MAX_PLAYER_COUNT = 2;

        public bool IsConnected => networkRunner.IsConnectedToServer;

        [SerializeField] private NetworkRunner networkRunner = null;

        #region Mono

        protected override void Awake()
        {
            base.Awake();

            if (Instance != this)
                return;

            //network manager runs as dont destroy on load
            networkRunner.transform.parent = null;
            networkRunner.AddCallbacks(this); // register callbacks
        }

        #endregion

        #region Connect

        public async Task ConnectAsync(Action onConnectSuccess, Action onConnectFail)
        {
            if (networkRunner.IsConnectedToServer)
            {
                onConnectSuccess?.Invoke();
                return;
            }

            var result = await networkRunner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                PlayerCount = MAX_PLAYER_COUNT,
                SessionName = null, // null => random
                MatchmakingMode = Fusion.Photon.Realtime.MatchmakingMode.FillRoom, //fill oldest servers first
                CustomLobbyName = "Default",
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });

            // Check if all went fine
            if (result.Ok)
            {
                onConnectSuccess?.Invoke();
                return;
            }
            else
            {
                onConnectFail?.Invoke();
            }
        }

        #endregion

        #region INetworkRunnerCallbacks

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            PlayerSpawnManager.Instance.OnPlayerJoined(runner, player);
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
            
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
            
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
            
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
            
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
            
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
            
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {
            
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
            
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
            
        }

        #endregion

    }

}