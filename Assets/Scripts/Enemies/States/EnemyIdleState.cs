using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState<T> : State<T>
{
    private BaseEnemyModel _model;
    private BaseEnemyView _view;


    public EnemyIdleState(BaseEnemyModel model, BaseEnemyView view)
    {
        _model = model;
        _view = view;
    }

    public override void Enter()
    {
        base.Enter();

        _view.StayIdle();
    }


    // Update is called once per frame
    public override void Execute()
    {
        _model.Move(Vector3.zero);

        if(_model.idleTimer > 0)
        {
            _model.idleTimer -= Time.deltaTime;
        }
    }

    public override void Sleep()
    {
        base.Sleep();

        _model.ActiveAllPatrolPoints();
        _model.inOrder = !_model.inOrder;
    }
}
