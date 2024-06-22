using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : ISteering
{
    Transform _entity;
    Transform _target;
    BaseEnemyModel _model;
    BaseEnemyController _controller;
    
    public Seek(BaseEnemyModel model, Transform entity)
    {
        _entity = entity;
        _model = model;
    }

    public Vector3 GetDir()
    {
        _target = _controller.currentObjective;
        return (_target.position - _entity.position).normalized;
    }
}
