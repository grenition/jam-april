using UnityEngine;

public class SplashEffectSystem : MonoBehaviour
{
    [SerializeField] private SplashEffect _prefab;

    private SplashEffect[,] _effects = new SplashEffect[3, 3];

    private int _effectIndex = 0;

    private void Start()
    {
        var parent = ServiceLocator.Get<InGameCanvasParent>();
        var trans = transform;
        if(parent != null)
        {
            trans = parent.transform;
        }
        for(int i = 0; i < _effects.GetLength(0); i++)
        {
            for(int j = 0; j < _effects.GetLength(1); j++)
            {
                _effects[i, j] = Instantiate(_prefab, trans);
            }
        }
    }

    public void PlayEffect()
    {
        var randomVec = new Vector3(Random.Range(-.6f, .6f), Random.Range(-.6f, .6f), 0);
        for(int i = 0; i < _effects.GetLength(1); i++)
        {
            var randomVec2 = new Vector3(Random.Range(-.4f, .4f), Random.Range(-.4f, .4f), 0);
            _effects[_effectIndex, i].transform.position = transform.position + randomVec + randomVec2;
            _effects[_effectIndex, i].transform.localScale = Vector3.one * Random.Range(.7f, 1.3f);
            _effects[_effectIndex, i].transform.localRotation = Quaternion.identity;
            _effects[_effectIndex, i].transform.Rotate(transform.forward, Random.Range(-30f, 30f));
            _effects[_effectIndex, i].Initialize();
        }
        _effectIndex = (_effectIndex + 1) % _effects.GetLength(0);
    }
}
