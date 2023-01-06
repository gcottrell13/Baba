using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Core.Utils
{
    public class StateMachine<TState, TAction> : IDisposable
        where TState : notnull
        where TAction : notnull, new()
    {
        private readonly Dictionary<TState, StateDefinition<TState, TAction>> states = new();
        private readonly string name;
        private readonly TState noOpState;
        private StateDefinition<TState, TAction>? currentState;

        public TState CurrentState => currentState == null ? default! : currentState.State;

        public StateMachine(string name, TState noOpState)
        {
            this.name = name;
            this.noOpState = noOpState;
        }

        public StateMachine<TState, TAction> State(TState state, Func<TAction, TState> theSwitch, Action<StateDefinition<TState, TAction>>? stateConfig = null)
        {
            var stateDef = new StateDefinition<TState, TAction>(state, theSwitch);
            states.Add(state, stateDef);
            stateConfig?.Invoke(stateDef);
            return this;
        }

        public TState SendAction(TAction action)
        {
            if (currentState == null) throw new Exception("State machine was not initialized");

            var resultState = currentState.OnAction(action);

            if (Equals(resultState, noOpState))
                return resultState;

            var isDifferent = !Equals(resultState, currentState.State);
            if (isDifferent)
            {
                currentState.OnLeave(resultState);

                if (states.TryGetValue(resultState, out var newState))
                {
                    newState.OnEnter(currentState.State);
                    currentState = newState;
                }
                else
                {
                    currentState = null;
                }
            }

            return resultState;
        }

        public void Initialize(TState state)
        {
            currentState ??= states.TryGetValue(state, out var newState) ? newState : throw new ArgumentException($"State {state} does not exist");
            currentState?.OnEnter(default!);
        }

        public void Dispose()
        {
            currentState?.OnLeave(default!);
        }
    }


    public class StateDefinition<TState, TAction>
        where TState : notnull
        where TAction : notnull, new()
    {
        public TState State { get; }
        public Func<TAction, TState> TheSwitch { get; }

        public delegate void EventHandler(object sender, TState state);
        private event EventHandler? OnEnterEvent;
        private event EventHandler? OnLeaveEvent;

        public StateDefinition(TState state, Func<TAction, TState> theSwitch)
        {
            State = state;
            TheSwitch = theSwitch;
        }

        public StateDefinition<TState, TAction> AddOnEnter<T>(Func<TState, T> action)
        {
            OnEnterEvent += (o, c) => action(c);
            return this;
        }

        public StateDefinition<TState, TAction> AddOnEnter<T>(Func<T> action)
        {
            OnEnterEvent += (o, c) => action();
            return this;
        }

        public StateDefinition<TState, TAction> AddOnEnter(Action<TState> action)
        {
            OnEnterEvent += (o, c) => action(c);
            return this;
        }

        public StateDefinition<TState, TAction> AddOnEnter(Action action)
        {
            OnEnterEvent += (o, c) => action();
            return this;
        }

        public StateDefinition<TState, TAction> AddOnLeave<T>(Func<TState, T> action)
        {
            OnLeaveEvent += (o, c) => action(c);
            return this;
        }

        public StateDefinition<TState, TAction> AddOnLeave<T>(Func<T> action)
        {
            OnLeaveEvent += (o, c) => action();
            return this;
        }
        public StateDefinition<TState, TAction> AddOnLeave(Action<TState> action)
        {
            OnLeaveEvent += (o, c) => action(c);
            return this;
        }

        public StateDefinition<TState, TAction> AddOnLeave(Action action)
        {
            OnLeaveEvent += (o, c) => action();
            return this;
        }

        public void OnEnter(TState previous)
        {
            OnEnterEvent?.Invoke(this, previous);
        }

        public void OnLeave(TState nextState)
        {
            OnLeaveEvent?.Invoke(this, nextState);
        }

        public TState OnAction(TAction value)
        {
            var newState = TheSwitch(value);
            return newState ?? State;
        }
    }
}
