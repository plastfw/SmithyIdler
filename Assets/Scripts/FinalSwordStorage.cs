using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FinalSwordStorage : MonoBehaviour
{
  [SerializeField] private List<GameObject> _swords;
  [SerializeField] private List<Transform> _swordsPoints;
  [SerializeField] private GameObject _arrow;

  private readonly Vector3 _swordRotation = new Vector3(0, 0, 180);
  private int _swordIndex = 0;
  private float _duration = 1f;
  private Sequence _moveAnimation;

  public Transform PointForSword => _swordsPoints[_swordIndex];

  public void TakeSword(GameObject sword)
  {
    _swords.Add(sword);
    SetSwordPosition(sword);
    _swordIndex++;

    if (_arrow.activeSelf)
      _arrow.SetActive(false);
  }

  private void SetSwordPosition(GameObject sword)
  {
    sword.transform.DOLocalRotate(_swordRotation, _duration);
    sword.transform.DOLocalMove(Vector3.zero, _duration);
  }
}