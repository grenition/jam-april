using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCapMovementSpline : MovementSpline 
{
    public void MoveRedCap()
    {
        var redCapLifetime = ServiceLocator.Get<RedCapLifetime>();
        var redcap = redCapLifetime != null ? redCapLifetime.RedCap : null;

        MoveAgent(redcap.Agent);
    }
}
