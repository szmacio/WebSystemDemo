using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JuCheap.Infrastructure.Attributes;

namespace JuCheap.Infrastructure.Extentions
{
    /// <summary>
    /// 自定义属性扩展
    /// </summary>
    public static class CustomAttributeExtention
    {
        /// <summary>
        /// 获取指定Attribute的属性的值
        /// </summary>
        /// <typeparam name="TTarget">目标对象类型</typeparam>
        /// <param name="objectTarget">目标对象</param>
        /// <returns></returns>
        public static List<ExportColumn> GetExportAttribute<TTarget>(
            this TTarget objectTarget) where TTarget : class
        {
            var data = new List<ExportColumn>();
            var type = typeof(TTarget);
            var properties =
                type.GetProperties().Where(x => x.GetCustomAttribute<ExportPropertyAttribute>(false) != null).ToList();

            if (!properties.AnyOne()) return data;

            return properties.Select(propertyInfo => propertyInfo.GetCustomAttribute<ExportPropertyAttribute>())
                .Select(attribute => new ExportColumn
                {
                    Name = attribute.Name,
                    Order = attribute.Order
                }).OrderBy(x => x.Order).ToList();
        }

        /// <summary>
        /// 获取含有指定Attribute的属性的值
        /// </summary>
        /// <typeparam name="TTarget">目标对象类型</typeparam>
        /// <param name="objectTarget">目标对象</param>
        /// <returns></returns>
        public static List<ExportDataValue> GetPropertyValues<TTarget>(
            this TTarget objectTarget) where TTarget : class
        {
            var data = new List<ExportDataValue>();
            var type = typeof(TTarget);
            var properties =
                type.GetProperties().Where(x => x.GetCustomAttribute<ExportPropertyAttribute>(false) != null).ToList();

            if (!properties.AnyOne()) return data;

            return properties.OrderBy(x => x.GetCustomAttribute<ExportPropertyAttribute>().Order)
                .Select(propertyInfo => new ExportDataValue
                {
                    Value = propertyInfo.GetValue(objectTarget)!=null? propertyInfo.GetValue(objectTarget).ToString():string.Empty,
                    Type = propertyInfo.PropertyType
                }).ToList();
        }
    }
}
