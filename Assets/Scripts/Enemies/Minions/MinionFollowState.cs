using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionFollowState<T> : State<T>
{
    ISteering _steering;
    MinionModel _miModel;
    ObstacleAvoidance _obs;
    MinionController _controller;
    public MinionFollowState(MinionModel model, MinionController controller,ISteering steering, ObstacleAvoidance obs)
    {
        _controller = controller;
        _steering = steering;
        _miModel = model;
        _obs = obs;
    }

    public override void Enter()
    {
        base.Enter();

        _controller.SetSteerToFollow();
        _miModel.speed = 1f; 
    }

    public override void Execute()
    {
        var dir = _obs.GetDir(_steering.GetDir(), false);


        _miModel.Move(dir);
        _miModel.LookDir(dir);
    }

}
