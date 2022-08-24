using System;
using System.Collections.Generic;
using UnityEngine;

public class WorkPiece : MonoBehaviour
{
    private const int MaxSize = 4;

    [SerializeField] private List<Vector3> _scales;
    [SerializeField] private ParticleSystem _explosion;

    private int _scaleNumber = 0;

    public event Action ImReady;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Hammer hammer))
        {
            if (_scaleNumber == _scales.Count)
                return;

            ChangeSize();
            _explosion.Play();

            if (_scaleNumber == MaxSize)
                ImReady?.Invoke();
        }
    }

    private void ChangeSize()
    {
        transform.localScale = _scales[_scaleNumber];
        _scaleNumber++;
    }
}