using UnityEngine;
using DG.Tweening;

public class Container : MonoBehaviour
{
  [SerializeField] private GameObject _lava;
  [SerializeField] private Transform _upperLavaPosition;
  [SerializeField] private bool _isFull;

  private float _fillDuration = 2f;
  private float _rotationDuration = 0.5f;
  private float _moveDuration = 0.2f;
  private Sequence _moveAnimation;
  private Vector3 _targetRotation = Vector3.zero;

  private void Start()
  {
    if (_isFull)
      Fill();
  }

  public void SetPosition(Vector3 targetPosition)
  {
    transform.DOLocalMove(targetPosition, _moveDuration);
    transform.DOLocalRotate(_targetRotation, _rotationDuration);
  }

  public void Fill()
  {
    _lava.gameObject.SetActive(true);

    _lava.transform
      .DOMoveY(_upperLavaPosition.position.y, _fillDuration)
      .SetEase(Ease.Linear)
      .SetAutoKill(true);

    _isFull = true;
  }
}