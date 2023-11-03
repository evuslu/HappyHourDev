namespace Evu.Network{

    using UnityEngine;
    using Evu.Common;
    using Fusion;
    using Unity.VisualScripting;

    public class PlayerNetworkController : NetworkBehaviour
    {
        [SerializeField] NetworkObject networkObject = null;

        protected void Start()
        {
            Debug.Log("HasStateAuthority : " + HasStateAuthority);
        }
    }

}