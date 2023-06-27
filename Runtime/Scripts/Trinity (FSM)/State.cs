using System;
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
        /// Action that gets triggerd on OnEnter()
        /// </summary>
        public Action onEnter;
        /// <summary>
        /// Action that gets triggerd on OnUpdate()
        /// </summary>
        public Action onUpdate;
        /// <summary>
        /// Action that gets triggerd on OnExit()
        /// </summary>
        public Action onExit;

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
            onEnter.Invoke();
        }

        /// <summary>
        /// When the state gets updated
        /// </summary>
        /// <remarks>Do not call this within the state!</remarks>
        public virtual void OnUpdate()
        {
            onUpdate.Invoke();
        }

        /// <summary>
        /// When the state gets exited
        /// </summary>
        /// <remarks>Do not call this within the state!</remarks>
        public virtual void OnExit()
        {
            onExit.Invoke();
        }
    }
}
