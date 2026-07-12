using UnityEngine;
using UnityEngine.UI;

public class UICounter : MonoBehaviour
{
    public static UICounter Instance { get; private set; }

    [SerializeField] private Text countText;
    private int _count;

    void Awake() => Instance = this;

    void Start()
    {
        if (countText != null)
            countText.text = "0";
    }

    public void AddCount(int amount = 1)
    {
        _count += amount;
        if (countText != null)
            countText.text = _count.ToString();
    }
}
