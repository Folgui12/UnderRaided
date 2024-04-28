using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseEnemyController : MonoBehaviour
{
    BaseEnemyModel _model;
    BaseEnemyView _view;

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
    #endregion

    private void Awake()
    {
        _model = GetComponent<BaseEnemyModel>();
        _view = GetComponent<BaseEnemyView>();
        _los = GetComponent<LoS>();
    }

    private void Start()
    {
        InitializeSteerings();
        InitializedTree();
        InitializeFSM();
    }

    void InitializeFSM()
    {
        var idle = new EnemyIdleState<StatesEnum>(_model, _view);
        var attack = new EnemyAttackState<StatesEnum>(_model);
        var patrol = new EnemyPatrolState<StatesEnum>(_steering, _model, _view, _obstacleAvoidance);
        var chase = new EnemyChaseState<StatesEnum>(_steering, _model, _view, _obstacleAvoidance);

        idle.AddTransition(StatesEnum.Attack, attack);
        idle.AddTransition(StatesEnum.Patrol, patrol);
        idle.AddTransition(StatesEnum.Chase, chase);

        attack.AddTransition(StatesEnum.Idle, idle);
        attack.AddTransition(StatesEnum.Chase, chase);
        attack.AddTransition(StatesEnum.Patrol, patrol);

        chase.AddTransition(StatesEnum.Idle, idle);
        chase.AddTransition(StatesEnum.Patrol, patrol);
        chase.AddTransition(StatesEnum.Attack, attack);

        patrol.AddTransition(StatesEnum.Idle, idle);
        patrol.AddTransition(StatesEnum.Chase, chase);

        _fsm = new FSM<StatesEnum>(idle);
    }
    void InitializeSteerings()
    {
        var seek = new Seek(_model,_model.transform, _model.currentObjective);
        var flee = new Flee(_model.transform, _model.currentObjective);
        var pursuit = new Pursuit(_model.transform, _model.currentObjective.GetComponent<Rigidbody>(), timePrediction);
        var evade = new Evade(_model.transform, _model.currentObjective.GetComponent<Rigidbody>(), timePrediction);

        _steering = seek;

        _obstacleAvoidance = new ObstacleAvoidance(_model.transform, angle, radius, maskObs);
    }
    void InitializedTree()
    {
        //Actions
        ActionNode idle = new ActionNode(() => _fsm.Transition(StatesEnum.Idle));
        ActionNode attack = new ActionNode(() => _fsm.Transition(StatesEnum.Attack));
        ActionNode chase = new ActionNode(() => _fsm.Transition(StatesEnum.Chase));
        ActionNode patrol = new ActionNode(() => _fsm.Transition(StatesEnum.Patrol));

        //Question

        QuestionNode qAttackRange = new QuestionNode(QuestionAttackRange, attack, chase);
        QuestionNode qLos = new QuestionNode(QuestionLos, qAttackRange, patrol);
            
        _root = qLos;
    }
    bool QuestionAttackRange()
    {
        return _los.CheckAttackRange(_model.playerPosition);
    }

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
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_los.Origin, radius);
    }
#endif
}
