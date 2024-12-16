using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    // List of towers (prefabs) that will instantiate
    public List<GameObject> towersPrefabs;
    // Transform of the spawning towers (Root Object)
    public Transform spawnTowerRoot;
    // List of towers (UI)
    public List<Image> towersUI;
    // ID of the tower to spawn
    int spawnID = -1;
    // SpawnPoints Tilemap
    public Tilemap spawnTilemap;
    // Tile position
    private Vector3Int tilePos;
    private int currentUpgradeIndex = -1; // Tracks the last upgraded tower

    void Update()
    {
        if (CanSpawn())
        {
            if (spawnTilemap != null)
            {
                DetectSpawnPoint();
            }
            else
            {
                Debug.LogWarning("Tilemap is missing. Skipping spawning logic.");
            }
        }
    }

    bool CanSpawn()
    {
        return spawnID != -1;
    }

    void DetectSpawnPoint()
    {
        // Detect when mouse is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Get the world space position of the mouse
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Get the position of the cell in the tilemap
            var cellPosDefault = spawnTilemap.WorldToCell(mousePos);
            // Get the center position of the cell
            var cellPosCentered = spawnTilemap.GetCellCenterWorld(cellPosDefault);
            // Check if we can spawn in that cell (collider)
            if (spawnTilemap.GetColliderType(cellPosDefault) == Tile.ColliderType.Sprite)
            {
                int towerCost = TowerCost(spawnID);
                // Check if currency is enough to spawn
                if (GameManager.instance.currency.EnoughCurrency(towerCost))
                {
                    // Use the amount of cost from the available currency
                    GameManager.instance.currency.Use(towerCost);
                    // Spawn the tower
                    SpawnTower(cellPosCentered, cellPosDefault);
                    // Disable the collider
                    spawnTilemap.SetColliderType(cellPosDefault, Tile.ColliderType.None);
                }
                else
                {
                    Debug.Log("Not Enough Currency");
                }
            }
        }
    }

    public int TowerCost(int id)
    {
        switch (id)
        {
            case 0: return towersPrefabs[id].GetComponent<Tower_Pink>().cost;
            case 1: return towersPrefabs[id].GetComponent<Tower_Mask>().cost;
            case 2: return towersPrefabs[id].GetComponent<Tower_Ninja>().cost;
            default: return -1;
        }
    }

    void SpawnTower(Vector3 position, Vector3Int cellPosition)
    {
        GameObject tower = Instantiate(towersPrefabs[spawnID], spawnTowerRoot);
        tower.transform.position = position;
        Tower towerComponent = tower.GetComponent<Tower>();
        towerComponent.Init(cellPosition, spawnID);

        // Ensure the tower is added to the list
        GameManager.instance.activeTowers.Add(towerComponent);

        DeselectTowers();
    }


    public void RevertCellState(Vector3Int pos)
    {
        if (spawnTilemap != null) // Check if spawnTilemap exists
        {
            spawnTilemap.SetColliderType(pos, Tile.ColliderType.Sprite);
        }
    }
    
    public void SelectTower(int id)
    {
        DeselectTowers();
        // Set the spawnID
        spawnID = id;
        // Highlight the tower
        towersUI[spawnID].color = Color.white;
    }

    public void DeselectTowers()
    {
        spawnID = -1;
        foreach (var t in towersUI)
        {
            t.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

public void SelectUpgrade(int id)
{
    DeselectTowers();
    towersUI[id].color = Color.white;

    // Filter towers by the selected ID
    List<Tower> towersToUpgrade = GameManager.instance.activeTowers
        .FindAll(tower => tower.towerID == id);

    if (towersToUpgrade.Count == 0)
    {
        Debug.Log($"No towers of type {id} available for upgrade.");
        return;
    }

    // Increment the index and wrap around using modulo
    currentUpgradeIndex = (currentUpgradeIndex + 1) % towersToUpgrade.Count;

    Tower towerToUpgrade = towersToUpgrade[currentUpgradeIndex];

    // Attempt the upgrade
    int upgradeCost = TowerCost(id);
    if (GameManager.instance.currency.EnoughCurrency(upgradeCost))
    {
        if (towerToUpgrade.Upgrade())
        {
            GameManager.instance.currency.Use(upgradeCost);
            Debug.Log($"Upgraded tower {id}. Current upgrade count: {towerToUpgrade.upgradeCount}");
        }
        else
        {
            Debug.Log($"Tower {id} has already reached the maximum upgrade level.");
        }
    }
    else
    {
        Debug.Log("Not enough currency for upgrade.");
    }
}

    public void Init(Tilemap tilemap)
    {
        spawnTilemap = tilemap;
    }
}