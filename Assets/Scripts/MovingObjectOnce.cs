using UnityEngine;

public class MovingObjectOnce : MonoBehaviour
{
    [SerializeField] private Transform edgePoint;
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (rb != null)
            rb.linearVelocity = Vector2.left * moveSpeed;
        else
            transform.position += Vector3.left * moveSpeed * Time.fixedDeltaTime;
    }

    void Update()
    {
        if (edgePoint == null) return;

        float currentX = rb != null ? rb.position.x : transform.position.x;
        if (currentX <= edgePoint.position.x)
            Destroy(gameObject);
    }
}
