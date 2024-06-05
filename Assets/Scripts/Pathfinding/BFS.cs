using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class BFS
{
    public static List<T> Run<T>(T start, Func<T, List<T>> getConnections, Func<T, bool> isSatisfies, int watchDog = 200) 
    {
        Queue<T> pending = new Queue<T>();
        
        HashSet<T> visited = new HashSet<T>();  

        Dictionary<T, T> parents = new Dictionary<T, T>(); 

        pending.Enqueue(start);

        while(pending.Count > 0)
        {
            watchDog--;

            if(watchDog <= 0) break;

            T current = pending.Dequeue();

            if(isSatisfies(current))
            {
                var path = new List<T>();

                path.Add(current);

                while(parents.ContainsKey(path[path.Count - 1]))
                {
                    path.Add(parents[path[path.Count - 1]]);
                }

                path.Reverse();

                return path;
            }

            visited.Add(current);

            List<T> connections = getConnections(current);

            for(int i = 0; i < connections.Count; i++)
            {
                T child = connections[i]; 

                if(visited.Contains(child))
                    continue;

                pending.Enqueue(child);

                parents[child] = current;

            } 
        }

        return new List<T>();
    }
}
