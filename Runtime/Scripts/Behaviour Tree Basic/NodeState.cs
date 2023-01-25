using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.StateMachines.BehaviourTreeBasic
{
    /// <summary>
    /// The current state of a node
    /// </summary>
    public enum NodeState
    {
        running,
        success,
        failure
    }
}
