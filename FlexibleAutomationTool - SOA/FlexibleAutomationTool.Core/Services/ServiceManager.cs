using System;
using System.Collections.Concurrent;

namespace FlexibleAutomationTool.Core.Services
{
    public class ServiceManager
    {
        private readonly ConcurrentDictionary<string, object> _services = new();

        public void Register<T>(string name, T svc) => _services[name] = svc!;
        public T? Get<T>(string name) => _services.TryGetValue(name, out var s) && s is T t ? t : default;
    }
}
