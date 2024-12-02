using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public GameSettings gameSettings;
    public int score = 0;
    public GUIController guiController;
    public ThreeWaveFunctionCollapse wfc;

    // Method to add points to the score
    public void AddScore(int points)
    {
        score += points;
        guiController.updateStarCount(score);
        guiController.addMoreTime(30f);
        Debug.Log("Score: " + score);
        if (score == wfc.maxObjects)
        {
            gameSettings.maxObjects++;
            if (gameSettings.maxCellPerGrid < 10)
            {
                gameSettings.maxCellPerGrid++;
            }
            guiController.StartFadeOutAndRestart();
            guiController.updateCurrentTime();
        }
    }
    public int getScore()
    {
        return score;
    }
}
