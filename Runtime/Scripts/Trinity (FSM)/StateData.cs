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
        [MonoScript]
        public string stateName;

        public UnityEvent onEnter;
        public UnityEvent onUpdate;
        public UnityEvent onExit;
    }
}
