using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class MovementSpline : MonoBehaviour
{
    [SerializeField] protected float _thresholdDistance = 1.5f;

    protected SplineContainer _splineContainer;

    private void Awake()
    {
        _splineContainer = GetComponent<SplineContainer>();
    }

    public virtual void MoveAgent(GameObject agentObj)
    {
        if (!agentObj.TryGetComponent(out NavMeshAgent agent))
            return;
        MoveAgent(agent);
    }
    public virtual void MoveAgent(NavMeshAgent agent)
    {
        StopAllCoroutines();
        StartCoroutine(MovementCoroutine(_splineContainer.Spline, agent));
    }
    public virtual void StopAgentForce(GameObject agentObj)
    {
        if (!agentObj.TryGetComponent(out NavMeshAgent agent))
            return;
        StopAgentForce(agent);
    }
    public virtual void StopAgentForce(NavMeshAgent agent)
    {
        agent.destination = agent.transform.position;
        StopAllCoroutines();
    }
    private IEnumerator MovementCoroutine(Spline spline, NavMeshAgent agent)
    {
        int i = 0;
        while (i < spline.Count)
        {
            Vector3 point = transform.TransformPoint(spline[i].Position);

            agent.destination = point;
            yield return new WaitUntil(() => Vector3.Distance(agent.transform.position, point) < _thresholdDistance);

            i++;
        }
    }
}
