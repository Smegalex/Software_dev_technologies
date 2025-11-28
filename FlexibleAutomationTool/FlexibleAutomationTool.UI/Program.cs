using FlexibleAutomationTool.Core.Actions.InternalActions;
using FlexibleAutomationTool.Core.Interfaces;
using FlexibleAutomationTool.Core.Repositories;
using FlexibleAutomationTool.Core.Services;
using FlexibleAutomationTool.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace FlexibleAutomationTool.UI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            using var provider = services.BuildServiceProvider();

            ApplicationConfiguration.Initialize();

            // Головне вікно беремо з DI
            var mainForm = provider.GetRequiredService<MainForm>();
            Application.Run(mainForm);
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            // Реєстрація репозиторію та інших core сервісів
            services.AddSingleton<IRuleRepository, InMemoryRuleRepository>();
            services.AddSingleton<Logger>();
            services.AddSingleton<ServiceManager>();
            services.AddSingleton<Scheduler>();
            services.AddSingleton<AutomationEngine>();

            // Реєстрація сервісів UI
            services.AddSingleton<IMessageBoxService, WinFormsMessageBoxService>();

            // Реєстрація Actions
            services.AddTransient<MessageBoxAction>();

            // Реєстрація форм
            services.AddTransient<CreateRuleForm>();
            services.AddTransient<MainForm>();

            // AutomationEventHandler needs SynchronizationContext.Current from the UI thread. Use factory to capture it.
            services.AddTransient<AutomationEventHandler>(sp =>
                new AutomationEventHandler(
                    sp.GetRequiredService<AutomationEngine>(),
                    sp.GetRequiredService<Logger>(),
                    sp.GetRequiredService<IMessageBoxService>(),
                    SynchronizationContext.Current
                )
            );
        }
    }
}