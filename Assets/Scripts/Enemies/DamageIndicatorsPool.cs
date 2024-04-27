using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicatorsPool : MonoBehaviour
{
    [SerializeField] private DamageIndicator _indicatorPrefab;
    [SerializeField] private TMP_Text _mainText;
    [SerializeField] private Slider _healthBar, _hurtBar;

    public const float DAMAGE_COUNT_TIME = 3.5f;
    public const float FADE_OUT_TIME = .5f;
    public const float FULL_RED_TEXT_DAMAGE = 160;

    private readonly List<DamageIndicator> _pool = new();
    private int _poolIndex = 0;
    private float _damageCountTime = 0;

    private Transform _parent;
    private Vector3 _offset;

    public float TotalDamage { get; private set; }

    private void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            var pool = Instantiate(_indicatorPrefab, transform);
            pool.gameObject.SetActive(false);
            _pool.Add(pool);
        }
    }

    public void Initialize(float health)
    {
        _healthBar.maxValue = health;
        _healthBar.value = health;
        _hurtBar.maxValue = health;
        _hurtBar.value = health;
        _healthBar.transform.localScale = new Vector3(1, 0, 1);
    }

    public void SetParentAndOffset(Transform parent, Vector3 offset)
    {
        _parent = parent;
        _offset = offset;
        transform.SetParent(null);
    }

    public void SpawnIndicator(float damage, DamageType type, float totalHealth)
    {
        if(type == DamageType.HEAL)
        {
            SpawnIndicator(damage, Color.green, totalHealth);
        }
        else if(type == DamageType.QUICK_ATTACK)
        {
            SpawnIndicator(damage, Color.yellow, totalHealth);
        }
        else
        {
            SpawnIndicator(damage, Color.red, totalHealth);
        }
    }

    public void SpawnIndicator(float damage, float totalHealth)
    {
        SpawnIndicator(damage, Color.yellow, totalHealth);
    }

    private void ColorMainText()
    {
        if(TotalDamage > FULL_RED_TEXT_DAMAGE)
        {
            _mainText.color = Color.red;
        }
        else if(TotalDamage == 0)
        {
            _mainText.color = Color.yellow;
        }
        else
        {
            var yellow = (FULL_RED_TEXT_DAMAGE - TotalDamage) / FULL_RED_TEXT_DAMAGE * Color.yellow;
            var red = (TotalDamage / FULL_RED_TEXT_DAMAGE) * Color.red;
            var totalColor = yellow + red;
            _mainText.color = totalColor;
        }
    }

    public void SpawnIndicator(float damage, Color color, float totalHealth)
    {
        if(_damageCountTime > 0)
        {
            TotalDamage += damage;
        }
        else
        {
            TotalDamage = damage;
            _hurtBar.value = totalHealth + damage;
        }

        _healthBar.transform.localScale = Vector3.one;
        _healthBar.value = totalHealth;

        var clr = _mainText.color;
        clr.a = 1;
        _mainText.color = clr;
        _mainText.text = $"-{Mathf.RoundToInt(TotalDamage)}";
        ColorMainText();
        _damageCountTime = DAMAGE_COUNT_TIME;

        if (_pool[_poolIndex].gameObject.activeSelf)
        {
            _poolIndex = 0;

            var indicator = Instantiate(_indicatorPrefab, transform);
            _pool.Add(indicator);
            indicator.Initialize(damage, transform.position, color);
        }
        else
        {
            _pool[_poolIndex].Initialize(damage, transform.position, color);
            _poolIndex = (_poolIndex + 1) % _pool.Count;
        }

        if(totalHealth <= 0)
        {
            _damageCountTime = .1f;
        }
    }

    private void Update()
    {
        if(_damageCountTime > 0)
        {
            _damageCountTime = Mathf.Clamp(_damageCountTime - Time.deltaTime, 0, DAMAGE_COUNT_TIME);
            if(_damageCountTime < FADE_OUT_TIME)
            {
                var color = _mainText.color;
                color.a = _damageCountTime / FADE_OUT_TIME;
                _mainText.color = color;

                _healthBar.transform.localScale = new Vector3(1, _damageCountTime / FADE_OUT_TIME, 1);
            }
            if(_damageCountTime <= 0 && _parent == null && transform.parent == null)
            {
                Destroy(gameObject);
            }
        }

        if(_parent != null)
        {
            transform.position = _parent.position + _offset;
        }
    }
}

