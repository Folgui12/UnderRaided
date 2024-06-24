using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionController : MonoBehaviour
{
    public Rigidbody target;
    public float timePrediction;
    public float angle;
    public float radius;
    public LayerMask maskObs;
    FSM<StatesEnum> _fsm;
    ISteering _steering;
    ITreeNode _root;
    MinionModel _miModel;
    ObstacleAvoidance _obstacleAvoidance;
    AvoidanceBehaviour avoidanceBehaviour;
    MinionView _view;

    private void Awake()
    {
        _miModel = GetComponent<MinionModel>();
        _view = GetComponent<MinionView>();
        avoidanceBehaviour = GetComponent<AvoidanceBehaviour>();
        InitializeSteerings();
        InitializedTree();
        InitializeFSM();
    }

    void InitializeSteerings()
    {
        _steering = GetComponent<FlockingManager>();
        _obstacleAvoidance = new ObstacleAvoidance(_miModel.transform, angle, radius, maskObs);
    }

    void InitializeFSM()
    {
        var idle = new MinionIdleState<StatesEnum>(_view, _miModel);
        var follow = new MinionFollowState<StatesEnum>(_miModel, _steering, _obstacleAvoidance);

        idle.AddTransition(StatesEnum.Patrol, follow);

        follow.AddTransition(StatesEnum.Idle, idle);

        _fsm = new FSM<StatesEnum>(idle);
    }

    void InitializedTree()
    {
        //Actions
        ActionNode idle = new ActionNode(() => _fsm.Transition(StatesEnum.Idle));
        ActionNode follow = new ActionNode(() => _fsm.Transition(StatesEnum.Patrol));

        QuestionNode qToClose = new QuestionNode(QuestionToClose, idle, follow);
            
        _root = qToClose;
    }

    public bool QuestionToClose()
    {
        Debug.Log(Vector3.Distance(transform.position, target.transform.position) < avoidanceBehaviour.personalArea);
        return Vector3.Distance(transform.position, target.transform.position) < avoidanceBehaviour.personalArea;
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
    }
}
