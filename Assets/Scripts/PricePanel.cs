using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class PricePanel : MonoBehaviour
{
  [SerializeField] private GameObject _object;
  [SerializeField] private GameObject _cash;
  [SerializeField] private GameObject _cashBox;
  [SerializeField] private Transform _objectPoint;
  [SerializeField] private Transform _topPoint;
  [SerializeField] private TMP_Text _priceText;
  [SerializeField] private Image _slider;
  [SerializeField] private float _price;

  private readonly Vector3 _cashRotation = new Vector3(-90, 0, 90);
  private List<GameObject> _money = new List<GameObject>();
  private GameObject _currentCash;
  private Coroutine _cashAnimation;
  private BoxCollider _collider;
  private Sequence _cashSequence;
  private float _duration = 2f;
  private int _jumpPower = 10;
  float _jumpDuration = 0.5f;

  private void Start()
  {
    _collider = GetComponent<BoxCollider>();

    SetStartPrice();
  }

  private void OnTriggerEnter(Collider collider)
  {
    if (!collider.TryGetComponent(out Player player)) return;

    FillingSlider();
    _cashAnimation = StartCoroutine(CashCreator(player.transform));
  }

  private void SetStartPrice()
  {
    _priceText.text = "$" + _price;
  }

  private void SpawnObject()
  {
    if (_object == null)
      return;

    Instantiate(_object, _objectPoint.position, Quaternion.identity);

    DeactivateThis();
  }

  private void FillingSlider()
  {
    _slider.DOFillAmount(1, _duration).SetAutoKill(true).SetEase(Ease.Linear);
    StartCoroutine(PriceChange());
  }

  private void DeactivateThis()
  {
    gameObject.SetActive(false);
    _collider.enabled = false;
  }

  private void UpScale()
  {
    _cashBox.transform.DOScale(new Vector3(1.25f, 0, 1.7f), 0.5f);
  }

  private IEnumerator CashCreator(Transform player)
  {
    var delay = new WaitForSeconds(0.1f);

    while (true)
    {
      _currentCash = Instantiate(_cash, player.position, Quaternion.Euler(_cashRotation));
      _money.Add(_currentCash);

      CashAnimation(_currentCash);

      yield return delay;
    }
  }

  private void CashAnimation(GameObject cash)
  {
    cash.transform.DOJump(transform.position, _jumpPower, 1, _jumpDuration);
  }

  private IEnumerator PriceChange()
  {
    var elepsedTime = 0f;
    var initialValue = _price;

    UpScale();

    while (elepsedTime < _duration)
    {
      _price = Mathf.Lerp(initialValue, 0, elepsedTime / _duration);
      _priceText.text = $"{(int) _price}";
      elepsedTime += Time.deltaTime;

      yield return null;
    }

    SpawnObject();
    StopCoroutine(_cashAnimation);

    foreach (var cash in _money)
    {
      cash.SetActive(false);
    }
  }
}