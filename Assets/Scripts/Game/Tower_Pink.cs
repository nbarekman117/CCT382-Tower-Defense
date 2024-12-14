using System.Collections;
using UnityEngine;

public class Tower_Pink : Tower
{
    //FIELDS
    //Income value
    public int incomeValue;
    //Interval for income
    public float interval;
    //Coin image object
    public GameObject obj_coin;


    //METHODS
    //Init
    protected override void Start()
    {
        Debug.Log("PINK");
        StartCoroutine(Interval());
    }
    //Interval IEnumerator
    IEnumerator Interval()
    {
        yield return new WaitForSeconds(interval);
        //Trigger the income increase
        IncreaseIncome();

        StartCoroutine(Interval());
    }
    //Trigger Income Increase
    public void IncreaseIncome()
    {
        GameManager.instance.currency.Gain(incomeValue);
        StartCoroutine(CoinIndication());
    }  
    //Show coin indication over the tower for short time (0.5 second)
    IEnumerator CoinIndication()
    {
        obj_coin.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        obj_coin.SetActive(false);
    }

    public override void Init(Vector3Int cellPos, int id)
    {
        base.Init(cellPos, id); // Call the base implementation to set cellPos and towerID
    }

    public override bool Upgrade()
    {
        if (!base.Upgrade()) return false; // Check if further upgrades are allowed

        incomeValue += 5; // Increase income
        Debug.Log("Pink Tower upgraded! New income: " + incomeValue);
        return true; // Indicate successful upgrade
    }

}
