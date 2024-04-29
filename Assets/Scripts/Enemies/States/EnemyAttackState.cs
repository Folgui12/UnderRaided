using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState<T> : State<T>
{
    BaseEnemyModel _model;
    public EnemyAttackState(BaseEnemyModel model)
    {
        _model = model;
    }

    public override void Enter()
    {
        base.Enter();

        _model.Attack();
    }

    public override void Execute()
    {
        base.Execute();
        _model.Move(Vector3.zero);
        
    }
}
