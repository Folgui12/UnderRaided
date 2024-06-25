using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSeek : ISteering
{
    Transform _target;
    Transform _entity;

    public MinionSeek(Transform target, Transform entity)
    {
        _target = target;
        _entity = entity;
    }

    public Vector3 GetDir()
    {
        return (_target.position - _entity.position).normalized;
    }
}
