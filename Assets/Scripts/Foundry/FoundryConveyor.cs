using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FoundryConveyor : MonoBehaviour
{
  [SerializeField] private GameObject _bucketCreator;
  [SerializeField] private Container _container;
  [SerializeField] private Bucket _bucket;
  [SerializeField] private Renderer _lent;
  [Space(10)] [SerializeField] private Transform _startContainerPoint;
  [SerializeField] private Transform _firstContainerPoint;
  [SerializeField] private Transform _secondContainerPoint;
  [Space(10)] [SerializeField] private List<Container> _containers;
  [SerializeField] private List<Transform> _containersPoints;
  [SerializeField] private GameObject _lavaMesh;
  [SerializeField] private Material _lavaMeshMaterial;

  private float _materialMoveDuration = 2f;
  private int _playerСapacity = 5;
  private Vector2 _lavaOffset = new Vector2(0, 5);
  private Vector2 _lentSpeed = new Vector2(0, 5);
  private Quaternion _containerRotation = Quaternion.Euler(0, 90, 0);
  private Vector3 _bucketRotationOffset = new Vector3(0, 0, 50);
  private Vector3 _standartBucketRotation = Vector3.zero;
  private Container _currentContainer;
  private int _startContainersCount = 8;
  private float _bucketRotationDuration = 1f;
  private float _bucketCreatorOffset = 4f;
  private float _containerCreatorDuration = 0.2f;
  private float _moveDuration = 1f;
  private float _jumpDuration = 0.3f;
  private Tween _lentMover;
  private Tween _creator;
  private Tween _rotate;
  private Coroutine _work;
  private Coroutine _fill;
  private Coroutine _move;
  private Coroutine _give;

  private void Start()
  {
    CreateStartContainers();
    _work = StartCoroutine(Work());
  }

  private void OnTriggerEnter(Collider collider)
  {
    if (collider.TryGetComponent(out Player player))
    {
      if (_give != null)
        StopCoroutine(_give);

      _give = StartCoroutine(GiveContainer(player));
    }
  }

  private void CreateStartContainers()
  {
    for (int i = 0; i != _startContainersCount; i++)
    {
      _currentContainer = Instantiate(_container, _containersPoints[i].transform.position, _containerRotation,
        transform);
      AddToList();
      _currentContainer.Fill();
    }
  }

  private void Jump()
  {
    int index = _containers.Count;

    _currentContainer.transform
      .DOJump(_containersPoints[index].transform.position, 1, 1, _jumpDuration);
  }

  private void CreateContainer()
  {
    _creator = _bucketCreator.transform
      .DOMoveY(_bucketCreator.transform.position.y - _bucketCreatorOffset, _containerCreatorDuration)
      .SetLoops(2, LoopType.Yoyo)
      .SetEase(Ease.Linear);

    _currentContainer = Instantiate(_container, _startContainerPoint.position, _containerRotation, transform);
  }

  private void AddToList()
  {
    _containers.Add(_currentContainer);
  }

  private void ActivateLava(bool state)
  {
    _lavaMesh.SetActive(state);
    _lavaMeshMaterial.DOOffset(_lavaMeshMaterial.mainTextureOffset + _lavaOffset, _materialMoveDuration);
  }

  private IEnumerator GiveContainer(Player player)
  {
    var delay = new WaitForSeconds(0.02f);

    for (int i = 0; i < _playerСapacity; i++)
    {
      _containers[i].transform.SetParent(player.HandPoint);
      player.TakeContainer(_containers[i]);
      yield return delay;
    }

    _containers.Clear();
  }

  private IEnumerator Work()
  {
    while (_containers.Count != _containersPoints.Count)
    {
      var bigDelay = new WaitForSeconds(5f);
      var delay = new WaitForSeconds(1f);

      CreateContainer();
      yield return _containerCreatorDuration;

      _move = StartCoroutine(Move(_firstContainerPoint));

      _fill = StartCoroutine(FillContainer());
      yield return bigDelay;

      _move = StartCoroutine(Move(_secondContainerPoint));
      yield return delay;

      Jump();
      AddToList();
    }
  }

  private IEnumerator FillContainer()
  {
    var delay = new WaitForSeconds(1f);
    var bigDelay = new WaitForSeconds(2f);

    _rotate = _bucket.transform.DORotate(_bucketRotationOffset, _bucketRotationDuration);
    yield return _rotate.WaitForCompletion();

    ActivateLava(true);

    _bucket.LavaFallChangeState();
    yield return delay;
    _currentContainer.Fill();

    yield return bigDelay;

    ActivateLava(false);
    _rotate = _bucket.transform.DORotate(_standartBucketRotation, _bucketRotationDuration);
    yield return _rotate.WaitForCompletion();
    _bucket.LavaFallChangeState();
  }

  private IEnumerator Move(Transform containerPosition)
  {
    _currentContainer.transform
      .DOMoveZ(containerPosition.position.z, _moveDuration)
      .SetEase(Ease.Linear);

    _lentMover = _lent.sharedMaterial
      .DOOffset(_lent.sharedMaterial.mainTextureOffset + _lentSpeed, _moveDuration)
      .SetEase(Ease.Linear);

    yield return null;
  }
}