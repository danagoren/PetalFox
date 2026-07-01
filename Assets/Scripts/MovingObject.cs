using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform edgePoint;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float offscreenBuffer = 0.5f;

    private Vector3 spawnPosition;
    private float objectHalfWidth;
    private bool edgeWarningShown;

    void Start()
    {
        spawnPosition = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        transform.position = spawnPosition;

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            objectHalfWidth = sr.bounds.extents.x;
    }

    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

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

        if (transform.position.x + objectHalfWidth + offscreenBuffer < threshold)
        {
            transform.position = spawnPosition;
            gameObject.SetActive(true);
        }
    }
}
