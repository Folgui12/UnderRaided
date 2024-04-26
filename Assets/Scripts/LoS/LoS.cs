using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class LoS : MonoBehaviour, ILoS
{
    public LayerMask maskObs;
    private BaseEnemyModel _model;

    public GameObject objectInFront; 

    void Awake()
    {
        _model = GetComponent<BaseEnemyModel>();
    }

    // Rango de Vision
    public bool CheckViewRange(Transform target)
    {
        float distance = Vector3.Distance(target.position, Origin);
        return distance <= _model.Stats.viewRange;
    }

    public bool CheckAttackRange(Transform target)
    {
        float distance = Vector3.Distance(target.position, Origin);
        return distance <= _model.Stats.attackRange;
    }

    // Angulo de Vision
    public bool CheckAngle(Transform target)
    {
        Vector3 dirToTarget = target.position - Origin;
        float angleToTarget = Vector3.Angle(Forward, dirToTarget);
        return angleToTarget <= _model.Stats.viewAngle/2;
    }


    // Vision Obstruida
    public bool CheckView(Transform target)
    {
        return CheckView(target, maskObs);
    }
    public bool CheckView(Transform target, LayerMask maskObs)
    {
        Vector3 dirToTarget = target.position - Origin;
        float distance = dirToTarget.magnitude;
        return !Physics.Raycast(Origin, dirToTarget, distance, maskObs);
    }

    public bool CheckForTrees(Transform target, LayerMask maskObs)
    {
        Vector3 dirToTarget = target.position - Origin;
        float distance = dirToTarget.magnitude;
        RaycastHit hit;

        return Physics.Raycast(Origin, dirToTarget, out hit, distance, maskObs) && CheckAttackRange(hit.transform);
    }

    public Vector3 Origin => transform.position;
    public Vector3 Forward => transform.forward;
}
