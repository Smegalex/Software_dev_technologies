using FlexibleAutomationTool.Core.Factories;
using FlexibleAutomationTool.Core.Interfaces;
using FlexibleAutomationTool.UI.Services;
using System;
using FlexibleAutomationTool.Core.Services;

namespace FlexibleAutomationTool.UI.Services
{
    public class WinFormsPlatformFactory : IPlatformFactory
    {
        private readonly IMessageBoxService _msgService;
        private readonly ServiceManager _svcManager;

        public WinFormsPlatformFactory(IMessageBoxService msgService, ServiceManager svcManager)
        {
            _msgService = msgService;
            _svcManager = svcManager;

            // register in service manager for UI discovery
            _svcManager.Register(nameof(IMessageBoxService), _msgService);
        }

        public IMessageBoxService CreateMessageBoxService() => _msgService;
    }
}
