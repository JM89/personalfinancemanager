using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalFinanceManager.Helpers
{
    public static class ModelStateJsonConvertor
    {
        public static List<JsonError> Convert(ModelStateDictionary modelState)
        {
            var jsonErrors = new List<JsonError>();
            foreach (var modelStateKey in modelState)
            {
                var errors = new List<string>();
                foreach(var error in modelStateKey.Value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
                if (errors.Any())
                {
                    var jsonError = new JsonError()
                    {
                        Field = modelStateKey.Key,
                        ErrorMessages = errors
                    };
                    jsonErrors.Add(jsonError);
                }
            }
            return jsonErrors;
        }
    }
}