using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWaveFunctionCollapse : MonoBehaviour
{
    [System.Serializable]
    public class Tile
    {
        public GameObject tilePrefab;
        //public int id;
        public List<int> allowedNeighbors;
    }

    public List<Tile> tiles;
    public int lineLength = 10; // length of the grid col = row
    public float tileSpacing = 1f;
    private List<HashSet<int>> possibleTiles; // the possible tile that each position will be placed

    private void Start()
    {
        InitializeGrid();
        RunWFC();
        InstantiateTiles2D();
    }
    void InitializeGrid()
    {
        possibleTiles = new List<HashSet<int>>();
        int size = lineLength * lineLength;
        for (int i = 0; i < size; i++)
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

            // Calculate neighbors in 2D (top, bottom, left, right)
            int row = currentIndex / lineLength;
            int col = currentIndex % lineLength;

            int[] neighbors = {
                (row > 0) ? currentIndex - lineLength : -1, // Top
                (row < lineLength - 1) ? currentIndex + lineLength : -1, // Bottom
                (col > 0) ? currentIndex - 1 : -1, // Left
                (col < lineLength - 1) ? currentIndex + 1 : -1 // Right
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

    void InstantiateTiles1D()
    {
        // Instantiate tiles based on the collapsed grid
        for (int i = 0; i < lineLength; i++)
        {
            int tileId = GetCollapsedTile(i);
            if (tileId != -1)
            {
                GameObject tilePrefab = tiles[tileId].tilePrefab;
                Vector3 position = transform.position + Vector3.right * i * tileSpacing;
                Instantiate(tilePrefab, position, Quaternion.identity, transform);
            }
        }
    }
    void InstantiateTiles2D()
    {
        // Loop over each cell in a 10x10 grid
        for (int row = 0; row < lineLength; row++)
        {
            for (int col = 0; col < lineLength; col++)
            {
                int index = row * lineLength + col;
                int tileId = GetCollapsedTile(index);

                if (tileId != -1)
                {
                    GameObject tilePrefab = tiles[tileId].tilePrefab;

                    // Calculate the position based on row and column
                    Vector3 position = transform.position + new Vector3(col * tileSpacing, 0, row * tileSpacing);

                    // Instantiate the tile at the calculated position
                    Instantiate(tilePrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }
}
