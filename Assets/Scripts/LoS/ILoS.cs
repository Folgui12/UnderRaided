using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoS
{
    bool CheckViewRange(Transform target);
    bool CheckAttackRange(Transform target);
    bool CheckView(Transform target);
    bool CheckView(Transform target, LayerMask maskObs);
}
