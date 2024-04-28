using UnityEngine;

public class InGameUI : MonoBehaviour
{
    private void Start()
    {
        var forward = Camera.main.transform.forward;
        transform.LookAt(transform.position + forward);
    }
}
