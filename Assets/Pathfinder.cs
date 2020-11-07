using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();

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
        ExploreNeighbours();
    }

    private void ExploreNeighbours()
    {
        foreach (Vector2Int direction in directions) 
        {
            Vector2Int explorationCoordinates = start.GetGridPosition() + direction;
            try
            {
                grid[explorationCoordinates].SetTopColor(Color.blue);
            }
            catch
            {
                print("Empty Index");
            }
        }
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
