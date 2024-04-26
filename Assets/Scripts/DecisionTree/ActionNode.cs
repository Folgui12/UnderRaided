using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionNode : ITreeNode
{
    private Action _action;

    public ActionNode(Action action)
    {
        _action = action;
    }

    public void Execute()
    {
        _action();
    }
}
