using System;
using System.Linq;
using System.Linq.Expressions;

namespace JuCheap.Infrastructure.Utilities
{
    /// <summary>
    /// 利用表达式给对象赋值
    /// </summary>
    public static class ExpressionCopy<TIn, TOut>
    {
        /// <summary>
        /// 缓存
        /// </summary>
        private static readonly Func<TIn, TOut> MapCache = MapFunc();

        /// <summary>
        /// 反射循环所有的属性, 然后Expression.Bind所有的属性
        /// </summary>
        /// <returns></returns>
        private static Func<TIn, TOut> MapFunc()
        {
            var parameterExpression = Expression.Parameter(typeof(TIn), "p");

            var memberBinds = (from item in typeof(TOut).GetProperties()
                let property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name))
                select Expression.Bind(item, property)).Cast<MemberBinding>().ToArray();
            var memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBinds);
            var lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new [] { parameterExpression });

            return lambda.Compile();
        }

        /// <summary>
        /// 对象转换
        /// </summary>
        /// <param name="tIn"></param>
        /// <returns></returns>
        public static TOut Map(TIn tIn)
        {
            return MapCache(tIn);
        }
    }
}
