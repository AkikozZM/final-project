using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeWaveFunctionCollapse : MonoBehaviour
{
    [System.Serializable]
    public class Tile
    {
        public GameObject tilePrefab;
        //public List<int> allowedNeighbors;
        public List<int> allowedNeighborsX;
        public List<int> allowedNeighborsY;
        public List<int> allowedNeighborsZ;
        public bool requiresEmptyAbove; // check if a tile needs empty space above
        public bool canGenerateStars = true;
        public int maxOccurrencesPerFloor = 2;
    }
    public GameSettings gameSettings;
    public GameObject starPrefab;
    public int maxObjects;
    public List<Tile> tiles;
    public int lineLength = 4; // max cell per grid
    public float tileSpacing = 2f; // Space between each tile

    private List<HashSet<int>> possibleTiles; // Possible tiles for each position in the grid
    private int gridWidth;  // X dimension of the grid
    private int gridHeight; // Y dimension of the grid
    private int gridDepth;  // Z dimension of the grid
    private Dictionary<int, int[]> floorTileCounts;

    private void Start()
    {
        lineLength = gameSettings.maxCellPerGrid;
        maxObjects = gameSettings.maxObjects;
        gridWidth = this.lineLength;
        gridHeight = this.lineLength;
        gridDepth = this.lineLength;
        InitializeGrid();
        RunWFC();
        InstantiateTiles3D();
        GenerateRandomObjectsOnTiles();
    }
    public int getMaxObjects()
    {
        return maxObjects;
    }
    void GenerateRandomObjectsOnTiles()
    {
        List<int> availableTileIndices = new List<int>();
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                for (int z = 0; z < gridDepth; z++)
                {
                    int index = z * gridWidth * gridHeight + y * gridWidth + x;
                    if (GetCollapsedTile(index) != -1 && tiles[GetCollapsedTile(index)].canGenerateStars)
                    {
                        availableTileIndices.Add(index);
                    }
                }
            }
        }
        int objectsPlaced = 0;
        while (objectsPlaced < maxObjects && availableTileIndices.Count > 0)
        {
            // Choose a random tile from the available tiles
            int randomIndex = Random.Range(0, availableTileIndices.Count);
            int tileIndex = availableTileIndices[randomIndex];
            availableTileIndices.RemoveAt(randomIndex); // Remove the chosen tile to avoid duplicates

            // Get the tile's position in the world
            int x = tileIndex % gridWidth;
            int y = (tileIndex / gridWidth) % gridHeight;
            int z = tileIndex / (gridWidth * gridHeight);
            Vector3 position = transform.position + new Vector3(x * tileSpacing, y * tileSpacing, z * tileSpacing);
            Instantiate(starPrefab, position, Quaternion.identity, transform);
            objectsPlaced++;
        }
    }

    void InitializeGrid()
    {
        possibleTiles = new List<HashSet<int>>();
        floorTileCounts = new Dictionary<int, int[]>();
        for (int i = 0; i < gridWidth * gridHeight * gridDepth; i++)
        {
            HashSet<int> initialOptions = new HashSet<int>();
            for (int j = 0; j < tiles.Count; j++)
            {
                initialOptions.Add(j); // Use the index as the ID
            }
            possibleTiles.Add(initialOptions);
        }
        // Initialize floor appearance counters for each tile
        for (int y = 0; y < gridHeight; y++)
        {
            floorTileCounts[y] = new int[tiles.Count];
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
        int y = (index / gridWidth) % gridHeight;
        floorTileCounts[y][selectedTileID]++;
        options.Clear();
        options.Add(selectedTileID);
    }

    bool RestrictOptions(int index, int neighborTileId, List<int> allowedNeighbors)
    {
        var options = possibleTiles[index];
        Tile neighborTile = tiles[neighborTileId];
        bool optionsChanged = false;

        HashSet<int> allowedOptions = new HashSet<int>(allowedNeighbors);
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

            // Limit certain tiles' occurrences per floor
            if (floorTileCounts[y][tileId] >= tiles[tileId].maxOccurrencesPerFloor)
            {
                possibleTiles[currentIndex].Remove(tileId);
                continue;
            }

            // Ensure specific tiles have an empty space above them
            if (tiles[tileId].requiresEmptyAbove && y < gridHeight - 1)
            {
                int aboveIndex = currentIndex + gridWidth;
                if (possibleTiles[aboveIndex].Count > 0)
                {
                    possibleTiles[currentIndex].Remove(tileId);
                    continue;
                }
            }

            int[] neighbors = {
                (y < gridHeight - 1) ? currentIndex + gridWidth : -1,             // Top
                (y > 0) ? currentIndex - gridWidth : -1,                          // Bottom
                (x > 0) ? currentIndex - 1 : -1,                                  // Left
                (x < gridWidth - 1) ? currentIndex + 1 : -1,                      // Right
                (z < gridDepth - 1) ? currentIndex + gridWidth * gridHeight : -1, // Front
                (z > 0) ? currentIndex - gridWidth * gridHeight : -1              // Back
            };

            List<int>[] allowedNeighbors = {
                tiles[tileId].allowedNeighborsY, // Top
                tiles[tileId].allowedNeighborsY, // Bottom
                tiles[tileId].allowedNeighborsX, // Left
                tiles[tileId].allowedNeighborsX, // Right
                tiles[tileId].allowedNeighborsZ, // Front
                tiles[tileId].allowedNeighborsZ  // Back
            };

            for (int i = 0; i < neighbors.Length; i++)
            {
                int neighborIndex = neighbors[i];
                if (neighborIndex != -1)
                {
                    if (RestrictOptions(neighborIndex, tileId, allowedNeighbors[i]))
                    {
                        propagationQueue.Enqueue(neighborIndex);
                    }
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
                        // Generate a random Y-axis rotation (0, 90, 180, or 270 degrees)
                        Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
                        Instantiate(tilePrefab, position, rotation, transform);
                    }
                }
            }
        }
    }
}
