namespace Evu.AStar{
    using System.Collections.Generic;
    using UnityEngine;

    public class AStarAgent : AStarObject
    {
        private bool isStoped = true;
        public bool IsStopped
        {
            get => isStoped;
            set => isStoped = value;
        }

        public bool IsPathValid => pathCells.Count > 0;

        public Vector3 NextPosition
        {
            get
            {
                if (!IsPathValid)
                    return Vector3.zero;

                return pathCells[0].position;
            }
        }

        private AStarCell currentCell = null;
        private AStarCell targetCell = null;

        private List<AStarCell> pathCells = new List<AStarCell>();

        private AStarManager Manager => AStarManager.Instance;

        #region Mono

        private void Awake()
        {
            AStarManager.OnCellUpdate += OnCellUpdate;
        }

        private void OnDestroy()
        {
            AStarManager.OnCellUpdate -= OnCellUpdate;
        }

        protected override void Start()
        {
            base.Start();

            currentCell = Manager.FindCellFor(transform.position);
        }

        #endregion

        #region Public Functions

        public void OnNetworkSpawn()
        {
            currentCell = Manager.FindCellFor(transform.position);

            Manager.OnNetworkObjectSpawn(this);
        }

        public void SetDestination(Vector3 targetPos)
        {
            AStarCell targetCandidate = Manager.FindCellFor(targetPos);

            if (targetCandidate == null)
                return;

            if (targetCell != null && targetCell == targetCandidate)
                return;

            //we have new target
            targetCell = targetCandidate;

            UpdatePath();
        }

        public void OnPositionUpdate(Vector3 posNew, Vector3 direction)
        {
            AStarCell cellNew = Manager.FindCellFor(posNew);

            if (currentCell != cellNew && !cellNew.IsWalkable)
            {
                //cell might be updated during walk
                UpdatePath();
                //pathCells.Clear();
                if (!IsPathValid)
                    IsStopped = true;

                return;
                /*
                if (pathCells.Contains(cellNew))
                {
                    //cell might be updated during walk
                    UpdatePath();
                    //pathCells.Clear();
                    if(!IsPathValid)
                        IsStopped = true;

                    return;
                }
                */
            }
                

            if (currentCell != cellNew)
            {
                Vector3 targetDir = cellNew.position - posNew;
                targetDir = targetDir.normalized;

                if (Vector3.Dot(targetDir, direction) > 0.01f && (posNew - cellNew.position).sqrMagnitude > 0.1f)
                {
                    //we havent reached to center of target
                    return;
                }

                Manager.ReAsignAgent(this, currentCell, cellNew);
                
                currentCell = cellNew;
                if (IsPathValid && cellNew == pathCells[0])
                    pathCells.RemoveAt(0);
            }
        }

        #endregion

        #region Private Functions

        private void UpdatePath()
        {
            pathCells.Clear();

            AStarCell startCell = Manager.FindCellFor(transform.position);

            Manager.ResetCellsForCalculation();

            Dictionary<(int, int), AStarCell> openList = new Dictionary<(int, int), AStarCell>();
            openList.Add((startCell.cellX, startCell.cellY), startCell);

            Dictionary<(int, int), AStarCell> closedList = new Dictionary<(int, int), AStarCell>();

            startCell.gCost = 0;
            startCell.hCost = Manager.DistanceCost(startCell, targetCell);

            AStarCell currentCell = null;
            while (openList.Count > 0)
            {
                currentCell = LowestFCostCell(openList);

                if (currentCell == targetCell)
                    break;

                openList.Remove((currentCell.cellX, currentCell.cellY));
                closedList.Add((currentCell.cellX, currentCell.cellY), currentCell);

                //neighbours 
                int i = -1;
                for (int dx = -1; dx < 2; dx++)
                {
                    for (int dy = -1; dy < 2; dy++)
                    {
                        if (dx == 0 && dy == 0)
                            continue;//current cell
                        i++;
                        AStarCell neighbour = Manager.FindCellFor(currentCell.cellX + dx, currentCell.cellY + dy);

                        if (neighbour == null)
                            continue;

                        if (!neighbour.IsWalkable)
                        {
                            if (!closedList.ContainsKey((neighbour.cellX, neighbour.cellY)))
                                closedList.Add((neighbour.cellX, neighbour.cellY), neighbour);
                            continue;
                        }

                        if (closedList.ContainsKey((neighbour.cellX, neighbour.cellY)))
                            continue;

                        int cost = currentCell.gCost + Manager.DistanceCost(currentCell, targetCell);

                        if (cost < neighbour.gCost)
                        {
                            neighbour.fromCell = currentCell;
                            neighbour.gCost = cost;
                            neighbour.hCost = Manager.DistanceCost(neighbour, targetCell);

                            if (!openList.ContainsKey((neighbour.cellX, neighbour.cellY)))
                                openList.Add((neighbour.cellX, neighbour.cellY), neighbour);
                        }//if (cost < neighbour.gCost)

                    }//for (int dy = -1; dy < 2; dy++)
                }//for (int dx = -1; dx < 2; dx++)


                currentCell.gCost = 0;
            }//while (openList.Count > 0)

            if (currentCell != targetCell)
                return; // we don't have a valid path

            CreatePath();
        }

        private AStarCell LowestFCostCell(Dictionary<(int,int),AStarCell> dictCells)
        {
            AStarCell low = null;

            foreach (AStarCell cell in dictCells.Values)
                if (low == null)
                    low = cell;
                else if (cell.FCost < low.FCost)
                    low = cell;

            return low;
        }

        private void CreatePath()
        {
            if (currentCell != null)
                if(currentCell.objects != null)
                    currentCell.objects.Remove(this);

            AStarCell current = targetCell;
            pathCells.Clear();
            pathCells.Add(current); //target

            while (current.fromCell != null)
            {
                pathCells.Add(current.fromCell);
                current = current.fromCell;
            }

            pathCells.Reverse();

            if (pathCells.Count == 0)
            {
                currentCell = Manager.FindCellFor(transform.position);
                return;
            }

            currentCell = pathCells[0];
            currentCell.AddAStarObject(this);

            pathCells.RemoveAt(0);

            /*
            foreach (AStarCell cell in pathCells)
                Debug.Log(cell.cellX + " " + cell.cellY);
            */
            
        }

        private void OnCellUpdate(AStarAgent agent)
        {
            if (agent == this)
                return;

            if (IsStopped)
                return;

            if (targetCell != null)
            {
                //try to remove not walkable cells starting by last targets
                // if one next targets not walkable it will be handled on position update
                int index = pathCells.Count - 1;
                while (index > -1)
                {
                    if (!pathCells[index].IsWalkable)
                    {
                        pathCells.RemoveAt(index);
                        index--;
                    }
                    else
                        break;
                }

                if(!IsPathValid)
                    UpdatePath();
            }
                
        }

        #endregion
    }

}