using System;

namespace FlexibleAutomationTool.Core.Triggers
{
    // ManualTrigger is a semantic specialization of EventTrigger used for manual activations.
    // It has no extra data or condition by default — calling RaiseEvent() will cause ShouldExecute() to return true once.
    public class ManualTrigger : EventTrigger
    {
        public ManualTrigger() : base(null)
        {
        }

        public override bool Validate() => true;
    }
}
