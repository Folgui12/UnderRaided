using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState<T> : State<T>
{
    private ISteering _steering;
    private BaseEnemyModel _model;
    private BaseEnemyView _view;
    private BaseEnemyController _controller;
    private ObstacleAvoidance _obs;

    private bool _isFinishPath;
    List<Vector3> _waypoints;
    int _nextPoint = 0;


    public EnemyPatrolState(ISteering steering, BaseEnemyModel model, BaseEnemyView view, BaseEnemyController controller, ObstacleAvoidance obs)
    {
        _steering = steering;
        _model = model;
        _view = view;
        _controller = controller; 
        _obs = obs;
    }


    // Comienzo la animación de caminar y reseteo el tiempo de Idle para la siguiente ocasión
    public override void Enter()
    {
        base.Enter();

        _view.StartWalking();
        
        _model.ResetIdleTimer();

        _controller.RunAStarPlus();
    }


    // Update is called once per frame
    public override void Execute()
    {
        Run();
    }

    public void SetWayPoints(List<Node> newPoints)
    {
        var list = new List<Vector3>();
        for (int i = 0; i < newPoints.Count; i++)
        {
            list.Add(newPoints[i].transform.position);
        }
        SetWayPoints(list);
    }

    public void SetWayPoints(List<Vector3> newPoints)
    {
        _nextPoint = 0;
        if (newPoints.Count == 0) return;
        //_anim.Play("CIA_Idle");
        _waypoints = newPoints;
        var pos = _waypoints[_nextPoint];
        pos.y = _model.transform.position.y;
        _model.SetPosition(pos);
        _isFinishPath = false;
    }

    void Run()
    {
        if (IsFinishPath) return;
        var point = _waypoints[_nextPoint];
        var posPoint = point;
        posPoint.y = _model.transform.position.y;
        Vector3 dir = posPoint - _model.transform.position;
        if (dir.magnitude < 0.2f)
        {
            if (_nextPoint + 1 < _waypoints.Count)
                _nextPoint++;
            else
            {
                _isFinishPath = true;
                return;
            }
        }

        //var dir = _obs.GetDir(_steering.GetDir(), false);

        _model.Move(dir.normalized);
        _view.LookDir(dir);
    }

    public bool IsFinishPath => _isFinishPath;
}
