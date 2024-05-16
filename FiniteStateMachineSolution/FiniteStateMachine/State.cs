using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FiniteStateMachine.EventArgs;

namespace FiniteStateMachine
{
    public class State
    {
        public Guid Id { get; } = Guid.NewGuid();
        private IList<Transition> transitions { get; set; } = new List<Transition>();

        public ReadOnlyCollection<Transition> Transitions => new ReadOnlyCollection<Transition>(this.transitions);

        public event EventHandler<EnterEventArgs>? Enter;
        public event EventHandler<ExitEventArgs>? Exit;
        public event EventHandler<UpdateEventArgs>? Update;

        public virtual void OnEnter(EnterEventArgs e)
        {
            Enter?.Invoke(this, e);
        }

        public virtual void OnExit(ExitEventArgs e)
        {
            Exit?.Invoke(this, e);
        }

        public virtual void OnUpdate(UpdateEventArgs e)
        {
            Update?.Invoke(this, e);
        }

        public void AddTransition(Transition transition)
        {
            if (!this.transitions.Contains(transition))
            {
                this.transitions.Add(transition);    
            }
        }
    }
}