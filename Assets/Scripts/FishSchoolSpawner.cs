// =============================================================================
// FishSchoolSpawner.cs
// Spawns fish school prefabs across a grid of columns and rows for each level.
// =============================================================================

using UnityEngine;
using System.Collections.Generic;

public class FishSchoolSpawner : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Inspector Fields
    // -------------------------------------------------------------------------

    [Header("Rows")]
    public float[] rowYPositions = new float[3] { -370f, -270f, -170f };

    [Header("School Prefabs")]
    public GameObject[] fishSchoolPrefabs;

    [Header("Spawn")]
    public float spawnChancePerRow = 0.5f;
    public float startX = 350f;
    public float columnSpacing = 200f;
    public int columnCount = 16;

    // -------------------------------------------------------------------------
    // Private State
    // -------------------------------------------------------------------------

    private int currentLevel = 1;
    private List<GameObject> spawnedSchools = new List<GameObject>();

    // -------------------------------------------------------------------------
    // Public API
    // -------------------------------------------------------------------------

    // Store the current level and trigger a fresh school spawn
    public void SpawnForLevel(int level)
    {
        currentLevel = level;
        SpawnSchools();
    }

    // -------------------------------------------------------------------------
    // Spawning
    // -------------------------------------------------------------------------

    // Destroy existing schools then spawn new ones across even columns
    void SpawnSchools()
    {
        foreach (GameObject school in spawnedSchools)
        {
            if (school != null)
            {
                Destroy(school);
            }
        }

        spawnedSchools.Clear();

        GameObject prefab = GetPrefabForLevel(currentLevel);

        if (prefab == null)
        {
            return;
        }

        for (int col = 0; col < columnCount; col++)
        {
            if (col % 2 != 0)
            {
                continue;
            }

            float xPos = startX + col * columnSpacing;
            bool[] occupied = DecideOccupiedRows();

            for (int row = 0; row < rowYPositions.Length; row++)
            {
                if (occupied[row])
                {
                    SpawnSchool(prefab, xPos, rowYPositions[row]);
                }
            }
        }
    }

    // Return the prefab for the given level, falling back to level 1 if missing
    GameObject GetPrefabForLevel(int level)
    {
        if (fishSchoolPrefabs == null || fishSchoolPrefabs.Length == 0)
        {
            return null;
        }

        int index = level - 1;

        if (index >= 0 && index < fishSchoolPrefabs.Length && fishSchoolPrefabs[index] != null)
        {
            return fishSchoolPrefabs[index];
        }

        return fishSchoolPrefabs[0];
    }

    // Randomly assign rows as occupied, ensuring at least one and at most all-but-one are filled
    bool[] DecideOccupiedRows()
    {
        bool[] occupied = new bool[rowYPositions.Length];

        for (int index = 0; index < occupied.Length; index++)
        {
            occupied[index] = Random.value < spawnChancePerRow;
        }

        int schoolCount = CountTrue(occupied);

        if (schoolCount == 0)
        {
            occupied[Random.Range(0, occupied.Length)] = true;
        }

        if (schoolCount == occupied.Length)
        {
            occupied[Random.Range(0, occupied.Length)] = false;
        }

        return occupied;
    }

    // Instantiate a prefab at the given position and register it for cleanup
    void SpawnSchool(GameObject prefab, float xPosition, float yPosition)
    {
        Vector3 spawnPos = new Vector3(xPosition, yPosition, transform.position.z);
        GameObject school = Instantiate(prefab, spawnPos, Quaternion.identity);
        spawnedSchools.Add(school);
    }

    // Count the number of true values in a bool array
    int CountTrue(bool[] array)
    {
        int count = 0;

        foreach (bool value in array)
        {
            if (value)
            {
                count++;
            }
        }

        return count;
    }
}