using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class DamageIndicator : MonoBehaviour
{
    private TMP_Text _text;
    private float _maxSpeed = 1;
    private float _speed = 1;
    private float _acceleration = -.5f;

    public const float LIFE_TIME = .5f;

    private Vector3 _moveDirection = Vector3.right;
    private bool _isActive = false;

    public void Initialize(float damage, Vector3 pos, Color color)
    {
        _speed = _maxSpeed;
        transform.position = pos;
        transform.localScale = Vector3.one * .2f;
        gameObject.SetActive(true);
        _text = GetComponent<TMP_Text>();
        _text.color = color;
        _text.text = Mathf.RoundToInt(damage).ToString();
        if(Random.Range(0, 3) == 0)
        {
            _moveDirection = Vector3.left;
        }
        transform.position += _moveDirection * Random.Range(0f, 1f);
        transform.position += Vector3.forward * Random.Range(-1f, 1f);
        _isActive = true;
        StartCoroutine(TimerIE());
    }

    private IEnumerator TimerIE()
    {
        yield return new WaitForSeconds(LIFE_TIME / 2);

        _text.transform.DOScale(Vector3.zero, LIFE_TIME / 2);

        yield return new WaitForSeconds(LIFE_TIME / 2);
        _isActive = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(_isActive)
        {
            transform.position += _moveDirection * _speed * Time.deltaTime;
            _speed = Mathf.Clamp(_speed + _acceleration * Time.deltaTime, 0, 100);
        }
    }
}
