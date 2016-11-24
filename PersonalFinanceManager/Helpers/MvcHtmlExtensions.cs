using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace PersonalFinanceManager.Helpers
{
    public static class MvcHtmlExtensions
    {
        private const string FieldEditorTemplate= "<div class=\"form-group\">{0}<div class=\"col-lg-10\">{1}{2}</div></div>";

        public static MvcHtmlString FieldEditor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var sb = new StringBuilder();

            var label = htmlHelper.LabelFor(expression, new { @class = "control-label col-lg-2" }).ToString();

            var editor = htmlHelper.EditorFor(expression, new { htmlAttributes = new { @class = "form-control" } }).ToString();

            var validation = htmlHelper.ValidationMessageFor(expression, "", new { @class = "text-danger" });
            var pValidation = validation != null ? validation.ToString() : "";

            var field = string.Format(FieldEditorTemplate, label, editor, pValidation);

            return new MvcHtmlString(field);
        }
    }
}