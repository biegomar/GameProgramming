using System.Collections.Generic;

namespace FiniteStateMachine
{
    public class Transition
    {
        public State ToState { get; private set; }
        
        private EvaluateTransition EvaluateTransition { get; set; }

        public Transition(EvaluateTransition evaluateTransition, State toState)
        {
            this.EvaluateTransition = evaluateTransition;
            this.ToState = toState;
        }

        public bool Evaluate()
        {
            return this.EvaluateTransition.Invoke();
        }
    }
}