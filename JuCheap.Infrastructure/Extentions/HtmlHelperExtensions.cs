using JuCheap.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;


/// <summary>
/// HTML helper
/// </summary>
public static class HtmlHelperExtensions
{
    public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TProperty>> expression, Type enumType)
    {
        return htmlHelper.DropDownListFor(expression, enumType, null);
    }

    public static MvcHtmlString DropDownListForAll<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TProperty>> expression, Type enumType, string allText)
    {
        return htmlHelper.DropDownListForAll(expression, enumType, allText, null);
    }

    public static MvcHtmlString DropDownListForAll<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TProperty>> expression, Type enumType, string allText, object htmlAttributes,
        params object[] exceptValues)
    {
        var options = EnumUtility.GetValuesWithNullableValue(enumType, allText, exceptValues);

        return htmlHelper.DropDownListFor(expression, options, htmlAttributes);
    }

    public static MvcHtmlString DropDownListAll(this HtmlHelper htmlHelper, string id, Type enumType, string allText,
        object htmlAttributes, params object[] exceptValues)
    {
        var options = EnumUtility.GetValuesWithNullableValue(enumType, allText, exceptValues);

        return htmlHelper.DropDownList(id, options, htmlAttributes);
    }

    public static MvcHtmlString DropDownListAll(this HtmlHelper htmlHelper, string id, Type enumType, string allText,
        IDictionary<string,object> htmlAttributes, params object[] exceptValues)
    {
        var options = EnumUtility.GetValuesWithNullableValue(enumType, allText, exceptValues);

        return htmlHelper.DropDownList(id, options, htmlAttributes);
    }

    public static MvcHtmlString DropDownListAll(this HtmlHelper htmlHelper, string id, Type enumType, string allText,
        object htmlAttributes, string value, params object[] exceptValues)
    {
        var options = EnumUtility.GetValuesWithNullableValue(enumType, allText, exceptValues);

        var list = options.ToList();
        foreach (var item in list)
        {
            if (item.Value == value)
            {
                item.Selected = true;
            }
        }
        return htmlHelper.DropDownList(id, list, htmlAttributes);
    }

    public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TProperty>> expression, Type enumType, object htmlAttributes,
        params object[] exceptValues)
    {
        var options = EnumUtility.GetValues(enumType, exceptValues);

        return htmlHelper.DropDownListFor(expression, options, htmlAttributes);
    }
}

