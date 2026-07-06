using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform edgePoint;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float offscreenBuffer = 0.5f;
    [SerializeField] private float startDelay = 0f;

    private Vector3 spawnPosition;
    private float _readyTime;
    private float objectHalfWidth;
    private bool edgeWarningShown;
    private bool firstIteration;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _readyTime = Time.time + startDelay;
        firstIteration = true;

        spawnPosition = spawnPoint != null ? spawnPoint.position : Vector3.zero;

        if (rb != null)
            rb.position = spawnPosition;
        else
            transform.position = spawnPosition;

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            objectHalfWidth = sr.bounds.extents.x;
    }

    void FixedUpdate()
    {
        if (Time.time < _readyTime && firstIteration) return;
        firstIteration = false;

        if (rb != null)
            rb.linearVelocity = Vector2.left * moveSpeed;
        else
            transform.position += Vector3.left * moveSpeed * Time.fixedDeltaTime;
    }

    void Update()
    {
        if (Time.time < _readyTime) return;

        float threshold;
        if (edgePoint != null)
        {
            threshold = edgePoint.position.x;
        }
        else
        {
            if (!edgeWarningShown)
            {
                Debug.LogWarning("edgePoint is not assigned, falling back to camera left edge");
                edgeWarningShown = true;
            }

            Camera cam = Camera.main;
            threshold = cam.transform.position.x - cam.orthographicSize * cam.aspect;
        }

        float currentX = rb != null ? rb.position.x : transform.position.x;

        if (currentX + objectHalfWidth + offscreenBuffer < threshold)
        {
            if (rb != null)
                rb.position = spawnPosition;
            else
                transform.position = spawnPosition;

            _readyTime = Time.time + startDelay;
            gameObject.SetActive(true);

            foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
                sr.enabled = true;

            foreach (var col in GetComponentsInChildren<Collider2D>())
                col.enabled = true;

            foreach (var berry in GetComponentsInChildren<Berry>())
                berry.ResetState();
        }
    }
}