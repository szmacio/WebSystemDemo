using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Models.Enum;
using Newtonsoft.Json;

namespace JuCheap.Models.Filters
{
    /// <summary>
    /// 高级条件过滤器
    /// </summary>
    public class AdvanceFilter : BaseFilter
    {
        /// <summary>
        /// 连接符号
        /// </summary>
        public GroupOperator GroupOperator { get; set; }

        /// <summary>
        /// 规则
        /// </summary>
        public IList<RuleFilter> Rules { get; set; }
    }

    /// <summary>
    /// 规则过滤器
    /// </summary>
    public class RuleFilter
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 连接符号
        /// </summary>
        [JsonProperty("OperatorName")]
        public string Operater { get; set; }

        /// <summary>
        /// 查询值
        /// </summary>
        public string Data { get; set; }
    }



    /// <summary>
    /// 扩展
    /// </summary>
    public static class FilterExtention
    {
        /// <summary>
        /// 根据条件数据动态生成或连接条件
        /// </summary>
        /// <typeparam name="TSource">集合项类型</typeparam>
        /// <returns></returns>
        public static Expression<Func<TSource, bool>> ToExpression<TSource>(this AdvanceFilter filter)
        {
            if (!filter.Rules.AnyOne()) return null;

            var type = typeof(TSource);
            Expression expressionReturn = null;
            var expressionParam = Expression.Parameter(type, "p");
            foreach (var rule in filter.Rules)
            {
                Expression temp;
                var left = Expression.Property(expressionParam, type.GetProperty(rule.FieldName));
                switch (rule.Operater)
                {
                    case "ne":
                        temp = Expression.Equal(left, Expression.Constant(rule.Data));
                        break;
                    case "bw":
                        temp = Expression.GreaterThanOrEqual(left, Expression.Constant(rule.Data));
                        break;
                    case "bn":
                        temp = Expression.LessThan(left, Expression.Constant(rule.Data));
                        break;
                    case "ew":
                        temp = Expression.LessThanOrEqual(left, Expression.Constant(rule.Data));
                        break;
                    case "en":
                        temp = Expression.GreaterThan(left, Expression.Constant(rule.Data));
                        break;
                    case "cn":
                        temp = Expression.Call(left, typeof(string).GetMethod("Contains"), Expression.Constant(rule.Data, typeof(string)));
                        break;
                    case "nc":
                        temp = Expression.Not(Expression.Call(left, typeof(string).GetMethod("Contains"), Expression.Constant(rule.Data, typeof(string))));
                        break;
                    case "in":
                        temp = Expression.Call(left, typeof(string).GetMethod("Contains"), Expression.Constant(rule.Data, typeof(string)));
                        break;
                    case "ni":
                        temp = Expression.Not(Expression.Call(left, typeof(string).GetMethod("Contains"), Expression.Constant(rule.Data, typeof(string))));
                        break;
                    case "nu":
                        temp = Expression.Equal(left, Expression.Constant(rule.Data));
                        break;
                    default:
                        temp = Expression.Equal(left, Expression.Constant(rule.Data));
                        break;
                }
                if (expressionReturn == null)
                    expressionReturn = temp;
                else
                {
                    expressionReturn = filter.GroupOperator == GroupOperator.And
                        ? Expression.And(expressionReturn, temp)
                        : Expression.Or(expressionReturn, temp);
                }
            }

            return expressionReturn == null
                ? null
                : Expression.Lambda<Func<TSource, bool>>(expressionReturn, expressionParam);
        }
    }
}
