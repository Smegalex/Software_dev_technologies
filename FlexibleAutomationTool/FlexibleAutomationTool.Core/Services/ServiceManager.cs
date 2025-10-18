using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexibleAutomationTool.Core.Services
{
    public class ServiceManager
    {
        private readonly Dictionary<string, object> _services = new();
        public void Register(string name, object svc) { _services[name] = svc; }
        public object? Get(string name) => _services.TryGetValue(name, out var s) ? s : null;
    }
}
