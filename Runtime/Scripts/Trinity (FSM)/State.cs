using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.StateMachines.Trinity
{
    /// <summary>
    /// The base class for a state
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// Does this state allow to be exited?
        /// </summary>
        public bool allowsExit = true;
        /// <summary>
        /// The state name in string
        /// </summary>
        protected string name;
        /// <summary>
        /// The baseclass of statemachine
        /// </summary>
        protected StateMachine baseStateMachine;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseStateMachine"></param>
        public State(StateMachine baseStateMachine)
        {
            this.baseStateMachine = baseStateMachine;
            name = GetType().ToString();
        }


        /// <summary>
        /// When the state gets enterd
        /// </summary>
        /// <remarks>Do not call this within the state!</remarks>
        public virtual void OnEnter()
        {
            
        }

        /// <summary>
        /// When the state gets updated
        /// </summary>
        /// <remarks>Do not call this within the state!</remarks>
        public virtual void OnUpdate()
        {

        }

        /// <summary>
        /// When the state gets exited
        /// </summary>
        /// <remarks>Do not call this within the state!</remarks>
        public virtual void OnExit()
        {

        }
    }
}
