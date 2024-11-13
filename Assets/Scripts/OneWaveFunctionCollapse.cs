using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWaveFunctionCollapse : MonoBehaviour
{
    [System.Serializable]
    public class Tile
    {
        public string name;
        public int id;
        public List<int> allowedNeighbors;
    }

    public List<Tile> tiles;
    public int lineLength = 10; // length of the 1D grid
    private List<HashSet<int>> possibleTiles; // the possible tile that each position will be placed

    private void Start()
    {
        InitializeGrid();
        RunWFC();
        PrintResult();
    }
    void InitializeGrid()
    {
        possibleTiles = new List<HashSet<int>>();
        for (int i = 0; i < lineLength; i++)
        {
            HashSet<int> initialOptions = new HashSet<int>();
            foreach (Tile tile in tiles)
            {
                initialOptions.Add(tile.id);
            }
            possibleTiles.Add(initialOptions);
        }
    }

    bool IsCollapsed()
    {
        // check if each cell in the grid has one option left
        foreach (var options in possibleTiles)
        {
            if (options.Count > 1) { return false; }
        }
        return true;
    }

    int FindCellWithFewestOptions()
    {
        int minOptions = int.MaxValue;
        int minIndex = -1;
        for (int i = 0; i < possibleTiles.Count; i++)
        {
            int optionCount = possibleTiles[i].Count;
            if (optionCount > 1 && optionCount < minOptions)
            {
                minOptions = optionCount;
                minIndex = i;
            }
        }
        return minIndex;
    }

    void CollapseTile(int index)
    {
        var options = possibleTiles[index];
        int chosenTile = Random.Range(0, options.Count);
        // set the chosen tile and remove all other possibilities
        int selectedTileID = -1;
        foreach (int option in options)
        {
            if (chosenTile == 0)
            {
                selectedTileID = option;
                break;
            }
            chosenTile--;
        }
        options.Clear();
        options.Add(selectedTileID);
    }

    bool RestrictOptions(int index, int neighborTileId)
    {
        var options = possibleTiles[index];
        Tile neighborTile = tiles.Find(t => t.id == neighborTileId);
        bool optionsChanged = false;

        HashSet<int> allowedOptions = new HashSet<int>(neighborTile.allowedNeighbors);
        foreach (int option in new List<int>(options))
        {
            if (!allowedOptions.Contains(option))
            {
                options.Remove(option);
                optionsChanged = true;
            }
        }
        return optionsChanged;
    }

    int GetCollapsedTile(int index)
    {
        if (possibleTiles[index].Count == 1)
        {
            foreach (int tileId in possibleTiles[index])
            {
                return tileId;
            }
        }
        return -1;
    }

    void PropagateConstraints(int index)
    {
        Queue<int> propagationQueue = new Queue<int>();
        propagationQueue.Enqueue(index);

        while (propagationQueue.Count > 0)
        {
            int currentIndex = propagationQueue.Dequeue();
            int tileId = GetCollapsedTile(currentIndex);

            // Propagate constraints to neighbors
            foreach (int neighbor in new int[] { currentIndex - 1, currentIndex + 1 })
            {
                if (neighbor >= 0 && neighbor < lineLength)
                {
                    if (RestrictOptions(neighbor, tileId))
                    {
                        propagationQueue.Enqueue(neighbor);
                    }
                }
            }
        }
    }

    void RunWFC()
    {
        // while each cell has multiple possible options
        while (!IsCollapsed())
        {
            int minOptionsIndex = FindCellWithFewestOptions();
            if (minOptionsIndex == -1) break;

            // Collapse a random tile in this position
            CollapseTile(minOptionsIndex);

            // Propagate constraints to neighboring cells
            PropagateConstraints(minOptionsIndex);
        }
    }
    void PrintResult()
    {
        Debug.Log("Final 1D Wave Function Collapse Result:");
        for (int i = 0; i < lineLength; i++)
        {
            int tileId = GetCollapsedTile(i);
            Tile tile = tiles.Find(t => t.id == tileId);
            Debug.Log("Position " + i + ": " + tile.name);
        }
    }
}
