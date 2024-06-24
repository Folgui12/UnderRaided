using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState<T> : State<T>
{
    private BaseEnemyModel _model;
    private BaseEnemyView _view;
    private BaseEnemyController _controller;


    public EnemyIdleState(BaseEnemyModel model, BaseEnemyView view, BaseEnemyController controller)
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
        _controller.ResetIdleTimer();

    }


    // Update is called once per frame
    public override void Execute()
    {
        _model.Move(Vector3.zero);

        _controller.CountDown();
    }

    public override void Sleep()
    {
        base.Sleep();
        _controller.ResetIdleTimer();
    }
}
