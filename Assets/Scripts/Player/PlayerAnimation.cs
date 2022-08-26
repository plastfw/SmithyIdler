using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerAnimation : MonoBehaviour
{
  private const string Speed = "Speed";
  private const string IsCarry = "IsCarry";

  [SerializeField] private Animator _animator;

  private Player _player;

  private void OnEnable()
  {
    _player = GetComponent<Player>();
    _player.IsCarry += CarryState;
  }

  private void OnDisable()
  {
    _player.IsCarry -= CarryState;
  }

  public void Move(int value)
  {
    _animator.SetInteger(Speed, value);
  }

  private void CarryState(bool state)
  {
    _animator.SetBool(IsCarry, state);
  }
}