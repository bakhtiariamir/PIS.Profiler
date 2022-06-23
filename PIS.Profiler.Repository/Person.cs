using System.Dynamic;
using System.Linq.Expressions;
using PIS.Profiler.Core;
using PIS.Profiler.Core.Action;

namespace PIS.Profiler.Repository
{
    public class Person : IDynamicMetaObjectProvider
    {
        public string Name
        {
            get;
        }

        public string LastName
        {
            get;
        }

        public Person(string name, string lastName)
        {
            Name = name;
            LastName = lastName;
        }
        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new AspectWeaver(parameter, this);
        }

        [ActionProfiler] // apply aspect
        public void Save(Person entity)
        {
            Thread.Sleep(5000);
        }
    }
}