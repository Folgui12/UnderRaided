using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EnemyChaseState<T> : State<T>
{
    private BaseEnemyModel _model;
    private BaseEnemyView _view;
    private BaseEnemyController _controller; 
    private ObstacleAvoidance _obs;

    public EnemyChaseState(BaseEnemyModel model, BaseEnemyView view, BaseEnemyController controller, ObstacleAvoidance obs)
    {
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

        foreach (MinionModel minion in _controller.myMinions)
        {
            minion.speed = 2.5f; 
        }
    }

    public override void Execute()
    {
        var dir = _obs.GetDir(_controller.CalculateDirectionToPlayer(), false).normalized;

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
