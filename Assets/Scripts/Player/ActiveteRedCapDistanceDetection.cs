using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveteRedCapDistanceDetection : MonoBehaviour
{
    public void Activate()
    {
        var ui = ServiceLocator.Get<UIComebackToRedCapPanel>();

        if(ui != null)
        {
            ui.Active = true;
        }
    }
}
