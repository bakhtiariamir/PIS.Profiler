using System.Collections;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using PIS.Profiler.Core.Aspects;

namespace PIS.Profiler.Core.Base.Properties
{
    internal class SetterGenerator
    {
        private readonly BindingRestrictions _rule;
        private readonly List<Expression> _aspects;

        public SetterGenerator(object instance, DynamicMetaObject metaObj, IEnumerable aspects, PropertyInfo property, DynamicMetaObject value)
        {
            _rule = metaObj.Restrictions;
            _aspects = GenerateAspectCalls(aspects, new PropertyInterceptionArgs(instance, property, value.Value));
        }

        public DynamicMetaObject Generate()
        {
            Expression setter = Expression.Block(_aspects.First(), Expression.Default(typeof(object)));
            for (int i = 1; i < _aspects.Count; i++)
            {
                setter = Expression.Block(
                new[]
                {
                    _aspects[i],
                    setter
                });
            }
            return new DynamicMetaObject(setter, _rule);
        }

        private List<Expression> GenerateAspectCalls(IEnumerable aspects, LocationInterceptionArgs args)
        {
            var aspectCalls = new List<Expression>();
            foreach (var aspect in aspects)
            {
                aspectCalls.Add(
                    Expression.Call(Expression.Constant(aspect), typeof(LocationInterceptionAspect).GetMethod("OnSetValue"),
                    Expression.Constant(args)));
            }
            return aspectCalls;
        }
    }
}
