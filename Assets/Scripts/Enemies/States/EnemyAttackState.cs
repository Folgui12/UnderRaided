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

    public override void Execute()
    {
        base.Execute();
        _model.Move(Vector3.zero);
        _model.Attack();
    }
}
