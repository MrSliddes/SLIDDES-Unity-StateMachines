using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.StateMachines.BehaviourTreeBasic
{
    public class Node
    {
        /// <summary>
        /// The parent node
        /// </summary>
        public Node parent;
        /// <summary>
        /// Current state of the node
        /// </summary>
        protected NodeState state;
        /// <summary>
        /// The children of this node
        /// </summary>
        protected List<Node> children = new List<Node>();

        private Dictionary<string, object> dataContext = new Dictionary<string, object>();


        public Node()
        {

        }

        public Node(List<Node> children)
        {
            foreach(var child in children)
            {
                AttachChild(child);
            }
        }


        public virtual NodeState Evaluate() => NodeState.failure;


        /// <summary>
        /// Clear data from dataContext
        /// </summary>
        /// <param name="key"></param>
        /// <returns>True if it cleared the data, false if it doesnt exist</returns>
        public bool ClearData(string key)
        {
            if(dataContext.ContainsKey(key))
            {
                dataContext.Remove(key);
                return true;
            }

            // Get recursive
            Node node = parent;
            while(node != null)
            {
                if(node.ClearData(key)) return true;
                node = node.parent;
            }

            return false;
        }

        /// <summary>
        /// Get data from dataContext
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetData(string key)
        {
            object value = null;
            if(dataContext.TryGetValue(key, out value)) return value;

            // Get recursive
            Node node = parent;
            while(node != null)
            {
                value = node.GetData(key);
                if(value != null) return value;
                node = node.parent;
            }

            // Not found
            return null;
        }

        /// <summary>
        /// Set data in dataContext
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetData(string key, object value)
        {
            if(dataContext.ContainsKey(key))
            {
                dataContext[key] = value;
            }
        }

        /// <summary>
        /// Attach a child node to this node
        /// </summary>
        /// <param name="node"></param>
        private void AttachChild(Node node)
        {
            node.parent = this;
            children.Add(node);
        }
    }
}
