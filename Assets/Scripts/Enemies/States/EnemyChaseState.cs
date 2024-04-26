using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EnemyChaseState<T> : State<T>
{
    private ISteering _steering;
    private BaseEnemyModel _model;
    private BaseEnemyView _view;
    private ObstacleAvoidance _obs;

    public EnemyChaseState(ISteering steering, BaseEnemyModel model, BaseEnemyView view, ObstacleAvoidance obs)
    {
        _steering = steering;
        _model = model;
        _view = view;
        _obs = obs;
    }

    public override void Enter()
    {
        _view.ActiveNoise();
    }

    public override void Execute()
    {

        var dir = _obs.GetDir(_model.CalculateDirectionToPlayer(), false).normalized;

        _model.Move(dir);
        _view.LookDir(dir);
    }
}
