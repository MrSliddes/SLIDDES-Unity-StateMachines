using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SLIDDES.StateMachines.Trinity
{
    /// <summary>
    /// A basic state machine with an enter(), update(), exit()
    /// </summary>
    public class StateMachine : MonoBehaviour
    {
        /// <summary>
        /// The current active state of the state machine
        /// </summary>
        public State CurrentState => currentState;
        /// <summary>
        /// The next in line state to be enterd
        /// </summary>
        public State NextState => nextState;

        public Values values;
        public Components components;
                
        /// <summary>
        /// The states of the statemachine
        /// </summary>
        public Dictionary<string, State> states = new Dictionary<string, State>();

        private readonly string debugPrefix = "[StateMachine]";
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

        /// <summary>
        /// Initialize the statemachine
        /// </summary>
        /// <param name="stateMachineUser">The gameobject that uses this stateMachine</param>
        public void Initialize(GameObject stateMachineUser)
        {
            // Set stateMachineUser
            if(stateMachineUser == null) stateMachineUser = gameObject;
            components.stateMachineUser = stateMachineUser;

            // Create each state and add it to states
            State initializedCurrentState = null;
            for(int i = 0; i < values.stateDatas.Count; i++)
            {
                // Get the type of a state through its stateName
                string stateName = values.stateDatas[i].stateName;
                if(string.IsNullOrEmpty(stateName)) continue;

                if(values.showDebug) Debug.Log($"{debugPrefix} Get type for {stateName}");

                Type type = TypeExtentions.GetType(stateName);
                if(type == null)
                {
                    if(values.showDebug) Debug.Log($"{debugPrefix} No type found for {stateName}");
                    continue;
                }

                if(values.showDebug) Debug.Log($"{debugPrefix} Got type {type.FullName}");
                State state = (State)Activator.CreateInstance(type);
                if(state != null)
                {
                    // Add state
                    if(values.showDebug) Debug.Log($"{debugPrefix} Add state {state}");
                    states.Add(stateName, state);
                    // Link state callbacks
                    state.onEnter += values.stateDatas[i].onEnter.Invoke;
                    state.onUpdate += values.stateDatas[i].onUpdate.Invoke;
                    state.onExit += values.stateDatas[i].onExit.Invoke;
                    // Initialize
                    state.Initialize(this);

                    // Set starting state if index matches
                    if(i == values.stateStartIndex) initializedCurrentState = state;
                }
            }

            // Set initialized currentState
            if(initializedCurrentState != null) NewState(initializedCurrentState);
        }

        public StateMachine()
        {

        }

        public void Start()
        {
            if(values.initializeOnStart) Initialize(components.stateMachineUser);
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

            if(currentState != null) values.currentStateName = currentState.ToString();
        }

        /// <summary>
        /// Transition to a new state
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="overrideAllowExit"></param>
        public void NewState(string stateName)
        {
            if(!states.ContainsKey(stateName))
            {
                Debug.LogError($"{debugPrefix} Tried entering new state but '{stateName}' cannot be found. (Did you forget to add it to the statemachine?)");
                return;
            }
            NewState(states[stateName], false);
        }
                
        /// <summary>
        /// Transition to a new state
        /// </summary>
        /// <param name="newState">The new state to enter</param>
        /// <param name="overrideAllowExit">If the current active state doesnt allow an exit override it if you need to force the exit</param>
        public void NewState(State newState, bool overrideAllowExit = false)
        {
            if(statesChangedThisFrame >= values.maxStateChangesPerFrame)
            {
                Debug.LogWarning($"{debugPrefix} Maximum states change amount has been reached this frame. NewState() will not enter a new state to prevent a memory leak. Your states are causing a loop which causes a memory leak in 1 frame. Example: State.A is going to State.B but state B is immiditly going back to A and a go's again back to B infinitly. You can increase the stateMachine value 'maxStateChangesPerFrame' to circumvent this at your own risk.");
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

        /// <summary>
        /// Force statemachine to transition to a new state
        /// </summary>
        /// <param name="stateName"></param>
        public void NewStateForce(string stateName)
        {
            if(!states.ContainsKey(stateName))
            {
                Debug.LogError($"{debugPrefix} Tried entering new state but '{stateName}' cannot be found. (Did you forget to add it to the statemachine?)");
                return;
            }
            NewState(states[stateName], true);
        }

        [System.Serializable]
        public class Components
        {
            [Tooltip("The gameobject that is linked to this statemachine")]
            public GameObject stateMachineUser;
        }

        [System.Serializable]
        public class Values
        {
            [Tooltip("The state that will be set as currentState on initialize")]
            public int stateStartIndex;
            [Tooltip("List containing each state script data for this stateMachine")]
            public List<StateData> stateDatas;
            [Space]

            [Tooltip("Initialize the statemachine on start")]
            public bool initializeOnStart = true;
            [Tooltip("The maximum amount of times the state machine can change a state per frame (prevents memory leaks)")]
            public int maxStateChangesPerFrame = 99;

            [Header("Ref Values")]
            public string currentStateName;

            [Header("Debug")]
            public bool showDebug;
        }
    }
}
