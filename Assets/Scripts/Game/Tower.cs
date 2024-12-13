using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int health;
    public int cost;
    public int towerID; // Add this field to identify towers

    
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
    public virtual void Upgrade()
    {
        Debug.Log("Base Tower upgraded!");
    }
}
