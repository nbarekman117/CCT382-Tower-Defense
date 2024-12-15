using UnityEngine;

public class ShootItem : MonoBehaviour
{
    //FIELDS
    public Transform graphics; // The sprite renderer
    public int damage; // Damage dealt
    public float flySpeed, rotateSpeed; // Movement speeds

    [SerializeField]
    private AudioClip hitSound; // Sound effect for hit
    private AudioSource audioSource; // Audio source to play sound

    //METHODS
    public void Init(int dmg)
    {
        damage = dmg;

        // Initialize audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = hitSound;
        audioSource.playOnAwake = false; // Ensure sound doesn't play on instantiation
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Shot the enemy");

            // Play the hit sound effect
            if (audioSource && hitSound)
            {
                audioSource.Play();
            }

            collision.GetComponent<Enemy>().LoseHealth();
            Destroy(gameObject, 0.1f); // Slight delay to ensure sound plays before destruction
        }
        else if (collision.tag == "Out")
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        Rotate();
        FlyForward();
    }

    void Rotate()
    {
        graphics.Rotate(new Vector3(0, 0, -rotateSpeed * Time.deltaTime));
    }

    void FlyForward()
    {
        transform.Translate(transform.right * flySpeed * Time.deltaTime);
    }
}
