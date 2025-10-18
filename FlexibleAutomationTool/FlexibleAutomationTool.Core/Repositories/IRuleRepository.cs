using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlexibleAutomationTool.Core.Models;

namespace FlexibleAutomationTool.Core.Repositories
{
    public interface IRuleRepository
    {
        IEnumerable<Rule> GetAll();
        void Add(Rule rule);
        void Update(Rule rule);
        void Delete(int id);
    }
}
