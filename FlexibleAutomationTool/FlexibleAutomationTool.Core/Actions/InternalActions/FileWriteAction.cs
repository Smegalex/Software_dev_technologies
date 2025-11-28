using System;
using System.IO;

namespace FlexibleAutomationTool.Core.Actions.InternalActions
{
    public class FileWriteAction : ActionBase
    {
        public string FilePath { get; set; } = "";
        public string Content { get; set; } = "";

        public override void Execute()
        {
            try
            {
                File.WriteAllText(FilePath, Content);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to write file '{FilePath}': {ex.Message}", ex);
            }
        }

        public override bool Validate() => !string.IsNullOrWhiteSpace(FilePath);
    }
}
