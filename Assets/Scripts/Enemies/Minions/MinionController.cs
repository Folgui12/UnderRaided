using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionController : MonoBehaviour
{
    public Rigidbody target;
    public float timePrediction;
    public float angle;
    public float radius;
    public float closeToLeaderRadius; 
    public LayerMask maskObs;
    FSM<StatesEnum> _fsm;
    ISteering _steering;
    ITreeNode _root;
    MinionModel _miModel;
    ObstacleAvoidance _obstacleAvoidance;
    AvoidanceBehaviour avoidanceBehaviour;
    MinionView _view;
    ISteering seek;
    FlockingManager flockingManager;


    private void Awake()
    {
        _miModel = GetComponent<MinionModel>();
        _view = GetComponent<MinionView>();
        avoidanceBehaviour = GetComponent<AvoidanceBehaviour>();
        flockingManager = GetComponent<FlockingManager>();
        InitializeSteerings();
        InitializedTree();
        InitializeFSM();
    }

    void InitializeSteerings()
    {   
        seek = new MinionSeek(target.transform, _miModel.transform);

        _steering = flockingManager;

        _obstacleAvoidance = new ObstacleAvoidance(_miModel.transform, angle, radius, maskObs);
    }

    void InitializeFSM()
    {
        var idle = new MinionIdleState<StatesEnum>(_view, _miModel);
        var follow = new MinionFollowState<StatesEnum>(_miModel, this, _steering, _obstacleAvoidance);
        var goToLeader = new MinionLostState<StatesEnum>(_miModel, this, _steering, _obstacleAvoidance);

        idle.AddTransition(StatesEnum.Patrol, follow);

        follow.AddTransition(StatesEnum.Idle, idle);
        follow.AddTransition(StatesEnum.Chase, goToLeader);

        goToLeader.AddTransition(StatesEnum.Patrol, follow);
        goToLeader.AddTransition(StatesEnum.Idle, idle);


        _fsm = new FSM<StatesEnum>(idle);
    }

    void InitializedTree()
    {
        //Actions
        ActionNode idle = new ActionNode(() => _fsm.Transition(StatesEnum.Idle));
        ActionNode follow = new ActionNode(() => _fsm.Transition(StatesEnum.Patrol));
        ActionNode seekLeader = new ActionNode(() => _fsm.Transition(StatesEnum.Chase));

        QuestionNode qIsToFar = new QuestionNode(QuestionToFar, seekLeader, follow); 

        QuestionNode qToClose = new QuestionNode(QuestionToClose, idle, qIsToFar);
            
        _root = qToClose;
    }

    public bool QuestionToClose()
    {
        return Vector3.Distance(transform.position, target.transform.position) < avoidanceBehaviour.personalArea;
    }

    public bool QuestionToFar()
    {
        return Vector3.Distance(transform.position, target.transform.position) > closeToLeaderRadius;
    }

    public void ChangeSteerToSeek()
    {
        _steering = seek;
    }

    public void SetSteerToFollow()
    {
        _steering = flockingManager;
    }

    void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * radius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * radius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, closeToLeaderRadius);
    }
}
