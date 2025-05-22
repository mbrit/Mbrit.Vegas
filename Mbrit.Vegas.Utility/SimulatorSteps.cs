using BootFX.Common;
using BootFX.Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class SimulatorSteps<T, TValue> : ISimulatorStep
    {
        private IEnumerable<TValue> Values { get; }
        private Expression<Func<T, TValue>> Expression { get; }

        internal SimulatorSteps(Expression<Func<T, TValue>> expression, TValue from, TValue to, TValue step)
        {
            this.Expression = expression;
            var useFrom = ConversionHelper.ToDecimal(from);
            var useTo = ConversionHelper.ToDecimal(to);
            var useStep = ConversionHelper.ToDecimal(step);

            var values = new List<TValue>();
            for (var value = useFrom; value <= useTo; value += useStep)
                values.Add(ConversionHelper.ChangeType<TValue>(value));

            this.Values = values;
        }

        internal void SetValue(T instance, object value)
        {
            var memberExpression = (MemberExpression)this.Expression.Body;
            var property = (System.Reflection.PropertyInfo)memberExpression.Member;
            property.SetValue(instance, value);
        }

        public IEnumerator<object> GetEnumerator() => this.Values.Cast<object>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public PropertyInfo GetProperty() => this.Expression.GetPropertyInfo();
    }
}
