using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState<T> : State<T>
{
    private BaseEnemyModel _model;
    private BaseEnemyView _view;
    private BaseEnemyController _controller; 
    private EnemyPatrolController _pController;
    private ObstacleAvoidance _obs;

    private bool _isFinishPath = false;
    List<Vector3> _waypoints;
    int _nextPoint = 0;


    public EnemyPatrolState(BaseEnemyModel model, BaseEnemyView view, BaseEnemyController controller, EnemyPatrolController pController, ObstacleAvoidance obs)
    {
        _model = model;
        _view = view;
        _pController = pController; 
        _controller = controller;
        _obs = obs;
    }


    // Comienzo la animación de caminar y reseteo el tiempo de Idle para la siguiente ocasión
    public override void Enter()
    {
        base.Enter();
        _view.StartWalking();
        _view.ChangeNoiseToWalk();
        _pController.RunAStarPlus();

        foreach (MinionModel minion in _controller.myMinions)
        {
            minion.speed = 1f; 
        }
    }


    // Update is called once per frame
    public override void Execute()
    {
        Run();
        _view.StepNoise();
    }

    public override void Sleep()
    {
        base.Sleep(); 

        
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
        _waypoints = newPoints;
        var pos = _waypoints[_nextPoint];
        pos.y = _model.transform.position.y;
        _isFinishPath = false;
    }

    void Run()
    {
        
        if (IsFinishPath) return; 
        var posPoint = _waypoints[_nextPoint];
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

        dir = _obs.GetDir(dir, false).normalized;

        _model.Move(dir.normalized);
        _view.LookDir(dir);
    }

    public bool IsFinishPath => _isFinishPath;
}
