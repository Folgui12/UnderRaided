using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : ISteering
{
    Transform _entity;
    Transform _target;
    BaseEnemyModel _model;
    BaseEnemyController _controller;
    
    public Seek(BaseEnemyModel model, Transform entity, Transform target)
    {
        _entity = entity;
        _target = target;
        _model = model;
    }

    public Vector3 GetDir()
    {
        _target = _controller.currentObjective;
        return (_target.position - _entity.position).normalized;
    }
}
