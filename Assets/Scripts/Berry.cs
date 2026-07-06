using UnityEngine;

public class Berry : MonoBehaviour
{
    private SpriteRenderer _sr;
    private Collider2D _col;

    [SerializeField] private Sprite[] collectSprites;
    [SerializeField] private float animDuration = 2f;

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _col.enabled = false;
        StartCoroutine(CollectAnimation());
    }

    private System.Collections.IEnumerator CollectAnimation()
    {
        float frameTime = animDuration / Mathf.Max(collectSprites.Length, 1);
        foreach (var sprite in collectSprites)
        {
            _sr.sprite = sprite;
            yield return new WaitForSeconds(frameTime);
        }
        Destroy(gameObject);
    }
}
