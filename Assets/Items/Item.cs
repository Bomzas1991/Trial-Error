using UnityEngine;

public class Item : MonoBehaviour
{
    public float fallingSpeed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer); // let the items fall through each other
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(rb.velocity.x, -fallingSpeed);
    }
}
