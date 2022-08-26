using UnityEngine;
using DG.Tweening;

public class ScalePuncher : MonoBehaviour
{
  [SerializeField] private Vector3 _punchScale;
  [SerializeField] private float _duration;

  private void Start()
  {
    PunchAnimation();
  }

  private void PunchAnimation()
  {
    transform.DOPunchScale(_punchScale, _duration);
  }
}