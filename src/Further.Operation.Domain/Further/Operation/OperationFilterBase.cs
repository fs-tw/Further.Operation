using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Volo.Abp.Specifications;

namespace Further.Operation
{
    public abstract class OperationFilterBase<T> : AutoFilterer.Types.FilterBase, ISpecification<T>
    {
        public virtual bool IsSatisfiedBy(T obj)
        {
            return ToExpression().Compile()(obj);
        }
        public virtual Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> defaultExpression = (x) => true;

            var parameter = Expression.Parameter(typeof(T), "x");

            var exp = this.BuildExpression(typeof(T), parameter);
            if (exp == null)
                return defaultExpression;

            if (exp is MemberExpression || exp is ParameterExpression)
                return defaultExpression;

            var lambda = Expression.Lambda<Func<T, bool>>(exp, parameter);

            return lambda;
        }
    }
}
