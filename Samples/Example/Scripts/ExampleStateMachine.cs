using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLIDDES.StateMachines.Trinity;

namespace SLIDDES.StateMachines.Samples
{
    public class ExampleStateMachine : StateMachine
    {
        public ExampleStateMachine(GameObject scriptThatStateMachineIsOn) : base()
        {
            State s = new ExampleStateZero(this);
            states.Add("Zero", s);
            states["Zero"].onEnter += () => { };
        }
    }
}
