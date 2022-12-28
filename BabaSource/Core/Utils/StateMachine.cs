using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utils
{
    public class StateMachine<TState, Transition>
        where TState : notnull
        where Transition : notnull
    {
        private readonly Dictionary<TState, StateDefinition<TState, Transition>> states = new();
        private StateDefinition<TState, Transition>? currentState;

        public TState CurrentState => currentState == null ? default : currentState.State;

        public StateMachine()
        {
        }

        public StateMachine<TState, Transition> State(TState state, Func<object, Transition> theSwitch, Action<StateDefinition<TState, Transition>>? stateConfig = null)
        {
            var stateDef = new StateDefinition<TState, Transition>(state, theSwitch);
            states.Add(state, stateDef);
            stateConfig?.Invoke(stateDef);
            return this;
        }

        public TState SendAction(object action)
        {
            if (currentState == null) throw new Exception("State machine was not initialized");

            if (states.TryGetValue(currentState.OnAction(action, out var trans), out var newState) && !Equals(newState, currentState))
            {
                newState.OnEnter(currentState.State, trans);
                currentState.OnLeave(newState.State, trans);
                currentState = newState;
                return newState.State;
            }

            return default;
        }

        public void Initialize(TState state)
        {
            currentState ??= states.TryGetValue(state, out var newState) ? newState : throw new ArgumentException($"State {state} does not exist");
            currentState?.OnEnter(default, default);
        }
    }


    public class StateDefinition<TState, Transition>
        where TState : notnull
        where Transition : notnull
    {
        public TState State { get; }
        public Func<object, Transition> TheSwitch { get; }

        public delegate void EventHandler(object sender, TState state, Transition trans);
        private event EventHandler? OnEnterEvent;
        private event EventHandler? OnLeaveEvent;
        private readonly Dictionary<Transition, TState> transitions = new();

        public StateDefinition(TState state, Func<object, Transition> theSwitch)
        {
            State = state;
            TheSwitch = theSwitch;
        }

        public StateDefinition<TState, Transition> AddOnEnter<T>(Func<TState, Transition, T> action)
        {
            OnEnterEvent += (o, c, trans) => action(c, trans);
            return this;
        }

        public StateDefinition<TState, Transition> AddOnEnter<T>(Func<T> action)
        {
            OnEnterEvent += (o, c, trans) => action();
            return this;
        }

        public StateDefinition<TState, Transition> AddOnEnter(Action<TState, Transition> action)
        {
            OnEnterEvent += (o, c, trans) => action(c, trans);
            return this;
        }

        public StateDefinition<TState, Transition> AddOnEnter(Action action)
        {
            OnEnterEvent += (o, c, trans) => action();
            return this;
        }

        public StateDefinition<TState, Transition> AddOnLeave<T>(Func<TState, Transition, T> action)
        {
            OnLeaveEvent += (o, c, trans) => action(c, trans);
            return this;
        }

        public StateDefinition<TState, Transition> AddOnLeave<T>(Func<T> action)
        {
            OnLeaveEvent += (o, c, trans) => action();
            return this;
        }
        public StateDefinition<TState, Transition> AddOnLeave(Action<TState, Transition> action)
        {
            OnLeaveEvent += (o, c, trans) => action(c, trans);
            return this;
        }

        public StateDefinition<TState, Transition> AddOnLeave(Action action)
        {
            OnLeaveEvent += (o, c, trans) => action();
            return this;
        }

        public void OnEnter(TState previous, Transition t)
        {
            OnEnterEvent?.Invoke(this, previous, t);
        }

        public void OnLeave(TState nextState, Transition t)
        {
            OnLeaveEvent?.Invoke(this, nextState, t);
        }

        public StateDefinition<TState, Transition> Change(Transition transition, TState newState)
        {
            transitions.Add(transition, newState);
            return this;
        }

        public TState OnAction(object value, out Transition transition)
        {
            transition = TheSwitch(value);
            if (transition == null) return State;
            return transitions.TryGetValue(transition, out var newState) ? newState : State;
        }
    }
}
