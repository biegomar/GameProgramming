
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FiniteStateMachine.EventArgs;

namespace FiniteStateMachine
{
    public class FiniteStateMachine
    {
        public IList<State> States { get; set; } = new List<State>();
        public State ActiveState { get; set; }

        private bool isMachineStarted = false;

        public FiniteStateMachine(State activeState)
        {
            this.ActiveState = activeState;
            this.States.Add(activeState);
        }

        public void AddState(State state)
        {
            if (!this.States.Contains(state))
            {
                this.States.Add(state);
            }
        }

        public void StartMachine(EnterEventArgs? eventArgs = null)
        {
            this.ActiveState.OnEnter(eventArgs ?? new EnterEventArgs());

            this.isMachineStarted = true;
        }

        public void UpdateMachine()
        {
            if (!this.isMachineStarted)
            {
                this.StartMachine();
                return;
            }
            
            this.ActiveState.OnUpdate(new UpdateEventArgs());

            foreach (var transition in this.ActiveState.Transitions)
            {
                if (transition.Evaluate())
                {
                    this.ActiveState.OnExit(new ExitEventArgs());

                    this.ActiveState = transition.ToState;
                    this.ActiveState.OnEnter(new EnterEventArgs());
                    
                    break;
                }
            }
        }
    }
}