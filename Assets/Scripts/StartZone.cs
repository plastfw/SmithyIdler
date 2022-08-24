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

  private Vector3 _doorRotation = new Vector3(0, 105, 0);
  private float _doorsOpeningDuration = 1f;
  private TutorialCamera _tutorialCamera;
  private float _topViewCameraDelay = 2.5f;

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
    foreach (MeshRenderer mesh in _startCarColor)
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
    var ViewCameraDelay = new WaitForSeconds(_topViewCameraDelay);

    _tutorialCamera.ActiveTopView();

    yield return ViewCameraDelay;
    _smokeExplosion.Play();
    OpenCarDoors();
    ChangeCarColor();
    DeactivateStartZone();
    _arrow.SetActive(true);

    _tutorialCamera.ActiveCarView();

    yield return ViewCameraDelay;

    _tutorialCamera.ActiveDefaultView();
  }
}