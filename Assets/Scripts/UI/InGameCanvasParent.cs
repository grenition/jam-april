using UnityEngine;

public class InGameCanvasParent : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Register(this);
    }
}
