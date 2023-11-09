namespace Evu.AStar{

    using System;
    using UnityEngine;

    public class AStarObject : MonoBehaviour
    {
        [NonSerialized]
        public AStarCell Cell = null;

        public bool IsWalkable => isWalkable;
        [SerializeField]
        protected bool isWalkable = false;

        protected virtual void Start()
        {
            if (Cell != null)
            {
                transform.position = Cell.position;
            }
                
        }
    }

}