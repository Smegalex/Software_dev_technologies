using FlexibleAutomationTool.Core.Interfaces;

namespace FlexibleAutomationTool.Core.Actions.InternalActions
{
    public class MessageBoxAction : ActionBase
    {
        public string Message { get; set; } = "";
        public string Title { get; set; } = "";

        private readonly IMessageBoxService _messageBoxService;

        public MessageBoxAction(IMessageBoxService service)
        {
            _messageBoxService = service;
        }

        public override void Execute()
        {
            _messageBoxService.Show(Message, Title);
        }

        public override bool Validate() => !string.IsNullOrWhiteSpace(Message);
    }
}
