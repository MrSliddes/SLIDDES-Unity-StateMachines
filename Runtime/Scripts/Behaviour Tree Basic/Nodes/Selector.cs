using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.StateMachines.BehaviourTreeBasic
{
    /// <summary>
    /// Think of an OR logic gate.
    /// </summary>
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }


        public override NodeState Evaluate()
        {
            // Return early when a child has succeeded or is running
            foreach(Node node in children)
            {
                switch(node.Evaluate())
                {
                    case NodeState.running:
                        return state = NodeState.running;
                    case NodeState.success:
                        return state = NodeState.success;
                    case NodeState.failure:
                        continue;
                    default:
                        break;
                }
            }

            return state = NodeState.failure;
        }
    }
}
