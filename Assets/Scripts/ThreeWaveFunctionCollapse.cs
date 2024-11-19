using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeWaveFunctionCollapse : MonoBehaviour
{
    [System.Serializable]
    public class Tile
    {
        public GameObject tilePrefab;
        public List<int> allowedNeighbors; // IDs of tiles that can be adjacent
    }

    public List<Tile> tiles;
    public int lineLength = 10;
    public float tileSpacing = 2f; // Space between each tile

    private List<HashSet<int>> possibleTiles; // Possible tiles for each position in the grid
    private int gridWidth;  // X dimension of the grid
    private int gridHeight; // Y dimension of the grid
    private int gridDepth;  // Z dimension of the grid

    private void Start()
    {
        gridWidth = this.lineLength;
        gridHeight = this.lineLength;
        gridDepth = this.lineLength;
        InitializeGrid();
        RunWFC();
        InstantiateTiles3D();
    }

    void InitializeGrid()
    {
        possibleTiles = new List<HashSet<int>>();
        for (int i = 0; i < gridWidth * gridHeight * gridDepth; i++)
        {
            HashSet<int> initialOptions = new HashSet<int>();
            for (int j = 0; j < tiles.Count; j++)
            {
                initialOptions.Add(j); // Use the index as the ID
            }
            possibleTiles.Add(initialOptions);
        }
    }

    bool IsCollapsed()
    {
        foreach (var options in possibleTiles)
        {
            if (options.Count > 1) return false;
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
        Tile neighborTile = tiles[neighborTileId];
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

            if (tileId == -1) continue;

            // Calculate neighbors in 3D (top, bottom, left, right, front, back)
            int x = currentIndex % gridWidth;
            int y = (currentIndex / gridWidth) % gridHeight;
            int z = currentIndex / (gridWidth * gridHeight);

            int[] neighbors = {
                (y < gridHeight - 1) ? currentIndex + gridWidth : -1,             // Top
                (y > 0) ? currentIndex - gridWidth : -1,                          // Bottom
                (x > 0) ? currentIndex - 1 : -1,                                  // Left
                (x < gridWidth - 1) ? currentIndex + 1 : -1,                      // Right
                (z < gridDepth - 1) ? currentIndex + gridWidth * gridHeight : -1, // Front
                (z > 0) ? currentIndex - gridWidth * gridHeight : -1              // Back
            };

            foreach (int neighbor in neighbors)
            {
                if (neighbor != -1 && RestrictOptions(neighbor, tileId))
                {
                    propagationQueue.Enqueue(neighbor);
                }
            }
        }
    }

    void RunWFC()
    {
        while (!IsCollapsed())
        {
            int minOptionsIndex = FindCellWithFewestOptions();
            if (minOptionsIndex == -1) break;

            CollapseTile(minOptionsIndex);
            PropagateConstraints(minOptionsIndex);
        }
    }

    void InstantiateTiles3D()
    {
        for (int z = 0; z < gridDepth; z++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    int index = z * gridWidth * gridHeight + y * gridWidth + x;
                    int tileId = GetCollapsedTile(index);

                    if (tileId != -1)
                    {
                        GameObject tilePrefab = tiles[tileId].tilePrefab;
                        Vector3 position = transform.position + new Vector3(x * tileSpacing, y * tileSpacing, z * tileSpacing);
                        Instantiate(tilePrefab, position, Quaternion.identity, transform);
                    }
                }
            }
        }
    }
}
