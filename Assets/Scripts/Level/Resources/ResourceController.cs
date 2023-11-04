namespace Evu.Level{

    using UnityEngine;
    using Fusion;

    public class ResourceController : NetworkBehaviour
    {
        public bool IsValidNetworkObject => networkObject.IsValid;

        public enum Types { None = -1, Wood = 0 }

        public Types Type => type;
        [SerializeField] private Types type = Types.None;

        [SerializeField] NetworkObject networkObject = null;


        private PlayerCharacterController targetCharacter = null;

        public void MoveToPlayer(PlayerCharacterController targetCharacter)
        {
            this.targetCharacter = targetCharacter;   
        }

        public void RequestStateAuthority()
        {
            networkObject.RequestStateAuthority();
        }

        public override void FixedUpdateNetwork()
        {
            if (targetCharacter == null)
                return;

            Vector3 targetPos = targetCharacter.transform.position + Vector3.up;
            Vector3 pos = Vector3.Lerp(transform.position, targetPos, 10f * Runner.DeltaTime);

            if ((pos - targetPos).sqrMagnitude < 0.25f)
            {
                // reached to target
                Destroy(gameObject);

                return;
            }

            transform.position = pos;
        }

    }

}