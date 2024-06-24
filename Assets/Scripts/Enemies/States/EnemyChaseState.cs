using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EnemyChaseState<T> : State<T>
{
    private ISteering _steering;
    private BaseEnemyModel _model;
    private BaseEnemyView _view;
    private BaseEnemyController _controller; 
    private ObstacleAvoidance _obs;

    public EnemyChaseState(ISteering steering, BaseEnemyModel model, BaseEnemyView view, BaseEnemyController controller, ObstacleAvoidance obs)
    {
        _steering = steering;
        _model = model;
        _view = view;
        _controller = controller; 
        _obs = obs;
    }

    public override void Enter()
    {
        _controller.ChangeSteeringToPursuit(); 
        
        _view.ChangeNoiseToPursuit();
        _view.StartSprinting();
    }

    public override void Execute()
    {
        var dir = _obs.GetDir(_controller.CalculateDirectionToPlayer(), false).normalized;

        if(_controller.SetToEvade)
        {
            _controller.ChangeSteeringToEvade();
        }
        else
        {
            _controller.ChangeSteeringToPursuit(); 
        }

        _view.StepNoise();

        _model.Move(dir);
        _view.LookDir(dir);
    }

    public override void Sleep()
    {
        base.Sleep();

        _controller.ChangeSteeringToSeek();
        _controller.SetToEvade = false; 
    }
}
