using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using PersonalFinanceManager.Models.Helpers;
using PersonalFinanceManager.Utils.Utils;

namespace PersonalFinanceManager.Helpers
{
    public static class MvcHtmlExtensions
    {
        private const string FieldEditorTemplate= "<div class=\"form-group\">{0}<div class=\"col-lg-10\">{1}{2}</div></div>";


        private static MvcHtmlString FieldEditor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string defaultValue)
        {
            var label = htmlHelper.LabelFor(expression, new { @class = "control-label col-lg-2" }).ToString();

            var editor = (defaultValue == null ? 
                htmlHelper.EditorFor(expression, new {htmlAttributes = new {@class = "form-control"}}) : 
                htmlHelper.EditorFor(expression, new { htmlAttributes = new { Value = defaultValue, @class = "form-control" } })).ToString();

            var validation = htmlHelper.ValidationMessageFor(expression, "", new { @class = "text-danger" });
            var pValidation = validation?.ToString() ?? "";

            var field = string.Format(FieldEditorTemplate, label, editor, pValidation);

            return new MvcHtmlString(field);
        }

        public static MvcHtmlString FieldEditor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return FieldEditor(htmlHelper, expression, null);
        }

        public static MvcHtmlString ListEditor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, IList<SelectListItem> list)
        {
            var label = htmlHelper.LabelFor(expression, new { @class = "control-label col-lg-2" }).ToString();

            var editor = htmlHelper.DropDownListFor(expression, list, "", new { @class = "form-control" }).ToString();

            var validation = htmlHelper.ValidationMessageFor(expression, "", new { @class = "text-danger" });
            var pValidation = validation?.ToString() ?? "";

            var field = string.Format(FieldEditorTemplate, label, editor, pValidation);

            return new MvcHtmlString(field);
        }

        public static MvcHtmlString DateFieldEditor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var method = expression.Compile();
            var value = method(htmlHelper.ViewData.Model);

            var sDate = "";
            if (value != null)
            {
                if (value is DateTime)
                {
                    var date = Convert.ToDateTime(value);
                    sDate = DateTimeFormatHelper.GetDisplayDateValue(date);
                }
            }
            else
            {
                sDate = DateTimeFormatHelper.GetDisplayDateValue(null);
            }

            return FieldEditor(htmlHelper, expression, sDate);
        }
    }
}