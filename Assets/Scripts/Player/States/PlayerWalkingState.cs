using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWalkingState<T> : State<T>
{
    private PlayerModel _model;
    private PlayerView _view;
    private PlayerController _controller;

    public PlayerWalkingState(PlayerModel model, PlayerView view, PlayerController controller)
    {
        _model = model;
        _view = view;
        _controller = controller;
    }

    public override void Enter()
    {
        _view.anim.SetBool("Walking", true);
    }

    public override void Execute()
    {
        if(Input.GetKey(KeyCode.W))
            _model.Move((_model.transform.forward * Time.deltaTime).normalized);

        if(Input.GetKey(KeyCode.S))
            _model.Move((-_model.transform.forward * Time.deltaTime).normalized);

        if(Input.GetKey(KeyCode.A))
            _view.LookDir(-_controller.turnSpeed);

        if(Input.GetKey(KeyCode.D))
            _view.LookDir(_controller.turnSpeed);
    }
}
