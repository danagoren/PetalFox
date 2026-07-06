using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Collider2D mainCollider;
    [SerializeField] private Collider2D triggerCollider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        FoxPlayer player = other.GetComponent<FoxPlayer>();

        if (player == null)
            return;

        Vector2 direction = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;

        float dot = Vector2.Dot(direction, Vector2.up);
 
        if (dot < 0f)
            mainCollider.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        FoxPlayer player = other.GetComponent<FoxPlayer>();

        if (player == null)
            return;

        mainCollider.enabled = true;
    }
}