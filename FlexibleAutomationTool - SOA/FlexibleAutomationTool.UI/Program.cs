using FlexibleAutomationTool.Core.Actions.InternalActions;
using FlexibleAutomationTool.Core.Interfaces;
using FlexibleAutomationTool.Core.Repositories;
using FlexibleAutomationTool.Core.Services;
using FlexibleAutomationTool.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using FlexibleAutomationTool.Core.Interpreter;
using FlexibleAutomationTool.Core.Factories;
using FlexibleAutomationTool.Core.Facades;
using FlexibleAutomationTool.Core.Data;

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

            ServiceProvider provider;
            try
            {
                provider = services.BuildServiceProvider();
            }
            catch (Exception ex)
            {
                try
                {
                    MessageBox.Show($"Failed to build services:\n{ex}", "Startup error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch { }
                throw;
            }

            ApplicationConfiguration.Initialize();

            try
            {
                // Resolve main form and run UI inside try/catch to show any startup exceptions
                var mainForm = provider.GetRequiredService<MainForm>();
                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                // Show exception so user can see why UI failed to start
                try
                {
                    MessageBox.Show($"Failed to start UI:\n{ex}", "Startup error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch
                {
                    // fallback to console if MessageBox can't be shown
                    Console.Error.WriteLine(ex.ToString());
                }
                throw;
            }
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FlexibleAutomationTool", "data.db");
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

            // Try to initialize SQLite-based persistence. If it fails, fall back to in-memory repo and logger so UI can start.
            SqliteDataContext? ctx = null;
            IExecutionHistoryRepository? historyRepo = null;
            bool sqliteOk = false;
            try
            {
                ctx = new SqliteDataContext(dbPath);
                // register context and sqlite-backed repositories
                services.AddSingleton(ctx);
                services.AddSingleton<SqliteRuleRepository>();
                services.AddSingleton<IExecutionHistoryRepository, SqliteLogger>(sp => new SqliteLogger(sp.GetRequiredService<SqliteDataContext>()));
                services.AddSingleton<IRuleRepository>(sp => sp.GetRequiredService<SqliteRuleRepository>());
                services.AddSingleton<Logger>(sp => new Logger(sp.GetRequiredService<IExecutionHistoryRepository>()));
                sqliteOk = true;
            }
            catch (Exception ex)
            {
                try
                {
                    MessageBox.Show($"Failed to initialize database, running in memory mode:\n{ex.Message}", "Database error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch { }

                // fallback registrations
                services.AddSingleton<IRuleRepository, InMemoryRuleRepository>();
                services.AddSingleton<Logger>();
            }

            // Always register ServiceManager
            services.AddSingleton<ServiceManager>();

            // Strategy
            services.AddSingleton<FlexibleAutomationTool.Core.Services.ITriggerStrategy, PollingTriggerStrategy>();

            // Scheduler now needs strategy and repository
            services.AddSingleton<Scheduler>(sp => new Scheduler(sp.GetRequiredService<IRuleRepository>(), sp.GetRequiredService<FlexibleAutomationTool.Core.Services.ITriggerStrategy>()));

            // Command dispatcher
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

            // Interpreter
            services.AddSingleton<IInterpreter, SimpleMacroInterpreter>();

            // Реєстрація сервісів UI
            services.AddSingleton<IMessageBoxService, WinFormsMessageBoxService>();

            // Platform factory
            services.AddSingleton<IPlatformFactory, WinFormsPlatformFactory>(sp => new WinFormsPlatformFactory(sp.GetRequiredService<IMessageBoxService>(), sp.GetRequiredService<ServiceManager>()));

            // Engine (now serves as facade) depends on scheduler and other services
            services.AddSingleton<IAutomationFacade>(sp => new AutomationEngine(sp.GetRequiredService<IRuleRepository>(), sp.GetRequiredService<Scheduler>(), sp.GetRequiredService<ServiceManager>(), sp.GetRequiredService<Logger>(), sp.GetRequiredService<IInterpreter>(), sp.GetRequiredService<ICommandDispatcher>(), sp.GetRequiredService<IPlatformFactory>(), sp.GetService<IExecutionHistoryRepository>()));

            // Facade convenience registration
            services.AddSingleton<AutomationEngine>(sp => (AutomationEngine)sp.GetRequiredService<IAutomationFacade>());

            // Реєстрація Actions
            services.AddTransient<MessageBoxAction>();

            // Реєстрація форм
            services.AddTransient<CreateRuleForm>();
            services.AddTransient<MainForm>();

            // AutomationEventHandler needs SynchronizationContext.Current from the UI thread. Use factory to capture it.
            services.AddTransient<AutomationEventHandler>(sp =>
                new AutomationEventHandler(
                    sp.GetRequiredService<IAutomationFacade>(),
                    sp.GetRequiredService<Logger>(),
                    sp.GetRequiredService<IMessageBoxService>(),
                    SynchronizationContext.Current
                )
            );
        }
    }
}