using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int maxHealth = 3;
    private int currentHealth;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(h, 0f, v) * moveSpeed;
        rb.velocity = move;
    }

    public void TakeDamage()
    {
        currentHealth--;
        Debug.Log("Игрок получил урон. Осталось жизней: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Игрок умер.");
        Destroy(gameObject);
    }
}
