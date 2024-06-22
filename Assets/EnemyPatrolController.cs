using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolController : MonoBehaviour
{
    public BaseEnemyController enemy;
    public float radius = 3;
    public LayerMask maskNodes;
    public LayerMask maskObs;
    public Transform CurrentObjective;
    private Node target;
    public List<Node> DeadEndsNodes;

    private int DeadEndsIndex = 0;
    
    public void RunAStarPlus()
    {
        GetNextNodeTarget();
        var start = GetNearNode(enemy.transform.position);
        if (start == null) return;
        List<Node> path = AStar.Run(start, GetConnections, IsSatiesfies, GetCost, Heuristic);
        path = AStar.CleanPath(path, InView);
        enemy.GetStateWaypoints.SetWayPoints(path);
    }

    bool InView(Node grandParent, Node child)
    {
        Debug.Log("RAY");
        return InView(grandParent.transform.position, child.transform.position);
    }

    bool InView(Vector3 a, Vector3 b)
    {
        //a->b  b-a
        Vector3 dir = b - a;
        return !Physics.Raycast(a, dir.normalized, dir.magnitude, maskObs);
    }

    float Heuristic(Node current)
    {
        float heuristic = 0;
        float multiplierDistance = 1;
        heuristic += Vector3.Distance(current.transform.position, target.transform.position) * multiplierDistance;
        return heuristic;
    }

    float GetCost(Node parent, Node child)
    {
        float cost = 0;
        float multiplierDistance = 1;
        float multiplierTrap = 200;
        cost += Vector3.Distance(parent.transform.position, child.transform.position) * multiplierDistance;
        if (child.alreadyVisited)
        {
            cost += multiplierTrap;
        }
        return cost;
    }

    void GetNextNodeTarget()
    {
        CurrentObjective = DeadEndsNodes[DeadEndsIndex].transform;
        target = DeadEndsNodes[DeadEndsIndex];
        DeadEndsIndex++;
        if(DeadEndsIndex == DeadEndsNodes.Count)
        {
            DeadEndsIndex = 0; 
        }
    } 

    Node GetNearNode(Vector3 pos)
    {
        var nodes = Physics.OverlapSphere(pos, radius, maskNodes);
        Node nearNode = null;
        float nearDistance = 0;
        for (int i = 0; i < nodes.Length; i++)
        {
            var currentNode = nodes[i];
            var dir = currentNode.transform.position - pos;
            float currentDistance = dir.magnitude;
            if (nearNode == null || currentDistance < nearDistance)
            {
                if (!Physics.Raycast(pos, dir.normalized, currentDistance, maskObs))
                {
                    nearNode = currentNode.GetComponent<Node>();
                    nearDistance = currentDistance;
                }
            }
        }
        return nearNode;
    }

    List<Node> GetConnections(Node current)
    {
        return current.neightbourds;
    }

    bool IsSatiesfies(Node current)
    {
        return current == target;
    }

    bool IsSatiesfies(Vector3 current)
    {
        return Vector3.Distance(current, CurrentObjective.transform.position) < 2 && InView(current, CurrentObjective.transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(enemy.transform.position, radius);
    }
}
