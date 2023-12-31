﻿namespace Evu.Level.PlayerChacterStateMachine
{
    using System;
    using Evu.AStar;
    using Evu.Common.UI;
    using UnityEngine;
    using UnityEngine.AI;

    [Serializable]
    public class StateInfo
    {
        public PlayerCharacterController controller = null;

        public GameObject[] goVisuals = null;
        public SkinnedMeshRenderer skinnedMeshRenderer = null;

        public float speed = 5f;
        public AStarAgent aStarAgent = null;
        [NonSerialized]
        public Vector3 moveTargetPosition = Vector3.zero;

        [NonSerialized]
        public ResourceController targetResource = null;

        [NonSerialized]
        public int targetInctanceId = int.MinValue;

        public GameObject goSelectionIndicator = null;

    }
}

