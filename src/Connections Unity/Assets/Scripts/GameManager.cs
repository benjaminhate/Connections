using System.Collections;
using System.Linq;
using Objects;
using ScriptableObjects;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public Difficulty difficulty;
    public GameConfiguration gameConfiguration;
    public Camera gameCamera;

    private void Start()
    {
        GetDifficulty();
        ConfigureGame();
        StartNewGrid();
    }

    private void GetDifficulty()
    {
        var difficultyManager = FindObjectOfType<DifficultyManager>();
        if (difficultyManager != null)
        {
            difficulty = difficultyManager.difficulty;
        }
    }

    private void ConfigureGame()
    {
        var difficultyConfig = gameConfiguration.GetConfiguration(difficulty);

        gameCamera.orthographicSize = difficultyConfig.cameraSize;
        gameCamera.transform.position = new Vector3(difficultyConfig.cameraX, difficultyConfig.cameraY, -10);
    }

    private void StartNewGrid()
    {
        var newSeed = new Random().Next(int.MinValue, int.MaxValue);
        gridManager.seed = newSeed;
        gridManager.difficulty = difficulty;
        gridManager.StartGrid();

        foreach (var gridManagerBrick in gridManager.Bricks)
        {
            gridManagerBrick.MouseUpEvent += OnBrickMouseUp;
        }
    }

    private void OnBrickMouseUp()
    {
        var allBricksValid = gridManager.grid.content.All(gridManager.grid.CheckBrickConnectors);
        if (allBricksValid)
        {
            gridManager.StopGrid();
            StartCoroutine(Reset());
        }
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(1f);
        StartNewGrid();
    }
}