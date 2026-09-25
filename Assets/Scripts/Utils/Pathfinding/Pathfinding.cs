using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Pathfinding
{
    public uint Id { get; private set; }

    public Grid<PathNode> Grid { get; }

    private List<PathNode> openList;
    private List<PathNode> closeList;

    private const int STRIGHT_COST = 10;
    private const int DIAGONAL_COST = 14;
    
    public Pathfinding(Grid<PathNode> grid, uint id = 0, bool debug = false) 
    {
        this.Id = id;
        Grid = grid;

        for (int x = 0; x < Grid.Width; x++)
            for (int y = 0; y < Grid.Height; y++)
                Grid.GetObject(x, y).InitalizeNeighbourNodes();
    }

    public Pathfinding(int width, int height, float cellSize, Vector3 location, GameObject gameObject, bool isGridMoving, uint id = 0, bool debug = false)
    {
        this.Id = id;

        Grid = new Grid<PathNode>
        (
            width, 
            height, 
            cellSize, 
            location, 
            (Grid<PathNode> grid, int x, int y) => new PathNode(grid, x, y),
            gameObject,
            isGridMoving,
            debug
       );

        for (int x = 0; x < Grid.Width; x++)
            for (int y = 0; y < Grid.Height; y++)
                Grid.GetObject(x, y).InitalizeNeighbourNodes();
    }

    /// <summary>
    /// Finds path from start node at grid space X Y to end node at grid space X Y
    /// </summary>
    /// <param name="startX"></param>
    /// <param name="startY"></param>
    /// <param name="endX"></param>
    /// <param name="endY"></param>
    /// <returns></returns>
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = Grid.GetObject(startX, startY);
        PathNode endNode = Grid.GetObject(endX, endY);

        if (startNode == null || endNode == null) return null;

        openList = new List<PathNode> { startNode };
        closeList = new List<PathNode>();

        for (int x = 0; x < Grid.Width; x++) 
            for (int y = 0; y < Grid.Height; y++)
            {
                PathNode pathNode = Grid.GetObject(x, y);
                pathNode.GCost = int.MaxValue;
                pathNode.CameFromNode = null;
            }

        startNode.GCost = 0;
        startNode.HCost = CalculateDistanceCost(startNode, endNode);

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestNodeFCost(openList);
            
            if (currentNode == endNode) return CalculatePath(endNode);

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach (PathNode neighbourNode in currentNode.NeighbourNodes)
            {
                if (closeList.Contains(neighbourNode)) continue;
                if (!neighbourNode.IsWalkable)
                {
                    closeList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.GCost)
                {
                    neighbourNode.CameFromNode = currentNode;
                    neighbourNode.GCost = tentativeGCost;
                    neighbourNode.HCost = CalculateDistanceCost(neighbourNode, endNode);

                    if (!openList.Contains(neighbourNode)) openList.Add(neighbourNode);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Finds path from the two given nodes
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="endNode"></param>
    /// <returns></returns>
    public List<PathNode> FindPath(PathNode startNode, PathNode endNode)
    {
        if (startNode == null || endNode == null) return null;

        openList = new List<PathNode> { startNode };
        closeList = new List<PathNode>();

        for (int x = 0; x < Grid.Width; x++)
            for (int y = 0; y < Grid.Height; y++)
            {
                PathNode pathNode = Grid.GetObject(x, y);
                pathNode.GCost = int.MaxValue;
                pathNode.CameFromNode = null;
            }

        startNode.GCost = 0;
        startNode.HCost = CalculateDistanceCost(startNode, endNode);

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestNodeFCost(openList);

            if (currentNode == endNode) return CalculatePath(endNode);

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach (PathNode neighbourNode in currentNode.NeighbourNodes)
            {
                if (closeList.Contains(neighbourNode)) continue;
                if (!neighbourNode.IsWalkable)
                {
                    closeList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.GCost)
                {
                    neighbourNode.CameFromNode = currentNode;
                    neighbourNode.GCost = tentativeGCost;
                    neighbourNode.HCost = CalculateDistanceCost(neighbourNode, endNode);

                    if (!openList.Contains(neighbourNode)) openList.Add(neighbourNode);
                }
            }
        }

        return null;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.CameFromNode != null)
        {
            //Debug.Log($"X:{currentNode.CameFromNode.x} Y:{currentNode.CameFromNode.y}");

            path.Add(currentNode.CameFromNode);
            currentNode = currentNode.CameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + STRIGHT_COST * remaining;
    }

    private PathNode GetLowestNodeFCost(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
            if (pathNodeList[i].FCost < lowestFCostNode.FCost)
                lowestFCostNode = pathNodeList[i];

        return lowestFCostNode;
    }
}