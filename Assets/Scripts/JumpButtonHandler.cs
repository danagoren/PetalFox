using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButtonHandler : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        var player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<FoxPlayer>();
        if (player != null)
            player.TryJump();
    }
}
