using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Titles : MonoBehaviour
{
    [SerializeField] private TMP_Text _text1, _text2;

    private IEnumerator Start()
    {
        _text1.DOColor(Color.white, 1);

        yield return new WaitForSeconds(3);

        _text1.DOColor(new Color(1, 1, 1, 0), 1);
        _text2.DOColor(Color.white, 1);

        yield return new WaitForSeconds(10);

        _text2.DOColor(new Color(1, 1, 1, 0), 1);

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(0);
    }
}
