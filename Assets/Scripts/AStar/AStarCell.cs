namespace Evu.AStar{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AStarCell
    {
        public bool IsWalkable
        {
            get
            {
                if (IsEmpty)
                    return true;

                foreach (AStarObject obj in objects)
                    if (!obj.IsWalkable)
                        return false;

                return true; 
            }
        }

        public bool IsEmpty => objects == null || objects.Count == 0;

        public Vector3 position = Vector3.zero;
        public int cellX;
        public int cellY;

        public int gCost = int.MaxValue;
        public int hCost = 0;
        public int FCost => gCost >= int.MaxValue / 2 || hCost >= int.MaxValue / 2 ? int.MaxValue : gCost + hCost;

        public AStarCell fromCell = null;

        public List<AStarObject> objects = null;

        public void AddAStarObject(AStarObject obj)
        {
            if (objects == null)
                objects = new List<AStarObject>();

            if (objects.Contains(obj))
                return;

            obj.Cell = this;

            objects.Add(obj);
        }

        public AStarCell(int cellX, int cellY, Vector3 position)
        {
            this.cellX = cellX;
            this.cellY = cellY;
            this.position = position;
        }

    }

}