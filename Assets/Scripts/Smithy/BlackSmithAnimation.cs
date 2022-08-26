using UnityEngine;
using UnityEngine.Playables;

public class BlackSmithAnimation : MonoBehaviour
{
  [SerializeField] private SmithyConveyor _smithyConveyor;
  [SerializeField] private ParticleSystem _waitingEffect;
  [SerializeField] private PlayableDirector _director;

  private void OnEnable()
  {
    _waitingEffect.Play();
    _smithyConveyor.ContainerOnConveyor += DeactivateEmotion;
    _smithyConveyor.ContainerOnSmithy += HitAnimation;
  }

  private void OnDisable()
  {
    _smithyConveyor.ContainerOnConveyor -= DeactivateEmotion;
    _smithyConveyor.ContainerOnSmithy -= HitAnimation;
  }

  private void HitAnimation()
  {
    _director.Play();
  }

  private void DeactivateEmotion()
  {
    _waitingEffect.Stop();
  }
}