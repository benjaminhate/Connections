using System;
using System.Collections.Generic;
using System.Linq;
using Objects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public DifficultyManager difficultyManager;
    public TMP_Dropdown difficultyDropdown;
    
    private void Start()
    {
        SetDifficultyDropdown();
    }

    private void SetDifficultyDropdown()
    {
        var names = Enum.GetNames(typeof(Difficulty)).ToList();
        difficultyDropdown.AddOptions(names);
    }

    public void OnDifficultyDropdownValueChange(int value)
    {
        var difficultyOption = (Difficulty) value;
        difficultyManager.difficulty = difficultyOption;
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
