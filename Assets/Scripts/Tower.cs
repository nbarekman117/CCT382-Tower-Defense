using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int health;
    public int cost;
    public int towerID; // Add this field to identify towers
    public int upgradeCount = 0; // Track the number of upgrades
    public const int maxUpgrades = 2; // Maximum number of upgrades allowed

    // Add a reference to the sprite renderer and the sprites for upgrades
    public SpriteRenderer spriteRenderer;
    public List<Sprite> sprites; // Assign these in the Unity Inspector

    private Vector3Int cellPosition;


    protected virtual void Start()
    {
        Debug.Log("BASE TOWER");
    }

    public virtual void Init(Vector3Int cellPos, int id)
    {
        cellPosition = cellPos;
        towerID = id; // Assign the tower ID here
    }
    //Lose Health
    public virtual bool LoseHealth(int amount)
    {
        //health = health - amount
        health-= amount;

        if (health <= 0)
        {
            Die();
            return true;
        }
        return false;
    }
    //Die
    protected virtual void Die()
    {
        Debug.Log("Tower is dead");
        FindObjectOfType<Spawner>().RevertCellState(cellPosition);
        Destroy(gameObject);
    }

    // Make this method virtual
    public virtual bool Upgrade()
    {
        if (upgradeCount >= maxUpgrades)
        {
            Debug.Log("Maximum upgrades reached!");
            return false;
        }

        upgradeCount++;

        // Change sprite based on the upgrade level
        if (upgradeCount < sprites.Count)
        {
            spriteRenderer.sprite = sprites[upgradeCount];
        }

        Debug.Log("Base Tower upgraded! Current upgrade count: " + upgradeCount);
        return true;
    }
}