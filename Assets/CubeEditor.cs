using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
public class CubeEditor : MonoBehaviour
{

    TextMesh textMesh;
    Vector3 gridPos;
    Waypoint waypoint;

    private void Awake()
    {
        waypoint = GetComponent<Waypoint>();
    }

    void Update()
    {
        SnapToGrid();
        UpdateLabel();
    }

    private void SnapToGrid()
    {
        int gridSize = waypoint.GetGridSize();
        transform.position = new Vector3(
            waypoint.GetGridPosition().x * gridSize,
            0f,
            waypoint.GetGridPosition().y * gridSize
            );
    }

    private void UpdateLabel()
    {
        int gridSize = waypoint.GetGridSize();
        textMesh = GetComponentInChildren<TextMesh>();
        string labelText = waypoint.GetGridPosition().x + "," + waypoint.GetGridPosition().y;
        textMesh.text = labelText;
        gameObject.name = labelText;
    }
}
