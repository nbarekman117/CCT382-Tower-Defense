using System.Collections;
using UnityEngine;

public class Tower_Pink : Tower
{
    //FIELDS
    public int incomeValue; // Income value
    public float interval; // Interval for income generation
    public GameObject obj_coin; // Coin image object

    [SerializeField]
    private AudioClip coinSound; // Sound effect for receiving a coin
    private AudioSource audioSource; // AudioSource to play the sound

    //METHODS
    protected override void Start()
    {
        Debug.Log("PINK");
        audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource component
        audioSource.clip = coinSound; // Assign coin sound
        audioSource.playOnAwake = false; // Ensure sound doesn't play on initialization
        StartCoroutine(Interval());
    }

    IEnumerator Interval()
    {
        yield return new WaitForSeconds(interval);
        IncreaseIncome(); // Trigger the income increase
        StartCoroutine(Interval());
    }

    public void IncreaseIncome()
    {
        GameManager.instance.currency.Gain(incomeValue);

        // Play the coin sound effect
        if (audioSource && coinSound)
        {
            audioSource.Play();
        }

        StartCoroutine(CoinIndication());
    }

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

        incomeValue += 1; // Increase income
        Debug.Log("Pink Tower upgraded! New income: " + incomeValue);
        return true; // Indicate successful upgrade
    }
}
