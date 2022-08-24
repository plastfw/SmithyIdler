using DG.Tweening;
using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
    [SerializeField] private float _offset = 1f;
    
    private float _duration = .5f;

    void Start()
    {
        MoveArrow();
    }

    private void MoveArrow()
    {
        transform.DOMoveY(transform.position.y + _offset, _duration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }
}