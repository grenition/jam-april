using UnityEngine;
using UnityEngine.UI;

public class SplashEffect : MonoBehaviour
{
    [SerializeField] private Image _image;

    public const float TIME = 1.2f;

    private Material _mat;
    private float _time = 0;
    private int _type = 0;

    private void Awake()
    {
        var mat = Instantiate(_image.material);
        _image.material = mat;
        _mat = mat;
        _time = TIME;
        _mat.color = new Color(1, 1, 1, 0);
    }

    public void Initialize()
    {
        _time = 0;
        _type = Random.Range(0, 2);
        _mat.color = Color.white;
    }

    private void Update()
    {
        if(_time < TIME)
        {
            _time += Time.deltaTime;
            int index = Mathf.Clamp(Mathf.FloorToInt(_time * 33 / TIME), 0, 8);
            _mat.mainTextureOffset = new Vector2((float)index / 9, (float)_type / 2);

            if (_time > .6f * TIME)
            {
                var clr = Color.white;
                clr.a = Mathf.Clamp((_time - .6f * TIME) / .3f * TIME, 0, 1);
                clr.a = 1 - clr.a;
                _mat.color = clr;
            }
        }
    }
}
