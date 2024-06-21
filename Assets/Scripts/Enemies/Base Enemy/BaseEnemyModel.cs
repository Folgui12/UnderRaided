using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyModel : MonoBehaviour
{

    [SerializeField] private EnemyStats _stats;
    [SerializeField] private Collider rightArm;
    [SerializeField] private float idleTimerSetter;

    public Transform[] patrolPoints;

    public int targetPoint;
    public EnemyStats Stats => _stats;
    public Transform currentObjective;
    public Transform playerPosition;
    public float idleTimer;
    public bool inOrder;
    public bool onLastPoint;
    public bool onFirstPoint;

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
        inOrder = false;
        ResetIdleTimer();
    }

    // Muevo al enemigo en la dirección que se le pasa por parámetro
    public void Move(Vector3 dir)
    {
        dir *= _stats.travelSpeed;
        dir.y = _rb.velocity.y;
        _rb.velocity = dir;
    }

    // Calculo el vector director hacia la posición del jugador para el estado de Perseguir o Chase
    public Vector3 CalculateDirectionToPlayer()
    {
        return (playerPosition.position - transform.position).normalized;
    }

    // Verificador para saber si se agotó el tiempo de Idle
    public bool outOfIdleTime()
    {
        return idleTimer <= 0;
    }

    // Devuelvo el tiempo de Idle para el próximo uso
    public void ResetIdleTimer()
    {
        idleTimer = idleTimerSetter;
    }

    // Activo ataque
    public void Attack()
    {
        _view.ActiveAttack();
    }

    // Activo y Desactivo el brazo con el que golpea el enemigo
    public void ActiveRightArm()
    {
        rightArm.enabled = true;
    }
    public void DeactiveRightArm()
    {
        rightArm.enabled = false;
    }

    public Vector3 Origin => transform.position;
    public Vector3 Forward => transform.forward;

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
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

    // Guardo el punto de patrullaje actual, para recordarlo despues de Perseguir
    /*public void CurrentWaypoint()
    {
        currentObjective = patrolPoints[targetPoint];
    }*/

    // Sumo el index de los Puntos de Patrullaje
    /*private void IncreaseTargetInt()
    {
        if(targetPoint < patrolPoints.Length-1)
            targetPoint++;
    }

    // Resto el index de los Pntos de Patrullaje
    private void DecreaseTargetInt()
    {   
        if(targetPoint >= 1)
            targetPoint--;
    }*/


    // Chequeo que el enemigo llegó al siguiente punto de patrullaje
    /*void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PatrolPoint") && inOrder)
        {
            patrolPoints[targetPoint].gameObject.SetActive(false);
            if(targetPoint == patrolPoints.Length-1)
            {
                onLastPoint = true;
            }  
            IncreaseTargetInt();
        }
        
        else if(other.gameObject.CompareTag("PatrolPoint") && !inOrder)
        {
            patrolPoints[targetPoint].gameObject.SetActive(false);
            if(targetPoint == 0)
                onFirstPoint = true;
            DecreaseTargetInt();
        }
    }

    // Activo todos los puntos de patrullaje para una nueva ronda
    public void ActiveAllPatrolPoints()
    {
        foreach (Transform points in patrolPoints)
        {
            points.gameObject.SetActive(true);
        }
        CurrentWaypoint();
        ResetPositionCheckers();
    }

    // Reinicio las variables que me confirman cuando el enemigo finalizó una ronda de patrullaje
    private void ResetPositionCheckers()
    {
        onFirstPoint = false;
        onLastPoint = false;
    }

    // Checkeo si el enemigo está en el último punto de patrullaje
    public bool onLastPatrolPoint()
    {
        return onLastPoint;
    }

    // Checkeo si el enemigo está en el primer punto de patrullaje
    public bool onFirstPatrolPoint()
    {
        return onFirstPoint;
    }*/

    
}
