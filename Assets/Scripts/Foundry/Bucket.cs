using UnityEngine;

public class Bucket : MonoBehaviour
{
    [SerializeField] private ParticleSystem _lavaFall;

    private Container _currentContainer;
    
    public void LavaFallChangeState()
    {
        if (_lavaFall.gameObject.activeSelf == false)
        {
            _lavaFall.gameObject.SetActive(true);
        }
        else
            _lavaFall.gameObject.SetActive(false);
    }
}