using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();

    Queue<Waypoint> queue = new Queue<Waypoint>();

    bool isRunning = true;

    Waypoint searchCenter;

    List<Waypoint> path = new List<Waypoint>();

    Vector2Int[] directions = {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
        };

    [SerializeField] Waypoint start;
    [SerializeField] Waypoint end;

    public List<Waypoint> GetPath()
    {
        LoadBlocks();
        ColorCheckpoints();
        BreadthFirstSearch();
        CreatePath();
        return path;
    }

    private void ColorCheckpoints()
    {
        start.SetTopColor(Color.green);
        end.SetTopColor(Color.red);
    }

    private void BreadthFirstSearch()
    {
        queue.Enqueue(start);

        while (queue.Count > 0 && isRunning)
        {
            searchCenter = queue.Dequeue();
            searchCenter.isExplored = true;
            if (CheckEndFound()) return;
            ExploreNeighbours();
        }
    }

    private void CreatePath()
    {
        path.Add(end);
        Waypoint previousWaypoint = end.exploredFrom;

        while(previousWaypoint != start)
        {
            previousWaypoint.SetTopColor(Color.blue);
            path.Add(previousWaypoint);
            previousWaypoint = previousWaypoint.exploredFrom;
        }
        path.Add(start);
        path.Reverse();
    }

    private bool CheckEndFound()
    {
        if(searchCenter == end)
        {
            print("Start point == End point - Stopping");
            isRunning = false;
            return true;
        }
        return false;
    }

    private void CheckEndFound(Waypoint waypoint)
    {
        if (waypoint == end)
        {
            end.SetTopColor(Color.red);
            isRunning = false;
        }
    }

    private void ExploreNeighbours()
    {
        if (!isRunning) return;

        foreach (Vector2Int direction in directions) 
        {
            if (!isRunning) return;
            Vector2Int neighbourCoordinates = searchCenter.GetGridPosition() + direction;
            if(grid.ContainsKey(neighbourCoordinates)) QueueNewNeighbours(neighbourCoordinates);
        }
    }

    private void QueueNewNeighbours(Vector2Int neighbourCoordinates)
    {
        Waypoint neighbour = grid[neighbourCoordinates];
        if (neighbour.isExplored) return;
        queue.Enqueue(neighbour);
        neighbour.exploredFrom = searchCenter;
        neighbour.isExplored = true;
        neighbour.SetTopColor(Color.grey);
        CheckEndFound(neighbour);
    }

    private void LoadBlocks()
    {
        var waypoints = FindObjectsOfType<Waypoint>();
        foreach (Waypoint waypoint in waypoints)
        {
            bool isOverlapping = grid.ContainsKey(waypoint.GetGridPosition());
            if (isOverlapping)
            {
            }
            else
            {
                grid.Add(waypoint.GetGridPosition(), waypoint);
            }
        }
    }
}
