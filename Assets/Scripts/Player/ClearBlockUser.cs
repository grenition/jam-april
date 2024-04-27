using UnityEngine;

public class ClearBlockUser : MonoBehaviour
{
    private bool _isUseBlock = false;

    public float BlockTimer { get; private set; } = 0;

    public void Block()
    {
        if (_isUseBlock)
            return;

        _isUseBlock = true;
        BlockTimer = 0;
    }

    public void UnBlock()
    {
        _isUseBlock = false;
        BlockTimer = 0;
    }

    private void Update()
    {
        if(_isUseBlock)
        {
            BlockTimer += Time.deltaTime;
        }
    }
}
