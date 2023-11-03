namespace Evu.Level.PlayerChacterStateMachine
{
    using System;
    using Evu.Common.UI;
    using UnityEngine;

    [Serializable]
    public class StateInfo
    {
        public GameObject[] goVisuals = null;
        public SkinnedMeshRenderer skinnedMeshRenderer = null;

        public Animator animator = null;
        private int animHashWalkSpeed;

        public void InitStateInfo()
        {
            animHashWalkSpeed = Animator.StringToHash("walkSpeed");
        }

        public void SetAnimatorWalkSpeedParameter(float speed)
        {
            animator.SetFloat(animHashWalkSpeed, speed);
        }
    }
}

