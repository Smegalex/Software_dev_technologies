using FlexibleAutomationTool.Core.Models;
using System;

namespace FlexibleAutomationTool.Core.Services
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly Logger _logger;

        public CommandDispatcher(Logger logger)
        {
            _logger = logger;
        }

        public void Dispatch(Rule rule)
        {
            try
            {
                rule.Execute();
                _logger.Log(rule.Name, "executed via dispatcher");
            }
            catch (Exception ex)
            {
                _logger.Log(rule.Name, $"dispatch failed: {ex}");
                throw;
            }
        }
    }
}
