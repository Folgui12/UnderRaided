using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _los = GetComponent<LoS>();
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
        patrolState = new EnemyPatrolState<StatesEnum>(_steering, _model, _view, this, _obstacleAvoidance);
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
        seek = new Seek(_model, _model.transform, currentObjective);
        //pursuit = new Pursuit(_model.transform, _model.currentObjective.GetComponent<Rigidbody>(), timePrediction);
        //evade = new Evade(_model.transform, _model.currentObjective.GetComponent<Rigidbody>(), timePrediction);

        _steering = seek;

        _obstacleAvoidance = new ObstacleAvoidance(_model.transform, angle, radius, maskObs);
    }

    // Inicializo todo el arbol
    void InitializedTree()
    {
        //Actions
        ActionNode idle = new ActionNode(() => _fsm.Transition(StatesEnum.Idle));
        //ActionNode attack = new ActionNode(() => _fsm.Transition(StatesEnum.Attack));
        ActionNode chase = new ActionNode(() => _fsm.Transition(StatesEnum.Chase));
        ActionNode patrol = new ActionNode(() => _fsm.Transition(StatesEnum.Patrol));

        //Question
        //QuestionNode qAttackRange = new QuestionNode(QuestionAttackRange, attack, chase);

        QuestionNode qLos = new QuestionNode(QuestionLos, chase, patrol);
        
        QuestionNode qIdle = new QuestionNode(QuestionWithIdleTime, idle, patrol);

        QuestionNode qPatrol = new QuestionNode(() => patrolState.IsFinishPath, qLos, patrol);
            
        _root = qPatrol;
    }

    // Pregunto si el enemigo todavia tiene tiempo para quedarse en estado de Idle
    bool QuestionWithIdleTime()
    {
        return !_model.outOfIdleTime();
    }

    // Pregunto si el player se encuentra en rango de ataque
    bool QuestionAttackRange()
    {
        return _los.CheckAttackRange(_model.playerPosition);
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

#region PATHFINDING

    public void RunAStarPlus()
    {
        var start = GetNearNode(transform.position);
        if (start == null) return;
        List<Node> path = AStar.Run(start, GetConnections, IsSatiesfies, GetCost, Heuristic);
        path = AStar.CleanPath(path, InView);
        patrolState.SetWayPoints(path);
        //PosTarget
    }

    bool InView(Node grandParent, Node child)
    {
        Debug.Log("RAY");
        return InView(grandParent.transform.position, child.transform.position);
    }

    bool InView(Vector3 a, Vector3 b)
    {
        //a->b  b-a
        Vector3 dir = b - a;
        return !Physics.Raycast(a, dir.normalized, dir.magnitude, maskObs);
    }

    float Heuristic(Node current)
    {
        float heuristic = 0;
        float multiplierDistance = 1;
        heuristic += Vector3.Distance(current.transform.position, target.transform.position) * multiplierDistance;
        return heuristic;
    }

    float GetCost(Node parent, Node child)
    {
        float cost = 0;
        float multiplierDistance = 1;
        float multiplierTrap = 200;
        cost += Vector3.Distance(parent.transform.position, child.transform.position) * multiplierDistance;
        /*if (child.hasTrap)
        {
            cost += multiplierTrap;
        }*/
        return cost;
    }

    Node GetNearNode(Vector3 pos)
    {
        var nodes = Physics.OverlapSphere(pos, radius, maskNodes);
        Node nearNode = null;
        float nearDistance = 0;
        for (int i = 0; i < nodes.Length; i++)
        {
            var currentNode = nodes[i];
            var dir = currentNode.transform.position - pos;
            float currentDistance = dir.magnitude;
            if (nearNode == null || currentDistance < nearDistance)
            {
                if (!Physics.Raycast(pos, dir.normalized, currentDistance, maskObs))
                {
                    nearNode = currentNode.GetComponent<Node>();
                    nearDistance = currentDistance;
                }
            }
        }
        return nearNode;
    }
    List<Node> GetConnections(Node current)
    {
        return current.neightbourds;
    }

    bool IsSatiesfies(Node current)
    {
        return current == target;
    }

#endregion
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_los.Origin, radius);
    }
#endif

}
