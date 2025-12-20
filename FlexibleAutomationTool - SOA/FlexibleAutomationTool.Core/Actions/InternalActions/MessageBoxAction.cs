using FlexibleAutomationTool.Core.Interfaces;
using System.ComponentModel;

namespace FlexibleAutomationTool.Core.Actions.InternalActions
{
    public class MessageBoxAction : ActionBase
    {
        public string Message { get; set; } = "";
        public string Title { get; set; } = "";

        [Browsable(false)]
        public IMessageBoxService? MessageBoxService { get; set; }

        public MessageBoxAction()
        {
        }

        public MessageBoxAction(IMessageBoxService service)
        {
            MessageBoxService = service;
        }

        public override void Execute()
        {
            try
            {
                if (MessageBoxService == null)
                    throw new System.InvalidOperationException("No IMessageBoxService available to show message. Set MessageBoxService before executing this action.");

                MessageBoxService.Show(Message, Title);
            }
            catch (System.Exception ex)
            {
                throw new System.InvalidOperationException($"Failed to show message box: {ex.Message}", ex);
            }
        }

        public override bool Validate() => !string.IsNullOrWhiteSpace(Message);
    }
}
