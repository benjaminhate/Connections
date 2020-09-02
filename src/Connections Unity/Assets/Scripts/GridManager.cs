using System.Linq;
using Objects;
using UnityEngine;
using Grid = Objects.Grid;

public class GridManager : MonoBehaviour
{
    [Header("Background")]
    public GameObject backgroundGridPrefab;
    public Transform backgroundParent;

    [Header("Brick")]
    public BrickManager brickPrefab;
    public Transform brickParent;

    [Header("Grid")] 
    public bool proceduralGenerateGrid;
    public bool randomizeGrid;
    public int seed;
    public Grid grid;

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
        if (proceduralGenerateGrid)
        {
            grid.GenerateLevel(seed);
        }

        if (randomizeGrid)
        {
            grid.RandomizeLevel(seed);   
        }

        foreach (var brick in grid.content)
        {
            GenerateBrick(brick);
        }
    }

    private void GenerateBrick(Brick brick)
    {
        var newBrick = Instantiate(brickPrefab, brickParent);
        newBrick.transform.localPosition = brick.position;
        newBrick.type = brick.type;
        newBrick.initialFacingDirection = brick.facingDirection;
        newBrick.mainCamera = Camera.main;
        newBrick.gridManager = this;
        newBrick.CreateConnectors(brick.connectors);
    }

    public bool MoveBrickToPosition(Transform brick, Vector2 brickNewPosition, Vector2 brickLastPosition)
    {
        var isBrickAlreadyInPosition = grid.content.Exists(b => b.position == brickNewPosition);

        var newPosition = isBrickAlreadyInPosition ? brickLastPosition : brickNewPosition;
        grid.content.Find(b => b.position == brickLastPosition).position = newPosition;
        brick.localPosition = newPosition;
        return !isBrickAlreadyInPosition;
    }
}
