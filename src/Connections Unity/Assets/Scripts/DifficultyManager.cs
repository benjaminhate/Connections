using Objects;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public Difficulty difficulty;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}