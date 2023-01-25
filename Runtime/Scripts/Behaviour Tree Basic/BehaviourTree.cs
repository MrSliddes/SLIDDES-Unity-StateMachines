using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.StateMachines.BehaviourTreeBasic
{
    public abstract class BehaviourTree : MonoBehaviour
    {
        /// <summary>
        /// The root node of the tree
        /// </summary>
        private Node root;

        // Start is called before the first frame update
        void Start()
        {
            root = SetupTree();
        }

        // Update is called once per frame
        void Update()
        {
            // Update the tree
            if(root != null) { root.Evaluate(); }
        }

        protected abstract Node SetupTree();
    }
}
