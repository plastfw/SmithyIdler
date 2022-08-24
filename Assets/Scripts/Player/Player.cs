using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
  private const float ContainerYOffset = 0.3f;
  private const float SwordYOffset = 0.1f;

  [SerializeField] private Transform _handPoint;
  [SerializeField] private Transform _swordHandPoint;
  [SerializeField] private Transform _cashpoint;
  [SerializeField] private List<Container> _containersInHands;
  [SerializeField] private List<GameObject> _swords;

  private Vector3 _ContainerOffset = new Vector3(0, 0, 0);
  private Vector3 _swordOffset = new Vector3(0, 0, 0);
  private Vector3 _swordRotation = new Vector3(90, 90, 0);
  private Coroutine _give;
  private float _swordJump = 0.3f;
  private bool _firstSelectionSword = true;

  public Transform CashPoint => _cashpoint;
  public Transform SwordHandPoint => _swordHandPoint;
  public Transform HandPoint => _handPoint;

  public event Action<bool> IsCarry;
  public event Action HaveSword;

  private void OnTriggerEnter(Collider collider)
  {
    if (collider.TryGetComponent(out SmithyConveyor smithyConveyor))
    {
      if (_containersInHands.Count == 0)
        return;

      if (_give != null)
        StopCoroutine(_give);

      _give = StartCoroutine(GiveContainers(smithyConveyor));
    }

    if (collider.TryGetComponent(out FinalSwordStorage finalSwordStorage))
    {
      GiveSwords(finalSwordStorage);
    }
  }

  public void TakeContainer(Container container)
  {
    IsCarry?.Invoke(true);

    _containersInHands.Add(container);

    container.SetPosition(_ContainerOffset);
    _ContainerOffset.y += ContainerYOffset;
  }

  public void TakeSword(GameObject sword)
  {
    _swords.Add(sword);
    sword.transform.DOLocalJump(_swordOffset, 1, 1, _swordJump);
    sword.transform.DOLocalRotate(_swordRotation, _swordJump);
    _swordOffset.y += SwordYOffset;
    IsCarry?.Invoke(true);

    if (_firstSelectionSword)
    {
      HaveSword?.Invoke();
      _firstSelectionSword = false;
    }
  }
  
  private void GiveSwords(FinalSwordStorage storage)
  {
    for (int i = 0; i < _swords.Count; i++)
    {
      _swords[i].transform.SetParent(storage.PointForSword);
      storage.TakeSword(_swords[i]);
    }

    _swords.Clear();
    IsCarry?.Invoke(false);
    _swordOffset.y = 0;
  }

  private IEnumerator GiveContainers(SmithyConveyor smithyConveyor)
  {
    var delay = new WaitForSeconds(0.02f);

    for (int i = _containersInHands.Count - 1; i >= 0; i--)
    {
      _containersInHands[i].transform.SetParent(smithyConveyor.PointForContainer);
      smithyConveyor.TakeContainer(_containersInHands[i]);
      yield return delay;
    }

    _containersInHands.Clear();
    IsCarry?.Invoke(false);
    _ContainerOffset.y = 0;
  }
}