using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SmithyConveyor : MonoBehaviour
{
  private const float YOffset = 0.15f;

  [SerializeField] private List<Container> _containersOnStand;
  [SerializeField] private List<Transform> _pointsForContainers;
  [SerializeField] private List<GameObject> _swords;
  [SerializeField] private List<Transform> _swordsPoints;
  [SerializeField] private ParticleSystem _waterSteam;
  [SerializeField] private GameObject _arrow;
  [SerializeField] private Renderer _lent;
  [SerializeField] private Transform _containersPool;
  [SerializeField] private Transform _firstPosition;
  [SerializeField] private Transform _secondPosition;
  [SerializeField] private Transform _thirdPosition;
  [SerializeField] private Transform _fourthContainerPosition;
  [SerializeField] private Transform _swordPoint;
  [SerializeField] private Transform _finalSwordPoint;
  [SerializeField] private GameObject _workPiece;
  [SerializeField] private GameObject Sword;
  [SerializeField] private SwordStorage _swordStorage;

  private GameObject _currentSword;
  private Container _currentContainer;
  private GameObject _currentWorkPiece;
  private readonly Vector2 _lentSpeed = new Vector2(0, 5);
  private readonly Vector3 _position = Vector3.zero;
  private float _moveDuration = 1f;
  private float _standartJumpDuration = 0.2f;
  private float _hittingDuration = 1.6f;
  private int _standartJumpPower = 1;
  private int _swordJumpPower = 5;
  private int _containerIndex = 0;
  private int _swordIndex = 0;
  private Coroutine _work;
  private Coroutine _move;
  private Coroutine _jump;
  private Tween _lentMover;

  public Transform PointForContainer => _pointsForContainers[_containerIndex];

  public event Action ContainerOnSmithy;
  public event Action ContainerOnConveyor;

  public void TakeContainer(Container container)
  {
    DeactivateArrow();

    _containersOnStand.Add(container);

    container.SetPosition(_position);
    _containerIndex++;

    if (_work != null)
      return;

    _work = StartCoroutine(Work());
  }

  private void DeactivateArrow()
  {
    if (_arrow.gameObject.activeSelf == false)
      return;

    _arrow.SetActive(false);
  }


  private IEnumerator Work()
  {
    var jumpDelay = new WaitForSeconds(_standartJumpDuration);
    var delay = new WaitForSeconds(1f);
    var moveDelay = new WaitForSeconds(_moveDuration);
    var hittingDuration = new WaitForSeconds(_hittingDuration);

    yield return delay;

    while (_containersOnStand.Count > 0)
    {
      _currentContainer = _containersOnStand[_containersOnStand.Count - 1];

      yield return delay;

      _jump = StartCoroutine(Jump(_firstPosition, _currentContainer.gameObject, _standartJumpPower));
      yield return jumpDelay;
      
      ContainerOnConveyor?.Invoke();

      _containerIndex--;

      _move = StartCoroutine(Move(_secondPosition, _currentContainer.gameObject));
      yield return moveDelay;

      _waterSteam.Play();

      ChangeObject(_currentContainer);
      yield return delay;

      _move = StartCoroutine(Move(_thirdPosition, _currentWorkPiece));
      yield return moveDelay;

      _jump = StartCoroutine(Jump(_fourthContainerPosition, _currentWorkPiece, _standartJumpPower));
      yield return jumpDelay;

      ContainerOnSmithy?.Invoke();
      yield return hittingDuration;

      _currentWorkPiece.SetActive(false);
      CreateSword();

      GetSword();

      _jump = StartCoroutine(Jump(_swordsPoints[_swordIndex], _currentSword, _swordJumpPower));
      _swordIndex++;
      yield return jumpDelay;
    }
  }

  private void GetSword()
  {
    _swordStorage.TakeSwords(_currentSword);
  }

  private void CreateSword()
  {
    _currentSword = Instantiate(Sword, _swordPoint.position, _swordPoint.rotation);

    _swords.Add(_currentSword);
  }

  private void ChangeObject(Container container)
  {
    container.gameObject.SetActive(false);
    _currentWorkPiece = Instantiate(_workPiece, _secondPosition.position, Quaternion.identity);
    _containersOnStand.Remove(container);
  }

  private IEnumerator Jump(Transform position, GameObject gameObject, int jumpPower)
  {
    gameObject.transform
      .DOJump(position.position, jumpPower, 1, _standartJumpDuration)
      .SetEase(Ease.Linear);

    yield return null;
  }

  private IEnumerator Move(Transform containerPosition, GameObject gameObject)
  {
    gameObject.transform
      .DOMoveX(containerPosition.position.x, _moveDuration)
      .SetEase(Ease.Linear);

    _lentMover = _lent.sharedMaterial
      .DOOffset(_lent.sharedMaterial.mainTextureOffset + _lentSpeed, _moveDuration)
      .SetEase(Ease.Linear);

    yield return null;
  }
}