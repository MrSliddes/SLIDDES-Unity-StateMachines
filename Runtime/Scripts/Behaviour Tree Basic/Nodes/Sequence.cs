using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.StateMachines.BehaviourTreeBasic
{
    /// <summary>
    /// Think of an AND logic gate. Only if all child nodes succeed will this node succeed
    /// </summary>
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        /// <summary>
        /// Only if all children are a success will this node return success
        /// </summary>
        /// <returns></returns>
        public override NodeState Evaluate()
        {
            // Check if a child node is still running
            bool anyChildIsRunning = false;

            // Loop through all children and check their state
            foreach(Node node in children)
            {
                switch(node.Evaluate())
                {
                    case NodeState.running:
                        anyChildIsRunning = true;
                        continue;
                    case NodeState.success:
                        continue;
                    case NodeState.failure:
                        state = NodeState.failure;
                        return state;
                    default:
                        break;
                }
            }

            state = anyChildIsRunning ? NodeState.running : NodeState.success;
            return state;
        }
    }
}
