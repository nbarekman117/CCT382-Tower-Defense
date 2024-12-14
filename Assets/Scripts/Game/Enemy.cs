using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    // Health, AttackPower, MoveSpeed
    public int health, attackPower;
    public float moveSpeed;

    public Animator animator;
    public float attackInterval;
    Coroutine attackOrder;
    Tower detectedTower;

    private GameManager gameManager;  // Reference to GameManager

    void Start()
    {
        // Find the GameManager object in the scene
        gameManager = GameManager.instance;

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    void Update()
    {
        if (!detectedTower)
        {
            Move();
        }
    }

    IEnumerator Attack()
    {
        animator.Play("Attack", 0, 0);
        // Wait for attack interval
        yield return new WaitForSeconds(attackInterval);
        // Attack again
        attackOrder = StartCoroutine(Attack());
    }

    // Moving forward
    void Move()
    {
        animator.Play("Move");
        transform.Translate(-transform.right * moveSpeed * Time.deltaTime);
    }

    // Inflict damage to detected towers
    public void InflictDamage()
    {
        bool towerDied = detectedTower.LoseHealth(attackPower);

        if (towerDied)
        {
            detectedTower = null;
            StopCoroutine(attackOrder);
        }
    }

    // Lose health when hit by something (like the "Out" area)
    public void LoseHealth()
    {
        // Decrease health value
        health--;
        Debug.Log("Enemy Health: " + health); // Debug to check health value
        // Blink Red animation
        StartCoroutine(BlinkRed());

        // Check if health is zero => destroy enemy
        if (health <= 0)
        {
            Debug.Log("Enemy Destroyed!"); // Debug message when the enemy is destroyed
            Destroy(gameObject);
        }
    }

    IEnumerator BlinkRed()
    {
        // Change the sprite renderer color to red
        GetComponent<SpriteRenderer>().color = Color.red;
        // Wait for a really small amount of time
        yield return new WaitForSeconds(0.2f);
        // Revert to default color
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If already detected a tower, do nothing
        if (detectedTower)
            return;

        // Detect when the enemy collides with an "Out" trigger zone
        if (collision.CompareTag("Out"))
        {
            Debug.Log("Enemy reached the Out zone!"); // Debug message

            // Decrease the player's health when the enemy reaches the "Out" zone
            if (gameManager != null && gameManager.health != null)
            {
                gameManager.health.DecreaseHealth(1); // Example: decrease by 1 when an enemy reaches the "Out" zone
            }

            // Optionally, destroy the enemy here if you want it to disappear after reaching the end
            Destroy(gameObject);
        }

        // Detect when the enemy collides with a tower (to start attacking)
        if (collision.CompareTag("Tower"))
        {
            detectedTower = collision.GetComponent<Tower>();
            attackOrder = StartCoroutine(Attack());
        }
    }
}
