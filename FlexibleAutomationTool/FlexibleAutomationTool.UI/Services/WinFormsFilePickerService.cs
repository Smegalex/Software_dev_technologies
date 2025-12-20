using FlexibleAutomationTool.Core.Interfaces;
using System.Windows.Forms;

namespace FlexibleAutomationTool.UI.Services
{
    public class WinFormsFilePickerService : IFilePickerService
    {
        public string? PickFile(string filter = "All files|*.*")
        {
            try
            {
                using var dlg = new OpenFileDialog();
                dlg.Filter = filter;
                var res = dlg.ShowDialog();
                if (res == DialogResult.OK)
                    return dlg.FileName;
            }
            catch { }
            return null;
        }
    }
}
