using DG.Tweening;
using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
  private readonly float _duration = .5f;
  private readonly float _offset = 1f;

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