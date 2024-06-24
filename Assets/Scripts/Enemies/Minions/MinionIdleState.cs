using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionIdleState<T> : State<T>
{
    MinionView _view;
    MinionModel _model; 

    public MinionIdleState(MinionView view, MinionModel model)
    {
        _view = view;
        _model = model;
    }

    public override void Enter()
    {
        _view.SetIdleAnimation();
    }

    public override void Execute()
    {
        _model.Move(Vector3.zero);
    }

    public override void Sleep()
    {
        _view.SetWalkingAnimation();
    }
    
}
