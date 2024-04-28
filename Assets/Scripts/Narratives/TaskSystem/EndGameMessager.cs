using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameMessager : MonoBehaviour
{
    public void EndGameMessage()
    {
        var gameLifeTime = ServiceLocator.Get<GameLifetime>();
        gameLifeTime?.EndGame();
    }
}
