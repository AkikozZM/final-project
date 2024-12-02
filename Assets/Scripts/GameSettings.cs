using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/Settings", order = 1)]
public class GameSettings : ScriptableObject
{
    public int maxObjects = 1;
    public int maxCellPerGrid = 4;
    public float currentTime = 60f;
}