namespace Evu.AStar{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Evu.Common;
    using System;

    public class AStarManager : Singleton<AStarManager>
    {
        public static Action<AStarAgent> OnCellUpdate;

        [SerializeField] private float cellSize = 2f;
        [SerializeField] private int cellCountX = 18;
        [SerializeField] private int cellCountY = 18;

        public int CellIndex(float unit) => (int)(unit / cellSize);

        private AStarCell[][] cells;

        protected override void Awake()
        {
            base.Awake();

            cells = new AStarCell[cellCountX][];

            for (int x = 0; x < cellCountX; x++)
            {
                cells[x] = new AStarCell[cellCountY];

                for (int y = 0; y < cellCountY; y++)
                {
                    cells[x][y] = new AStarCell(x, y, new Vector3(cellSize * (x + 0.5f), 0f, cellSize * (y + 0.5f)));
                }
            }

            AStarObject[] starObjects = FindObjectsOfType<AStarObject>();
            foreach (AStarObject starObject in starObjects)
                OnObjectStart(starObject);
        }

        public void OnNetworkObjectSpawn(AStarObject obj)
        {
            for (int x = 0; x < cellCountX; x++)
                for (int y = 0; y < cellCountY; y++)
                    if(cells[x][y].objects != null)
                        cells[x][y].objects.Remove(obj);

            AStarCell cell = FindCellFor(obj.transform.position);
            if (cell == null)
                return;

            cell.AddAStarObject(obj);
        }

        public void ReAsignAgent(AStarAgent agent, AStarCell fromCell, AStarCell toCell)
        {
            if(fromCell != null && fromCell.objects != null)
                fromCell.objects.Remove(agent);

            if(toCell != null)
                toCell.AddAStarObject(agent);

            OnCellUpdate?.Invoke(agent);
        }

        public void ResetCellsForCalculation()
        {
            for (int x = 0; x < cellCountX; x++)
            {
                for (int y = 0; y < cellCountY; y++)
                {
                    cells[x][y].gCost = int.MaxValue;
                    cells[x][y].hCost = 0;

                    cells[x][y].fromCell = null;
                }
            }
        }

        private void OnObjectStart(AStarObject obj)
        {
            AStarCell cell = FindCellFor(obj);

            if (cell == null)
            {
                Debug.LogError($"cell == null {obj.gameObject.name} {obj.transform.position}");
                return;
            }

            cell.AddAStarObject(obj);
        }

        public int DistanceCost(AStarCell a, AStarCell b)
        {
            int distanceX = Mathf.Abs(a.cellX - b.cellX);
            int distanceY = Mathf.Abs(a.cellY - b.cellY);
            int diff = Mathf.Abs(distanceX - distanceY);

            return 14 * Mathf.Min(distanceX, distanceY) +  10 * diff;
        }

        public AStarCell FindCellFor(int cellX, int cellY)
        {
            if (cellX < 0 || cellX > cellCountX - 1 || cellY < 0 || cellY > cellCountY - 1)
                return null;

            return cells[cellX][cellY];
        }

        public AStarCell FindCellFor(Vector3 pos)
        {
            int cellX = CellIndex(pos.x);
            int cellY = CellIndex(pos.z);

            return FindCellFor(cellX, cellY);
        }

        private AStarCell FindCellFor(AStarObject obj)
        {
            Vector3 pos = obj.transform.position;

            return FindCellFor(pos);
        }
    }

}