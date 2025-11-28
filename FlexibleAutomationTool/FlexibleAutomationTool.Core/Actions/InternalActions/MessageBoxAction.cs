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
            try
            {
                _messageBoxService.Show(Message, Title);
            }
            catch (System.Exception ex)
            {
                throw new System.InvalidOperationException($"Failed to show message box: {ex.Message}", ex);
            }
        }

        public override bool Validate() => !string.IsNullOrWhiteSpace(Message);
    }
}
