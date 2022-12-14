using Cinemachine;
using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
  [SerializeField] private CinemachineVirtualCamera _topPosition;
  [SerializeField] private CinemachineVirtualCamera _carPosition;
  [SerializeField] private CinemachineVirtualCamera _playerPosition;

  public void ActiveTopView()
  {
    _topPosition.m_Priority++;
  }

  public void ActiveCarView()
  {
    _carPosition.Priority++;
  }

  public void ActiveDefaultView()
  {
    _playerPosition.Priority++;
  }
}