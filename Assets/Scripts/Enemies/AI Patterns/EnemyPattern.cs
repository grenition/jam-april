using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyPattern : MonoBehaviour
{
    protected BaseEnemy Enemy { get; private set; }
    protected NavMeshPath Path { get; private set; }
    
    public bool Working { get; private set; }

    protected virtual void Start()
    {
        Path = new();
    }

    public void Initialize(BaseEnemy relatedEnemy)
    {
        Enemy = relatedEnemy;
    }

    public void StartPattern()
    {
        if(Enemy == null)
        {
            Debug.LogWarning("Please, initialize enemy pattern before start", this);
            return;
        }
        Working = true;
    }

    public void StopPattern()
    {
        Working = false;
    }

    private void Update()
    {
        if(Working)
        {
            WorkUpdate();
        }
    }

    public abstract bool ReCalculatePath();
    public abstract bool ToNextPoint();
    protected abstract void WorkUpdate();
}
