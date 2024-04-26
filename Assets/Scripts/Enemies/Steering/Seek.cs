using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : ISteering
{
    Transform _entity;
    Transform _target;
    BaseEnemyModel _model;
    public Seek(BaseEnemyModel model, Transform entity, Transform target)
    {
        _entity = entity;
        _target = target;
        _model = model;
    }

    public Vector3 GetDir()
    {
        _target = _model.currentObjective;
        return (_target.position - _entity.position).normalized;
    }
}
