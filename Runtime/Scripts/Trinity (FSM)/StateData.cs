using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SLIDDES.StateMachines.Trinity
{
    /// <summary>
    /// Contains data about a state
    /// </summary>
    [System.Serializable]
    public class StateData
    {
        [Tooltip("The name of the state")]
        public string stateName;
        [MonoScript]
        [Tooltip("A reference to the state script.cs Type.fullname")]
        public string stateScriptFullName;

        public UnityEvent onEnter;
        public UnityEvent onUpdate;
        public UnityEvent onExit;
    }
}
