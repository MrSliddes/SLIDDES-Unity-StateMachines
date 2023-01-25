using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.StateMachines.Trinity
{
    /// <summary>
    /// A substate can be active or inactive independent of a state
    /// </summary>
    public class SubState : State
    {
        /// <summary>
        /// Is the sub state active?
        /// </summary>
        public bool isActive;

        public SubState(StateMachine baseStateMachine) : base(baseStateMachine)
        {
        }
    }
}
