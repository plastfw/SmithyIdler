using UnityEngine;

[RequireComponent(typeof(Mover))]
public class PlayerInput : MonoBehaviour
{
  private const string Horizontal = "Horizontal";
  private const string Vertical = "Vertical";

  private Mover _mover;
  private float _horizontalValue;
  private float _verticalValue;

  private void Start()
  {
    _mover = GetComponent<Mover>();
  }

  private void FixedUpdate()
  {
    InputMovement();
  }

  private void InputMovement()
  {
    _horizontalValue = Input.GetAxis(Horizontal);
    _verticalValue = Input.GetAxis(Vertical);

    _mover.Movement(_horizontalValue, _verticalValue);
  }
}