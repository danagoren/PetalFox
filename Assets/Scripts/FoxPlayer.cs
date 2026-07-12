using UnityEngine;
using UnityEngine.InputSystem;

public class FoxPlayer : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Sprite jumpSprite;
    

    private bool isJumping;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    public void TryJump()
    {
        bool grounded = playerCollider.IsTouchingLayers(groundLayer);
        if (isJumping || !grounded || _rb.linearVelocity.y >= 0.1f) return;

        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
        _animator.enabled = false;
        if (jumpSprite != null)
            _sr.sprite = jumpSprite;
        isJumping = true;
    }

    private void Update()
    {
        bool grounded = playerCollider.IsTouchingLayers(groundLayer);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryJump();
        }

        if (isJumping && grounded && _rb.linearVelocity.y <= 0f)
        {
            _animator.enabled = true;
            isJumping = false;
        }

        if (!grounded && jumpSprite != null)
        {
            _sr.sprite = jumpSprite;
        }
    }
}