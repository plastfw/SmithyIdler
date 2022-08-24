using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ScrollingLava : MonoBehaviour
{
    private const string TextureName = "_MainTex";

    private Vector2 _speed = new Vector2(1.0f, 0.0f);
    private Vector2 _offset = Vector2.zero;
    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    void LateUpdate()
    {
        _offset += (_speed * Time.deltaTime);

        _renderer.sharedMaterial.SetTextureOffset(TextureName, _offset);
    }
}