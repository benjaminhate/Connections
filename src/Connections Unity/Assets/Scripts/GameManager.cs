using System;
using Objects;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public Difficulty difficulty;

    private void Start()
    {
        var newSeed = new Random().Next(int.MinValue, int.MaxValue);
        gridManager.seed = newSeed;
        gridManager.difficulty = difficulty;
        gridManager.StartGrid();
    }

    private void OnMouseUp()
    {
        Debug.Log("Test");
    }
}