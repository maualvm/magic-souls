using System;
using System.Collections.Generic;

public class StateMachine
{
    private IState CurrentState;

    private Dictionary<Type, List<Transition>> Transitions = new Dictionary<Type, List<Transition>>();
    private List<Transition> CurrentTransitions = new List<Transition>();
    private List<Transition> AnyTransitions = new List<Transition>();

    private static List<Transition> EmptyTransitions = new List<Transition>(0);

    public void Update()
    {
        var Transition = GetTransition();
        if (Transition != null)
            SetState(Transition.To);

        CurrentState?.Update();
    }

    public void SetState(IState state)
    {
        if (state == CurrentState)
            return;

        CurrentState?.OnExit();
        CurrentState = state;

        Transitions.TryGetValue(CurrentState.GetType(), out CurrentTransitions);
        if (CurrentTransitions == null)
            CurrentTransitions = EmptyTransitions;

        CurrentState.OnEnter();
    }

    public void AddTransition(IState from, IState to, Func<bool> predicate)
    {
        if(Transitions.TryGetValue(from.GetType(), out var _transitions) == false)
        {
            _transitions = new List<Transition>();
            Transitions[from.GetType()] = _transitions;
        }

        _transitions.Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(IState state, Func<bool> predicate)
    {
        AnyTransitions.Add(new Transition(state, predicate));
    }

    private class Transition
    {
        public Func<bool> Condition { get; }
        public IState To { get; }

        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    private Transition GetTransition()
    {
        foreach (var transition in AnyTransitions)
            if (transition.Condition())
                return transition;

        foreach (var transition in CurrentTransitions)
            if (transition.Condition())
                return transition;

        return null;
    }
}


