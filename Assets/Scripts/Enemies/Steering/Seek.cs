using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : ISteering
{
    Transform _entity;
    Transform _target;
    BaseEnemyModel _model;
    EnemyPatrolController _controller;
    
    public Seek(BaseEnemyModel model, Transform entity, EnemyPatrolController controller)
    {
        _entity = entity;
        _model = model;
        _controller = controller;
    }

    public Vector3 GetDir()
    {
        _target = _controller.CurrentObjective;
        return (_target.position - _entity.position).normalized;
    }
}
