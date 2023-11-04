namespace Evu.Network{

    using UnityEngine;
    using Evu.Common;
    using Evu.Level;
    using Fusion;
    using System.Collections.Generic;

    public class PlayerSpawnManager : EternalSingleton<PlayerSpawnManager>
    {
        [SerializeField] GameObject playerNetworkPrefab = null;
        [SerializeField] ResourceController resourceWoodPrefab = null;

        private List<PlayerCharacterController> localCharacters = new List<PlayerCharacterController>();

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (player != runner.LocalPlayer)
                return;

            runner.Spawn(playerNetworkPrefab, Vector3.zero, Quaternion.identity, player);
        }

        /// <summary>
        /// Collect game objects(Resources) and spawns resources on network
        /// </summary>
        /// <param name="networkObject"></param>
        /// <param name="playerRef"></param>
        public void HandleResourceSpawns(NetworkObject networkObject, PlayerRef playerRef)
        {
            ResourceController[] resources = FindObjectsOfType<ResourceController>();

            if (networkObject.Runner.IsSharedModeMasterClient)
            {
                // spawn common resources on master
                foreach (ResourceController r in resources)
                {
                    if (r.IsValidNetworkObject)
                        continue;//for relogin
                    switch (r.Type)
                    {
                        case ResourceController.Types.Wood:
                            networkObject.Runner.Spawn(resourceWoodPrefab, r.transform.position, r.transform.rotation, playerRef);
                            break;
                        default:
                            Debug.LogError("Unhandled resource type : " + r.Type);
                            break;
                    }
                }
            }//if (networkObject.Runner.IsSharedModeMasterClient)

            //disable originals for all network players
            foreach (ResourceController r in resources)
                if (!r.IsValidNetworkObject)
                    r.gameObject.SetActive(false);//disable only local objects(applicable only non shadered mode master clients), not spawned object on network
        }

    }

}