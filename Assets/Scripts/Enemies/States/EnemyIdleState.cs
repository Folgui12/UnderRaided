using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState<T> : State<T>
{
    private BaseEnemyModel _model;
    private BaseEnemyView _view;
    private EnemyPatrolController _controller;


    public EnemyIdleState(BaseEnemyModel model, BaseEnemyView view, EnemyPatrolController controller)
    {
        _model = model;
        _view = view;
        _controller = controller;
    }

    public override void Enter()
    {
        base.Enter();

        // Cambiamos de animación al idle
        _view.StayIdle();
        _model.ResetIdleTimer();

    }


    // Update is called once per frame
    public override void Execute()
    {
        _model.Move(Vector3.zero);

        _model.CountDown();
    }

    public override void Sleep()
    {
        base.Sleep();
        _model.ResetIdleTimer();
    }
}
