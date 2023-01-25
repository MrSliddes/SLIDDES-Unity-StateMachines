using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.StateMachines.Trinity
{
    /// <summary>
    /// A basic state machine with an enter(), update(), exit()
    /// </summary>
    public abstract class StateMachine
    {
        /// <summary>
        /// The current active state of the state machine
        /// </summary>
        public State CurrentState => currentState;
        /// <summary>
        /// The next in line state to be enterd
        /// </summary>
        public State NextState => nextState;

        /// <summary>
        /// The maximum amount of times the state machine can change a state per frame (prevents memory leaks)
        /// </summary>
        public int maxStateChangesPerFrame = 99;

        /// <summary>
        /// Counter that keeps track of the amount of states changed this frame
        /// </summary>
        private int statesChangedThisFrame;
        /// <summary>
        /// Internal
        /// </summary>
        /// <see cref="CurrentState"/>
        private State currentState;
        /// <summary>
        /// Internal
        /// </summary>
        /// <see cref="NextState"/>
        private State nextState;

        public StateMachine()
        {

        }

        /// <summary>
        /// Update the state machine and its currentState.Update
        /// </summary>
        public void Update()
        {
            statesChangedThisFrame = 0;

            // Update the currentState
            if(currentState != null) currentState.OnUpdate();

            // Check if it is possible to enter next state
            if(nextState != null)
            {
                if(currentState.allowsExit) NewState(nextState);
            }
        }

        /// <summary>
        /// Transition to a new state
        /// </summary>
        /// <param name="newState">The new state to enter</param>
        /// <param name="overrideAllowExit">If the current active state doesnt allow an exit override it if you need to force the exit</param>
        public void NewState(State newState, bool overrideAllowExit = false)
        {
            if(statesChangedThisFrame >= maxStateChangesPerFrame)
            {
                Debug.LogWarning("[StateMachine] Maximum states change amount has been reached this frame. NewState() will not enter a new state to prevent a memory leak. Your states are causing a loop which causes a memory leak in 1 frame. Example: State.A is going to State.B but state B is immiditly going back to A and a go's again back to B infinitly. You can increase the stateMachine value 'maxStateChangesPerFrame' to circumvent this at your own risk.");
                return;
            }

            // Check for override
            if(overrideAllowExit) currentState.allowsExit = true;

            // Check if current state is done
            if(currentState != null && !currentState.allowsExit)
            {
                // Current state is not done
                nextState = newState;
                return;
            }

            if(currentState != null) currentState.OnExit();
            currentState = newState;
            currentState.OnEnter();
            nextState = null;

            statesChangedThisFrame++;
        }
    }
}
