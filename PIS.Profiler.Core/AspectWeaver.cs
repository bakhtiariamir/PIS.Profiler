using System.Collections;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using PIS.Profiler.Core.Aspects;
using PIS.Profiler.Core.Base.Methods;
using PIS.Profiler.Core.Base.Properties;

namespace PIS.Profiler.Core
{
    public class AspectWeaver : DynamicMetaObject
    {
        readonly Type _objType;

        public AspectWeaver(Expression expression, object obj)
            : base(expression, BindingRestrictions.Empty, obj)
        {
            _objType = obj.GetType();
        }

        public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
        {
            var metaObj = base.BindInvokeMember(binder, args);

            var argsTypes = GetMethodArgsTypes(metaObj);
            var method = _objType.GetMethod(binder.Name,
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                argsTypes,
                null);

            if (method != null && method.IsDefined(typeof(IMethodAspect), false))
            {
                var aspects = RetrieveAspects(method);

                if (method.IsDefined(typeof(OnMethodBoundaryAspect), false))
                {
                    metaObj = new BoundaryAspectGenerator(Value, metaObj, aspects.Cast<OnMethodBoundaryAspect>(), method, args, argsTypes).Generate();
                }
                else
                {
                    metaObj = new InterceptionAspectGenerator(Value, metaObj.Restrictions, aspects.Cast<MethodInterceptionAspect>(), method, args, argsTypes).Generate();
                }
            }
            return metaObj;
        }

        public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            var metaObj = base.BindGetMember(binder);

            var property = _objType.GetProperty(binder.Name,
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (property != null && property.IsDefined(typeof(IAspect), false))
            {
                var aspects = RetrieveAspects(property);
                metaObj = new GetterGenerator(Value, metaObj.Restrictions, aspects, property).Generate();
            }
            return metaObj;
        }

        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        {
            var metaObj = base.BindSetMember(binder, value);

            var property = _objType.GetProperty(binder.Name,
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (property != null && property.IsDefined(typeof(IAspect), false))
            {
                var aspects = RetrieveAspects(property);
                metaObj = new SetterGenerator(Value, metaObj, aspects, property, value).Generate();
            }
            return metaObj;
        }

        IEnumerable RetrieveAspects(MemberInfo member)
        {
            var aspects = new SortedList<int, object>(new InvertedComparer());
            foreach (Aspect aspect in member.GetCustomAttributes(typeof(Aspect), false))
            {
                var instance = Activator.CreateInstance(aspect.GetType());
                aspects.Add(aspect.AspectPriority, instance);
            }
            return aspects.Values;
        }

        Type[] GetMethodArgsTypes(DynamicMetaObject metaObj)
        {
            MethodCallExpression callExpr = RetrieveMethodCall(metaObj);
            return (callExpr).Arguments.Select(arg =>
            {
                if (arg.NodeType != ExpressionType.Parameter) return arg.Type;

                return ((ParameterExpression)arg).IsByRef ? arg.Type.MakeByRefType() : arg.Type;
            }).ToArray();
        }

        MethodCallExpression RetrieveMethodCall(DynamicMetaObject metaObj)
        {
            Expression callExpr = null;

            while (callExpr == null || callExpr.NodeType != ExpressionType.Call)
            {
                switch (metaObj.Expression.NodeType)
                {
                    case ExpressionType.Call: return (MethodCallExpression)metaObj.Expression;

                    case ExpressionType.Block:
                        var block = (BlockExpression)metaObj.Expression;
                        return (MethodCallExpression)block.Expressions.First(expr => expr.NodeType == ExpressionType.Call);

                    case ExpressionType.Convert: callExpr = ((UnaryExpression)metaObj.Expression).Operand; break;

                    default: throw new NotImplementedException();
                }
            }
            return (MethodCallExpression)callExpr;
        }

        internal class InvertedComparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return y.CompareTo(x);
            }
        }
    }
}
