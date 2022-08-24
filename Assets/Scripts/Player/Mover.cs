using UnityEngine;

public class Mover : MonoBehaviour
{
    private const int ZeroSpeed = 0;
    private const int Speed = 1;
    
    private float MinValue = 0.1f;
    
    [SerializeField] private float _speed = 3;
    [SerializeField] private float _rotationSpeed = 10;

    private Rigidbody _rigidbody;
    private PlayerAnimation _animation;

    private void Start()
    {
        _animation = GetComponent<PlayerAnimation>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Movement(float horizontal,float vertical)
    {
        Vector3 directionVector = new Vector3(horizontal, 0, vertical).normalized;

        _animation.Move(ZeroSpeed);
        
        if (directionVector.magnitude > Mathf.Abs(MinValue))
        {
            _animation.Move(Speed);
            
            _rigidbody.MovePosition(transform.position + directionVector * (Time.fixedDeltaTime * _speed));
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionVector), Time.deltaTime * _rotationSpeed);
        }
    }
}