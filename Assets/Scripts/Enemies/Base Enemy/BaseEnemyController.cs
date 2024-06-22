using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyController : MonoBehaviour
{
    BaseEnemyModel _model;
    BaseEnemyView _view;
    EnemyPatrolController _pController;
    PlayerModel playerModel;

    #region STEERING
    public Rigidbody target;
    public float timePrediction;
    public float angle;
    public float radius;
    public LayerMask maskObs;
    ObstacleAvoidance _obstacleAvoidance;
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

    public Transform currentObjective;

    public LayerMask maskNodes;
    
    private EnemyPatrolState<StatesEnum> patrolState;

    #endregion

    private void Awake()
    {
        _model = GetComponent<BaseEnemyModel>();
        _view = GetComponent<BaseEnemyView>();
        _pController = GetComponent<EnemyPatrolController>();
        _los = GetComponent<LoS>();
        playerModel = FindObjectOfType<PlayerModel>();
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
        var idle = new EnemyIdleState<StatesEnum>(_model, _view);
        var attack = new EnemyAttackState<StatesEnum>(_model);
        patrolState = new EnemyPatrolState<StatesEnum>(_steering, _model, _view, _pController, _obstacleAvoidance);
        var chase = new EnemyChaseState<StatesEnum>(_steering, _model, _view, _obstacleAvoidance);

        idle.AddTransition(StatesEnum.Attack, attack);
        idle.AddTransition(StatesEnum.Patrol, patrolState);
        idle.AddTransition(StatesEnum.Chase, chase);

        attack.AddTransition(StatesEnum.Idle, idle);
        attack.AddTransition(StatesEnum.Chase, chase);
        attack.AddTransition(StatesEnum.Patrol, patrolState);

        chase.AddTransition(StatesEnum.Idle, idle);
        chase.AddTransition(StatesEnum.Patrol, patrolState);
        chase.AddTransition(StatesEnum.Attack, attack);

        patrolState.AddTransition(StatesEnum.Idle, idle);
        patrolState.AddTransition(StatesEnum.Chase, chase);

        _fsm = new FSM<StatesEnum>(idle);
    }

    // Inicializo la forma en la que los enemigos se moverán
    void InitializeSteerings()
    {
        seek = new Seek(_model, _model.transform);
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

        //Question
        //QuestionNode qAttackRange = new QuestionNode(QuestionAttackRange, attack, chase);

        QuestionNode qLos = new QuestionNode(QuestionLos, chase, patrol);
        
        QuestionNode qIdle = new QuestionNode(QuestionWithIdleTime, idle, patrol);

        QuestionNode qPatrol = new QuestionNode(() => patrolState.IsFinishPath, qLos, qIdle);
            
        _root = qPatrol;
    }

    // Pregunto si el enemigo todavia tiene tiempo para quedarse en estado de Idle
    bool QuestionWithIdleTime()
    {
        return !_model.outOfIdleTime();
    }

    // Pregunto si el player está a la vista
    bool QuestionLos()
    {
        return _los.CheckViewRange(_model.playerPosition) 
            && _los.CheckAngle(_model.playerPosition) 
            && _los.CheckView(_model.playerPosition);
    }

    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
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
