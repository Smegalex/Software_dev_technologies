using FlexibleAutomationTool.Core.Factories;
using FlexibleAutomationTool.Core.Interfaces;
using FlexibleAutomationTool.Core.Services;
using FlexibleAutomationTool.UI.Services;

namespace FlexibleAutomationTool.UI.Services
{
    public class WinFormsPlatformFactory : IPlatformFactory
    {
        private readonly IMessageBoxService _msgService;
        private readonly IClipboardService _clipboard;
        private readonly IFilePickerService _filePicker;
        private readonly ITrayIconService _trayIcon;
        private readonly ServiceManager _svcManager;

        public WinFormsPlatformFactory(IMessageBoxService msgService, ServiceManager svcManager)
        {
            _msgService = msgService;
            _svcManager = svcManager;

            _clipboard = new WinFormsClipboardService();
            _filePicker = new WinFormsFilePickerService();
            _trayIcon = new WinFormsTrayIconService();

            _svcManager.Register(nameof(IMessageBoxService), _msgService);
            _svcManager.Register(nameof(IClipboardService), _clipboard);
            _svcManager.Register(nameof(IFilePickerService), _filePicker);
            _svcManager.Register(nameof(ITrayIconService), _trayIcon);
        }

        public IMessageBoxService CreateMessageBoxService() => _msgService;
        public IClipboardService CreateClipboardService() => _clipboard;
        public IFilePickerService CreateFilePickerService() => _filePicker;
        public ITrayIconService CreateTrayIconService() => _trayIcon;
    }
}
