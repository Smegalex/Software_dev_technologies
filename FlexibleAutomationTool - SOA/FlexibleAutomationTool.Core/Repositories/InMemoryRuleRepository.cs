using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlexibleAutomationTool.Core.Models;

namespace FlexibleAutomationTool.Core.Repositories
{
    public class InMemoryRuleRepository : IRuleRepository
    {
        private readonly List<Rule> _rules = new();
        private int _nextId = 1;

        public IEnumerable<Rule> GetAll() => _rules.ToList();

        public void Add(Rule rule)
        {
            rule.Id = _nextId++;
            _rules.Add(rule);
        }

        public void Update(Rule rule)
        {
            var idx = _rules.FindIndex(r => r.Id == rule.Id);
            if (idx >= 0) _rules[idx] = rule;
        }

        public void Delete(int id)
        {
            var r = _rules.FirstOrDefault(x => x.Id == id);
            if (r != null) _rules.Remove(r);
        }
    }
}
