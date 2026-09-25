
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PathNode
{
    public List<PathNode> NeighbourNodes { get; private set; }
    
    public int x { get; }
    public int y { get; }
    public float WorldSpaceX { get; }
    public float WorldSpaceY { get; }

    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost { get { return GCost + HCost; } }

    public bool IsWalkable { get; set; }

    public PathNode CameFromNode { get; set; }

    private Grid<PathNode> grid;
    private GameObject debugNotWalkableMarker;

    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        WorldSpaceX = grid.GetWorldPosition(x, y).x;
        WorldSpaceY = grid.GetWorldPosition(x, y).y;
        IsWalkable = true;

        //Debug.Log(Utils.Json(new { x, y, IsWalkable }));
    }

    public Vector3 GetCenterOfNode()
    {
        return grid.GetCenterOfCellPosition(x, y);
    }

    public void PathFindPositions(out Vector2 leftSidePathFindBoundries, out Vector2 rightSidePathFindBoundries)
    {
        float pathFindCellBounds = (grid.CellSize / 4) + 2;

        leftSidePathFindBoundries = new Vector2(WorldSpaceX + pathFindCellBounds, WorldSpaceY + pathFindCellBounds);
        rightSidePathFindBoundries = new Vector2(WorldSpaceX + grid.CellSize - pathFindCellBounds, WorldSpaceY + grid.CellSize - pathFindCellBounds);
    }

    public void InitalizeNeighbourNodes()
    {
        NeighbourNodes = new List<PathNode>();

        //Left Neighbour Nodes
        if (x - 1 >= 0)
        {
            NeighbourNodes.Add(grid.GetObject(x - 1, y));

            if (y - 1 >= 0) NeighbourNodes.Add(grid.GetObject(x - 1, y - 1));

            if (y + 1 < grid.Height) NeighbourNodes.Add(grid.GetObject(x - 1, y + 1));
        }

        //Right Neighbour Nodes
        if (x + 1 < grid.Width)
        {
            NeighbourNodes.Add(grid.GetObject(x + 1, y));

            if (y - 1 >= 0) NeighbourNodes.Add(grid.GetObject(x + 1, y - 1));

            if (y + 1 < grid.Height) NeighbourNodes.Add(grid.GetObject(x + 1, y + 1));
        }

        //Up Neighbnour Node
        if (y - 1 >= 0) NeighbourNodes.Add(grid.GetObject(x, y - 1));
        //Down Neighbour Node
        if (y + 1 < grid.Height) NeighbourNodes.Add(grid.GetObject(x, y + 1));
    }

    public void RenderNotWalkableMarker()
    {
        if (grid.DebugInfo)
            if (!IsWalkable)
                debugNotWalkableMarker = Utils.CreateSpriteObject
                    (
                        "AINotWalkableMarker",
                        "Debug_AI_None_Walkable",
                        grid.GetCenterOfCellPosition(x, y),
                        grid.CellSize
                    );
            else GameObject.Destroy(debugNotWalkableMarker);
    }

    public override string ToString() { return $"{x}, {y}"; }
}
