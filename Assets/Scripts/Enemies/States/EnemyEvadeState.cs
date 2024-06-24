using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvadeState<T> : State<T>
{
    private ISteering _steering;
    private BaseEnemyModel _model;
    private BaseEnemyView _view;
    private BaseEnemyController _controller; 
    private ObstacleAvoidance _obs;

    public EnemyEvadeState(ISteering steering, BaseEnemyModel model, BaseEnemyView view, BaseEnemyController controller, ObstacleAvoidance obs)
    {
        _steering = steering;
        _model = model;
        _view = view;
        _controller = controller; 
        _obs = obs;
    }

    public override void Enter()
    {
        _controller.ChangeSpeedToEvade(); 
        
        _view.ChangeNoiseToPursuit();
        _view.StartSprinting();
    }

    public override void Execute()
    {
        var dir = _obs.GetDir(_steering.GetDir(), false).normalized;

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
