using System;
using System.Collections.Generic;
using System.Text;

namespace SirmaProject
{
    public class Employee
    {
        private Dictionary<string, DateTime[]> _projects;

        public Employee(string id)
        {
            Id = id;
            _projects = new Dictionary<string, DateTime[]>();
        }
        public string Id { get; private set; }
        public IReadOnlyDictionary<string, DateTime[]> Projects {get => _projects; }

        public void AddProject(string id, DateTime[] duration)
        {
            _projects.Add(id, duration);
        }
    }
}
