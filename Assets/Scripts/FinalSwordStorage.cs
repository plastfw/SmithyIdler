using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FinalSwordStorage : MonoBehaviour
{
  [SerializeField] private List<GameObject> _swords;
  [SerializeField] private List<Transform> _swordsPoints;
  [SerializeField] private GameObject _arrow;

  private int _swordIndex = 0;
  private Sequence _moveAnimation;
  private Vector3 _swordRotaion = new Vector3(0, 0, 180);
  private float _duration = 1f;

  public Transform PointForSword => _swordsPoints[_swordIndex];

  public void TakeSword(GameObject sword)
  {
    _swords.Add(sword);
    SetSwordPosition(sword);
    _swordIndex++;

    if (_arrow.activeSelf == true)
      _arrow.SetActive(false);
  }

  public void SetSwordPosition(GameObject sword)
  {
    sword.transform.DOLocalRotate(_swordRotaion, _duration);
    sword.transform.DOLocalMove(Vector3.zero, _duration);
  }
}