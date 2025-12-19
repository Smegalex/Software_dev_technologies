using System;

namespace FlexibleAutomationTool.Core.Triggers
{
    public class EventTrigger : Trigger
    {
        private volatile bool _eventRaised;
        private object? _lastEventData;
        private readonly object _lock = new();

        // Condition receives the last event data and returns true if the trigger should fire.
        // If null, any raised event will cause execution.
        public Func<object?, bool>? Condition { get; }

        public EventTrigger(Func<object?, bool>? condition = null)
        {
            Condition = condition;
        }

        // External code calls this to notify the trigger of an event occurrence.
        public void RaiseEvent(object? eventData = null)
        {
            lock (_lock)
            {
                _lastEventData = eventData;
                _eventRaised = true;
            }
        }

        public override bool ShouldExecute()
        {
            if (!_eventRaised)
                return false;

            object? data;
            lock (_lock)
            {
                data = _lastEventData;
                _eventRaised = false;
                _lastEventData = null;
            }

            try
            {
                return Condition == null || Condition(data);
            }
            catch
            {
                // If condition fails, do not execute the action.
                return false;
            }
        }

        public override bool Validate() => true;
    }
}
