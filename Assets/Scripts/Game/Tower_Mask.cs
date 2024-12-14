using UnityEngine;

public class Tower_Mask : Tower
{
    protected override void Start()
    {
        Debug.Log("MASK");
    }

    public override void Init(Vector3Int cellPos, int id)
    {
        base.Init(cellPos, id); // Call the base implementation to set cellPos and towerID
    }

    public override bool Upgrade()
    {
        if (!base.Upgrade()) return false; // Check if further upgrades are allowed

        health += 10; // Increase health
        Debug.Log("Mask Tower upgraded! New health: " + health);
        return true; // Indicate successful upgrade
    }

}
