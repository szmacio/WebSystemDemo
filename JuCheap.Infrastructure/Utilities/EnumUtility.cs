using JuCheap.Infrastructure.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace JuCheap.Infrastructure
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public static class EnumUtility
    {
        /// <summary>
        /// 枚举转 SelectListItem
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="allText"></param>
        /// <param name="exceptValues"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetValuesWithNullableValue(Type enumType, string allText, params object[] exceptValues)
        {
            yield return new SelectListItem { Text = allText, Value = string.Empty };
            var values = GetValues(enumType, exceptValues);
            foreach (var value in values)
            {
                yield return value;
            }
        }

        /// <summary>
        /// 枚举转 SelectListItem
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="exceptValues"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetValues(Type enumType, params object[] exceptValues)
        {
            var values = Enum.GetValues(enumType);
            return SelectValue(values, exceptValues);
        }

        /// <summary>
        /// 枚举转 SelectListItem
        /// </summary>
        /// <param name="values"></param>
        /// <param name="exceptValues"></param>
        /// <returns></returns>
        private static IEnumerable<SelectListItem> SelectValue(Array values, params object[] exceptValues)
        {
            return from object value in values
                   where exceptValues != null && !exceptValues.Contains(value)
                   select new SelectListItem
                   {
                       Text = value.GetDescriptionForEnum(),
                       Value = Convert.ToInt32(value).ToString()
                   };
        }
    }
}


       

