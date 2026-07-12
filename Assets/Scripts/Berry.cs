using UnityEngine;

public class Berry : MonoBehaviour
{
    [SerializeField] private Sprite[] collectSprites;
    [SerializeField] private float animDuration = 0.3f;

    private SpriteRenderer _sr;
    private Collider2D _col;
    private bool _collected;
    private Vector3 _startScale;
    private Sprite _startSprite;

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _col = GetComponent<Collider2D>();
        _startScale = transform.localScale;
        _startSprite = _sr.sprite;
    }

    public void ResetState()
    {
        _collected = false;
        _sr.enabled = true;
        _sr.color = Color.white;
        _sr.sprite = _startSprite;
        _col.enabled = true;
        transform.localScale = _startScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_collected || !other.CompareTag("Player")) return;
        _collected = true;
        _col.enabled = false;
        if (UICounter.Instance != null) UICounter.Instance.AddCount();
        StartCoroutine(CollectAnimation());
    }

    private System.Collections.IEnumerator CollectAnimation()
    {
        if (collectSprites != null && collectSprites.Length > 0)
        {
            float frameDuration = animDuration / collectSprites.Length;
            foreach (var sprite in collectSprites)
            {
                _sr.sprite = sprite;
                yield return new WaitForSeconds(frameDuration);
            }
        }
        _sr.enabled = false;
    }
}
