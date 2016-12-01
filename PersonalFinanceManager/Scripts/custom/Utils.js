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

function strContains(str, sstr)
{
    return str.indexOf(sstr) !== -1;
}

function getRGBA(hexa, a)
{
    var patt = /^([\da-fA-F]{2})([\da-fA-F]{2})([\da-fA-F]{2})$/;
    var matches = patt.exec(hexa);
    var rgba = "rgba(" + parseInt(matches[1], 16) + "," + parseInt(matches[2], 16) + "," + parseInt(matches[3], 16) + ", " + a + ")";
    return rgba;
}