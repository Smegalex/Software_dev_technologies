namespace FlexibleAutomationTool.Core.Actions
{
    public class ServiceAction : ActionBase
    {
        public string ServiceType { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string? Parameters { get; set; }

        public override void Execute()
        {
            Console.WriteLine($"[ServiceAction] Service={ServiceType} Command={Command} Params={Parameters}");
        }

        public override bool Validate() => !string.IsNullOrEmpty(ServiceType) && !string.IsNullOrEmpty(Command);
    }
}
