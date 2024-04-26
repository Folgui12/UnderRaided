using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState<T> : State<T>
{
    private ISteering _steering;
    private BaseEnemyModel _model;
    private BaseEnemyView _view;
    private ObstacleAvoidance _obs;

    public EnemyPatrolState(ISteering steering, BaseEnemyModel model, BaseEnemyView view, ObstacleAvoidance obs)
    {
        _steering = steering;
        _model = model;
        _view = view;
        _obs = obs;
    }

    public override void Enter()
    {
        base.Enter();

        _view.StartWalking();
    }


    // Update is called once per frame
    public override void Execute()
    {
        _model.CurrentWaypoint();

        var dir = _obs.GetDir(_steering.GetDir(), false);

        _model.Move(dir);
        _view.LookDir(dir);
    }
}
