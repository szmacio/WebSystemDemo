using System.Linq;
using System.Web.Mvc;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Models.Filters;
using Newtonsoft.Json;

namespace JuCheap.Web
{
    /// <summary>
    /// 模型绑定配置
    /// </summary>
    public class ModelBinderConfig
    {
        public static void RegisterModleBinders(ModelBinderDictionary container)
        {
            container.Add(typeof (AdvanceFilter), new FilterModelBinder());
        }
    }

    /// <summary>
    ///  高级查询模型绑定者
    /// </summary>
    public class FilterModelBinder : IModelBinder
    {
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            var filters = request["filters"];
            var advanceFilter = filters.IsNotBlank()
                ? JsonConvert.DeserializeObject<AdvanceFilter>(filters)
                : new AdvanceFilter();
            if (advanceFilter.Rules.AnyOne())
            {
                advanceFilter.Rules =
                    advanceFilter.Rules.Where(r => r.FieldName.IsNotBlank() && r.Data.IsNotBlank()).ToList();
            }
            advanceFilter.page = request["page"].ToIntWithDefaultValue(1);
            advanceFilter.rows = request["rows"].ToIntWithDefaultValue(15);
            advanceFilter.sidx = request["sidx"];
            advanceFilter.sord = request["sord"];
            advanceFilter.keywords = request["keywords"];

            return advanceFilter;
        }
    }
}