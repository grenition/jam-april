using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameMessager : MonoBehaviour
{
    public void EndGameMessage()
    {
        StartCoroutine(EndGameIE());
    }

    private IEnumerator EndGameIE()
    {
        foreach(var go in FindObjectsOfType<BlobMeshAnim>())
        {
            go.DestroyBlob();
        }
        yield return new WaitForSeconds(3.5f);
        var gameLifeTime = ServiceLocator.Get<GameLifetime>();
        gameLifeTime?.EndGame();
    }
}
