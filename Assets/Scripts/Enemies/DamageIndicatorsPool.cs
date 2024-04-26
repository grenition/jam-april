using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageIndicatorsPool : MonoBehaviour
{
    [SerializeField] private DamageIndicator _indicatorPrefab;
    [SerializeField] private TMP_Text _mainText;

    public const float DAMAGE_COUNT_TIME = 3.5f;
    public const float FADE_OUT_TIME = .5f;
    public const float FULL_RED_TEXT_DAMAGE = 160;

    private readonly List<DamageIndicator> _pool = new();
    private int _poolIndex = 0;
    private float _damageCountTime = 0;

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

    public void SpawnIndicator(float damage, DamageType type)
    {
        if(type == DamageType.HEAL)
        {
            SpawnIndicator(damage, Color.green);
        }
        else if(type == DamageType.QUICK_ATTACK)
        {
            SpawnIndicator(damage, Color.yellow);
        }
        else
        {
            SpawnIndicator(damage, Color.red);
        }
    }

    public void SpawnIndicator(float damage)
    {
        SpawnIndicator(damage, Color.yellow);
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

    public void SpawnIndicator(float damage, Color color)
    {
        if(_damageCountTime > 0)
        {
            TotalDamage += damage;
        }
        else
        {
            TotalDamage = damage;
        }

        var clr = _mainText.color;
        clr.a = 1;
        _mainText.color = clr;
        _mainText.text = $"-{TotalDamage}";
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
            }
        }
    }
}
