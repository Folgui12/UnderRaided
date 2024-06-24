using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionFollowState<T> : State<T>
{
    ISteering _steering;
    MinionModel _miModel;
    ObstacleAvoidance _obs;
    public MinionFollowState(MinionModel model, ISteering steering, ObstacleAvoidance obs)
    {
        _steering = steering;
        _miModel = model;
        _obs = obs;
    }
    public override void Execute()
    {
        var dir = _obs.GetDir(_steering.GetDir(), false);


        _miModel.Move(dir);
        _miModel.LookDir(dir);
    }
}
