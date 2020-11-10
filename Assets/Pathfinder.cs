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

    Vector2Int[] directions = {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
        };

    [SerializeField] Waypoint start;
    [SerializeField] Waypoint end;

    private void Start()
    {
        LoadBlocks();
        SetCheckpointsColors();
        Pathfind();
        //ExploreNeighbours();
    }

    private void Pathfind()
    {
        queue.Enqueue(start);

        while (queue.Count > 0 && isRunning)
        {
            searchCenter = queue.Dequeue();
            searchCenter.isExplored = true;
            print("Searching from " + searchCenter); // todo remove log 
            if (CheckEndFound()) return;
            ExploreNeighbours();
        }

        print("Finished pathfinding");
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
            print("neighbout " + waypoint + " is end.");
            isRunning = false;
        }
    }

    private void ExploreNeighbours()
    {
        if (!isRunning) return;

        print("Exploring");
        foreach (Vector2Int direction in directions) 
        {
            if (!isRunning) return;
            Vector2Int neighbourCoordinates = searchCenter.GetGridPosition() + direction;
            try
            {
                QueueNewNeighbours(neighbourCoordinates);
            }
            catch
            {
                print("Empty Index");
            }
        }
    }

    private void QueueNewNeighbours(Vector2Int neighbourCoordinates)
    {
        Waypoint neighbour = grid[neighbourCoordinates];
        if (neighbour.isExplored) return;
        neighbour.SetTopColor(Color.grey);
        queue.Enqueue(neighbour);
        neighbour.exploredFrom = searchCenter;
        neighbour.isExplored = true;
        print("Queueing " + neighbour);
        CheckEndFound(neighbour);
    }

    public void SetCheckpointsColors()
    {
        start.SetTopColor(Color.white);
        end.SetTopColor(Color.red);
    }

    private void LoadBlocks()
    {
        var waypoints = FindObjectsOfType<Waypoint>();
        foreach (Waypoint waypoint in waypoints)
        {
            bool isOverlapping = grid.ContainsKey(waypoint.GetGridPosition());
            if (isOverlapping)
            {
                Debug.Log("Skiping overlapping block " + waypoint);
            }
            else
            {
                grid.Add(waypoint.GetGridPosition(), waypoint);
                waypoint.SetTopColor(Color.black);
            }
        }
        Debug.Log("Loaded " + grid.Count + " blocks");

    }
}
