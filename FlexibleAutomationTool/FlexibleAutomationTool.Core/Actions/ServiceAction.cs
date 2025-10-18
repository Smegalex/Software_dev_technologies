using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexibleAutomationTool.Core.Actions
{
    public class ServiceAction : Action
    {
        public string ServiceType { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string? Parameters { get; set; }

        public override void Execute()
        {
            // placeholder: call ServiceManager in real app
            Console.WriteLine($"[ServiceAction] Service={ServiceType} Command={Command} Params={Parameters}");
        }

        public override bool Validate() => !string.IsNullOrEmpty(ServiceType) && !string.IsNullOrEmpty(Command);
    }
}
