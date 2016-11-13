function handleJsonValidationErrors(errorMessages)
{
    $.each(errorMessages, function (index, value) {
        var concatErrorMessages = "";
        $.each(value.ErrorMessages, function (index, value) {
            concatErrorMessages += value + "\n";
        });

        var errorLabel = "";
        var indexOpeningBracket = value.Field.indexOf("[");
        if (indexOpeningBracket >= 0) {
            var arrayName = value.Field.substring(0, value.Field.length - indexOpeningBracket - 1);
            var indexClosingBracket = value.Field.indexOf(".");
            var property = value.Field.substring(indexClosingBracket + 1);
            errorLabel = "#" + arrayName + "_" + property + "_ErrorMsg";
        }
        else {
            errorLabel = "#" + value.Field + "_ErrorMsg";
        }

        $(errorLabel).text(concatErrorMessages);
        $(errorLabel).css("display", "block")
    });
}