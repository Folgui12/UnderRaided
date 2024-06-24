using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyController : MonoBehaviour
{
    BaseEnemyModel _model;
    BaseEnemyView _view;
    EnemyPatrolController _pController;
    PlayerModel playerModel;

    [SerializeField] private float idleTimerSetter;
    private float idleTimer;

    #region STEERING
    public float timePrediction;
    public float angle;
    public float radius;
    public LayerMask maskObs;
    ObstacleAvoidance _obstacleAvoidance;
    public bool SetToEvade;
    #endregion

    #region INTERFACE
    [SerializeField] LoS _los;
    FSM<StatesEnum> _fsm;
    ITreeNode _root;
    ISteering _steering;
    ISteering seek;
    public ISteering pursuit;
    public ISteering evade;
    #endregion

    #region  Pathfinding

    public LayerMask maskNodes;
    
    public EnemyPatrolState<StatesEnum> patrolState;

    #endregion

    private void Awake()
    {
        _model = GetComponent<BaseEnemyModel>();
        _view = GetComponent<BaseEnemyView>();
        _pController = GetComponent<EnemyPatrolController>();
        _los = GetComponent<LoS>();
        playerModel = FindObjectOfType<PlayerModel>();
        ResetIdleTimer();
    }

    private void Start()
    {
        InitializeSteerings();
        InitializedTree();
        InitializeFSM();
    }

    // Inicializo la máquina de estados
    void InitializeFSM()
    {
        var idle = new EnemyIdleState<StatesEnum>(_model, _view, this);
        var ScapeFromPlayer = new EnemyEvadeState<StatesEnum>(evade, _model, _view, this,_obstacleAvoidance);
        patrolState = new EnemyPatrolState<StatesEnum>(_model, _view, _pController, _obstacleAvoidance);
        var chase = new EnemyChaseState<StatesEnum>(_model, _view, this,_obstacleAvoidance);

        idle.AddTransition(StatesEnum.Evade, ScapeFromPlayer);
        idle.AddTransition(StatesEnum.Patrol, patrolState);
        idle.AddTransition(StatesEnum.Chase, chase);

        ScapeFromPlayer.AddTransition(StatesEnum.Idle, idle);
        ScapeFromPlayer.AddTransition(StatesEnum.Chase, chase);
        ScapeFromPlayer.AddTransition(StatesEnum.Patrol, patrolState);

        chase.AddTransition(StatesEnum.Idle, idle);
        chase.AddTransition(StatesEnum.Patrol, patrolState);
        chase.AddTransition(StatesEnum.Evade, ScapeFromPlayer);

        patrolState.AddTransition(StatesEnum.Idle, idle);
        patrolState.AddTransition(StatesEnum.Chase, chase);

        _fsm = new FSM<StatesEnum>(idle);
    }

    // Inicializo la forma en la que los enemigos se moverán
    void InitializeSteerings()
    {
        seek = new Seek(_model, _model.transform, _pController);
        pursuit = new Pursuit(_model.transform, playerModel.GetComponent<Rigidbody>(), timePrediction);
        evade = new Evade(_model.transform, playerModel.GetComponent<Rigidbody>(), timePrediction);

        _steering = seek;

        _obstacleAvoidance = new ObstacleAvoidance(_model.transform, angle, radius, maskObs);
    }

    // Inicializo todo el arbol
    void InitializedTree()
    {
        //Actions
        ActionNode idle = new ActionNode(() => _fsm.Transition(StatesEnum.Idle));
        ActionNode chase = new ActionNode(() => _fsm.Transition(StatesEnum.Chase));
        ActionNode patrol = new ActionNode(() => _fsm.Transition(StatesEnum.Patrol));
        ActionNode escape = new ActionNode(() => _fsm.Transition(StatesEnum.Evade));

        //Question
        //QuestionNode qAttackRange = new QuestionNode(QuestionAttackRange, attack, chase);

        QuestionNode qSetToEvade = new QuestionNode(() => SetToEvade, escape, chase);

        QuestionNode qLos = new QuestionNode(QuestionLos, qSetToEvade, patrol);
        
        QuestionNode qIdle = new QuestionNode(QuestionWithIdleTime, idle, patrol);

        QuestionNode qPatrol = new QuestionNode(() => patrolState.IsFinishPath, qIdle, qLos);
            
        _root = qPatrol;
    }

    // Calculo el vector director hacia la posición del jugador para el estado de Perseguir o Chase
    public Vector3 CalculateDirectionToPlayer()
    {
        return (playerModel.transform.position - transform.position).normalized;
    }

    public void CountDown()
    {
        idleTimer -= Time.deltaTime;
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

    // Pregunto si el enemigo todavia tiene tiempo para quedarse en estado de Idle
    bool QuestionWithIdleTime()
    {
        return !outOfIdleTime();
    }

    // Pregunto si el player está a la vista
    bool QuestionLos()
    {
        return _los.CheckViewRange(playerModel.transform) 
            && _los.CheckAngle(playerModel.transform) 
            && _los.CheckView(playerModel.transform);
    }

    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }

    public void ChangeSteeringToSeek()
    {
        _steering = seek;
        _model.SetSpeedToPatrol();
    }

    public void ChangeSteeringToPursuit()
    {
        _steering = pursuit;
        _model.SetSpeedToPursuit();
    }

    public void ChangeSpeedToEvade()
    {
        _model.SetSpeedToPatrol();
    }

public EnemyPatrolState<StatesEnum> GetStateWaypoints => patrolState;
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_los.Origin, radius);
    }
#endif

}
