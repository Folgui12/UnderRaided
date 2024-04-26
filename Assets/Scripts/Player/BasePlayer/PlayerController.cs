using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float turnSpeed;
    PlayerModel _playerModel;
    PlayerView _playerView;
    PlayerController _playerController;
    
    FSM<PlayerStatesEnum> _fsm;
    ITreeNode _root;

    void Awake()
    {
        _playerModel = GetComponent<PlayerModel>();
        _playerView = GetComponent<PlayerView>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        InitializedTree();
        InitializeFSM();
    }

    void InitializeFSM()
    {
        var idle = new PlayerIdleState<PlayerStatesEnum>(_playerModel, _playerView);
        var walk = new PlayerWalkingState<PlayerStatesEnum>(_playerModel, _playerView, _playerController);
        var death = new PlayerDeathState<PlayerStatesEnum>();

        idle.AddTransition(PlayerStatesEnum.Dead, death);
        idle.AddTransition(PlayerStatesEnum.Walk, walk);

        walk.AddTransition(PlayerStatesEnum.Dead, death);
        walk.AddTransition(PlayerStatesEnum.Idle, idle);

        _fsm = new FSM<PlayerStatesEnum>(idle);
    }

    void InitializedTree()
    {
        //Actions
        //var idle = new ActionNode(() => _fsm.Transition(StatesEnum.Idle));
        var idle = new ActionNode(() => _fsm.Transition(PlayerStatesEnum.Idle));
        var walk = new ActionNode(() => _fsm.Transition(PlayerStatesEnum.Walk));
        var death = new ActionNode(() => _fsm.Transition(PlayerStatesEnum.Dead));

        //Question
        var qIsMoving = new QuestionNode(QuestionIsMoving, walk, idle);
        var qHasLife = new QuestionNode(QuestionHasLife, qIsMoving, death);

        _root = qHasLife;
    }

    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }

    bool QuestionHasLife()
    {
        return _playerModel.life > 0;
    }

    bool QuestionIsMoving()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
    }

    
}
