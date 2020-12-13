using System.Collections.Generic;
using Objects;
using ScriptableObjects;
using Unity.Mathematics;
using UnityEngine;
using Grid = Objects.Grid;

public class GridManager : MonoBehaviour
{
    [Header("Start")]
    public bool startOnLoad;
    
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
    public Difficulty difficulty;
    public DifficultyBricks difficultyBricks;
    public Grid grid;

    public List<BrickManager> Bricks { get; private set; }

    private void Start()
    {
        if (startOnLoad)
        {
            StartGrid();
        }
    }

    public void StartGrid()
    {
        ClearGrid();
        GenerateGrid();
        CreateGrid();
    }

    public void StopGrid()
    {
        foreach (var brick in Bricks)
        {
            brick.Stop();
        }
    }

    private void ClearGrid()
    {
        Bricks = new List<BrickManager>();
        foreach (Transform child in brickParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in backgroundParent)
        {
            Destroy(child.gameObject);
        }
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
            grid.GenerateLevel(seed, difficulty, difficultyBricks);
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
        
        Bricks.Add(newBrick);
    }

    private Brick GetBrickAtPosition(Vector2 position)
    {
        return grid.content.Find(b => b.position == position);
    }

    public bool MoveBrickToPosition(Transform brick, Vector2 brickNewPosition, Vector2 brickLastPosition)
    {
        var isBrickAlreadyInPosition = grid.content.Exists(b => b.position == brickNewPosition);

        var newPosition = isBrickAlreadyInPosition ? brickLastPosition : brickNewPosition;
        GetBrickAtPosition(brickLastPosition).position = newPosition;
        brick.localPosition = newPosition;
        return !isBrickAlreadyInPosition;
    }

    public void RotateBrick(Transform brick, Transform connectorParent, Direction rotationDirection)
    {
        var rotation = Quaternion.Euler(0, 0, rotationDirection.ToAngleRotation());
        connectorParent.localRotation = rotation;
        GetBrickAtPosition(brick.localPosition).facingDirection = rotationDirection;
    }
}
