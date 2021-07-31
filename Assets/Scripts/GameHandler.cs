using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public int Level { get; set;} 
    public int Rows { get; set;} 
    public int Score { get; set;} 

    [SerializeField] Text levelText;
    [SerializeField] Text rowsText;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject menu;

    public static readonly int[] LINE_POINTS = { 40, 100, 300, 1200 };

    public void NewGame()
    {
        ShowMenu(false);

        Level = 1;
        levelText.text  = Level.ToString();
        Rows = 0;
        rowsText.text  = Rows.ToString();
        Score = 0;
        scoreText.text  = Score.ToString();

        BlockHandler.ResetGrid();
        GameObject[] tetromino = GameObject.FindGameObjectsWithTag("Tetromino");

        foreach(GameObject item in tetromino)
        {
            Destroy(item);
        }

        FindObjectOfType<Spawn>().NewTetromino();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowMenu(bool show)
    {
        menu.SetActive(show);
    }

    public void IncreaseRows(int amount)
    {
        Rows += amount;
        rowsText.text  = Rows.ToString();
        IncreaseScore(amount);
        if (Rows > (Level + 1) * 10) {
            IncreaseLevel();
        }
    }

    private void IncreaseScore(int lines) {
        Score += LINE_POINTS[lines - 1];
        scoreText.text  = Score.ToString();
    }

    private void IncreaseLevel() {
        Level += 1;
        levelText.text  = Level.ToString();
    }
}
