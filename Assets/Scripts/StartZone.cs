using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(TutorialCamera))]
public class StartZone : MonoBehaviour
{
  [SerializeField] private Material _targetCarColor;
  [SerializeField] private Player _player;
  [SerializeField] private List<MeshRenderer> _startCarColor;
  [SerializeField] private ParticleSystem _smokeExplosion;
  [SerializeField] private GameObject _leftDoor;
  [SerializeField] private GameObject _rightDoor;
  [SerializeField] private List<GameObject> _startZones;
  [SerializeField] private GameObject _arrow;

  private readonly Vector3 _doorRotation = new Vector3(0, 105, 0);
  private TutorialCamera _tutorialCamera;
  private float _doorsOpeningDuration = 1f;
  private float _topViewCameraDelay = 2f;

  private void OnEnable()
  {
    _tutorialCamera = GetComponent<TutorialCamera>();
    _player.HaveSword += ActiveTutorial;
  }

  private void OnDisable()
  {
    _player.HaveSword -= ActiveTutorial;
  }

  private void ActiveTutorial()
  {
    StartCoroutine(DeactivateAnimation());
  }

  private void ChangeCarColor()
  {
    foreach (var mesh in _startCarColor)
    {
      mesh.material = _targetCarColor;
    }
  }

  private void OpenCarDoors()
  {
    _leftDoor.transform.DORotate(_doorRotation, _doorsOpeningDuration);
    _rightDoor.transform.DORotate(-_doorRotation, _doorsOpeningDuration);
  }

  private void DeactivateStartZone()
  {
    foreach (GameObject zone in _startZones)
    {
      zone.SetActive(false);
    }
  }

  private IEnumerator DeactivateAnimation()
  {
    var viewCameraDelay = new WaitForSeconds(_topViewCameraDelay);

    _arrow.SetActive(true);

    _tutorialCamera.ActiveTopView();

    yield return viewCameraDelay;
    _smokeExplosion.Play();
    OpenCarDoors();
    ChangeCarColor();
    DeactivateStartZone();

    _tutorialCamera.ActiveCarView();

    yield return viewCameraDelay;

    _tutorialCamera.ActiveDefaultView();
  }
}