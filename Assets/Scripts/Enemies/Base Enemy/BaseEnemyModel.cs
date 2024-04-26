using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyModel : MonoBehaviour
{

    [SerializeField] private EnemyStats _stats;
    [SerializeField] private Collider rightArm;

    public Transform[] patrolPoints;

    public int targetPoint;

    public EnemyStats Stats => _stats;
    public Transform currentObjective;
    public Transform playerPosition;

    Rigidbody _rb;

    LoS lineOfSight;

    private BaseEnemyView _view;
    
    private void Awake()
    {
        targetPoint = 0;
        rightArm.enabled = false;
        _rb = GetComponent<Rigidbody>();
        lineOfSight = GetComponent<LoS>();
        currentObjective = patrolPoints[0];
        _view = GetComponent<BaseEnemyView>();
    }

    public void Move(Vector3 dir)
    {
        dir *= _stats.travelSpeed;
        dir.y = _rb.velocity.y;
        _rb.velocity = dir;
    }

    public void CurrentWaypoint()
    {
        currentObjective = patrolPoints[targetPoint];
    }

    public Vector3 CalculateDirectionToPlayer()
    {
        return (playerPosition.position - transform.position).normalized;
    }

    private void IncreaseTargetInt()
    {
        targetPoint++;
        ResetTargetPoint();
        currentObjective = patrolPoints[targetPoint];
    }

    private void ResetTargetPoint()
    {
        if(targetPoint >= patrolPoints.Length)
            targetPoint = 0;
    }

    public void Attack()
    {
        _view.ActiveAttack();
    }

    /*public void ActiveRightArm()
    {
        rightArm.enabled = true;
    }

    public void DeactiveRightArm()
    {
        rightArm.enabled = false;
    }*/

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PatrolPoint"))
        {
            patrolPoints[targetPoint].gameObject.SetActive(false);
            if(targetPoint-1 < 0)
            {
                patrolPoints[patrolPoints.Length-1].gameObject.SetActive(true);
            }
            else
            {
                patrolPoints[targetPoint-1].gameObject.SetActive(true);
            }
            IncreaseTargetInt();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;      
        Gizmos.DrawWireSphere(Origin, _stats.viewRange);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, _stats.viewAngle/2, 0) * Forward * _stats.viewRange);
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, -_stats.viewAngle/2, 0) * Forward * _stats.viewRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Origin, _stats.attackRange);
    }
#endif

    public Vector3 Origin => transform.position;
    public Vector3 Forward => transform.forward;
}
