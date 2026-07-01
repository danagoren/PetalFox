using UnityEngine;

public class FoxPlayer : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }
}
