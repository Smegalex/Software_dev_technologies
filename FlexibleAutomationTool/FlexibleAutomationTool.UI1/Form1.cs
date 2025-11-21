using FlexibleAutomationTool.Core.Repositories;
using FlexibleAutomationTool.Core.Services;

namespace FlexibleAutomationTool.UI1
{
    public partial class Form1 : Form
    {
        private readonly IRuleRepository _repo;
        private readonly Logger _logger;
        private readonly ServiceManager _svcManager;
        private readonly Scheduler _scheduler;
        private readonly AutomationEngine _engine;

        public Form1()
        {
            InitializeComponent();

            _repo = new InMemoryRuleRepository();
            _logger = new Logger();
            _svcManager = new ServiceManager();
            _scheduler = new Scheduler(_repo);
            _engine = new AutomationEngine(_repo, _scheduler, _svcManager, _logger);

            _engine.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
