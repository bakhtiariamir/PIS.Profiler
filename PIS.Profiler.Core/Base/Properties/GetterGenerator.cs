using System.Collections;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using PIS.Profiler.Core.Aspects;

namespace PIS.Profiler.Core.Base.Properties
{
    internal class GetterGenerator
    {
        private readonly BindingRestrictions _rule;
        private readonly List<Expression> _aspects;
        private readonly PropertyInterceptionArgs _args;

        public GetterGenerator(object instance, BindingRestrictions rule, IEnumerable aspects, PropertyInfo property) 
        {
            _rule = rule;
            _args = new PropertyInterceptionArgs(instance, property, null);
            _aspects = GenerateAspectCalls(aspects, _args);
        }

        public DynamicMetaObject Generate()
        {
            ParameterExpression retValue = Expression.Parameter(typeof(object));

            var retValueExpr = Expression.Assign(retValue,
                Expression.Call(Expression.Constant(_args),
                typeof (PropertyInterceptionArgs).GetProperty("Value").GetGetMethod()));

            Expression getter = Expression.Block(_aspects.First(), retValueExpr);
            for (int i = 1; i < _aspects.Count; i++)
            {
                getter = Expression.Block(
                new[] { _aspects[i], getter });
            }
            return new DynamicMetaObject(Expression.Block(new[] { retValue }, getter), _rule);
        }

        private List<Expression> GenerateAspectCalls(IEnumerable aspects, LocationInterceptionArgs args)
        {
            var aspectCalls = new List<Expression>();
            foreach (var aspect in aspects)
            {
                aspectCalls.Add(
                    Expression.Call(Expression.Constant(aspect), typeof(LocationInterceptionAspect).GetMethod("OnGetValue"),
                    Expression.Constant(args)));
            }
            return aspectCalls;
        }
    }
}
