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

    void Update()
    {
        if (CanSpawn())
            DetectSpawnPoint();
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
        tower.GetComponent<Tower>().Init(cellPosition, spawnID); // Pass spawnID here

        DeselectTowers();
    }

    public void RevertCellState(Vector3Int pos)
    {
        spawnTilemap.SetColliderType(pos, Tile.ColliderType.Sprite);
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
        // Highlight the selected tower button for upgrade
        DeselectTowers();
        towersUI[id].color = Color.white;

        // Find the tower corresponding to the selected upgrade ID
        foreach (var tower in GameObject.FindObjectsOfType<Tower>())
        {
            if (tower.towerID == id)
            {
                int upgradeCost = TowerCost(id);

                // Check if the player has enough currency
                if (GameManager.instance.currency.EnoughCurrency(upgradeCost))
                {
                    // Attempt to upgrade the tower
                    if (tower.Upgrade())
                    {
                        // Deduct the currency if the upgrade is successful
                        GameManager.instance.currency.Use(upgradeCost);
                        Debug.Log($"Upgraded tower {id}. Current upgrade count: {tower.upgradeCount}");
                    }
                    else
                    {
                        // Notify the player that the tower has reached the maximum upgrade level
                        Debug.Log("Tower has already reached the maximum upgrade level.");
                    }
                    return;
                }
                else
                {
                    // Notify the player about insufficient currency
                    Debug.Log("Not enough currency for upgrade.");
                    return;
                }
            }
        }

        // Notify if no tower matches the provided ID
        Debug.Log("No tower found for the given ID.");
    }

}
