using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = Objects.Grid;

public class GridManager : MonoBehaviour
{
    [Header("Background")]
    public GameObject backgroundGridPrefab;
    public Transform backgroundParent;

    [Header("Grid")]
    public Grid grid;

    public BrickManager brickPrefab;

    public Transform brickParent;

    private void Start()
    {
        CreateGrid();
        GenerateGrid();
    }

    private void CreateGrid()
    {
        var height = grid.height;
        var width = grid.width;

        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                var backgroundGrid = Instantiate(backgroundGridPrefab, backgroundParent);
                backgroundGrid.transform.localPosition = new Vector3(i, j, 0);
            }
        }
    }

    private void GenerateGrid()
    {
        foreach (var brick in grid.content)
        {
            var newBrick = Instantiate(brickPrefab, brickParent);
            newBrick.transform.localPosition = brick.position;
            newBrick.type = brick.type;
            newBrick.initialFacingDirection = brick.facingDirection;
            newBrick.camera = Camera.main;
            newBrick.grid = this;
        }
    }
}
