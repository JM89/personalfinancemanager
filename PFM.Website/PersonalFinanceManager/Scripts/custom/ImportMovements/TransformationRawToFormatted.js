function FormatDescription(value) {
    return value;
}

function FormatDate(value, parameters) {
    try {
        //parameters.DateFormat
        var parts = value.split("/");
        return new Date(parts[2], parts[1] - 1, parts[0]);
    } catch (err) {
        return null;
    }
}

function FormatCost(value) {
    try {
        return Math.abs(parseFloat(value).toFixed(2));
    } catch (err) {
        return null;
    }
}

function FormatPaymentMethod(value, formatterPaymentMethods) {

    var paymentMethodId = null;
    var paymentMethod = null;
    var importType = null;

    $.each(formatterPaymentMethods, function (ind, el) {
        if (value == el.Value) {
            paymentMethodId = el.PaymentMethodId;
            paymentMethod = el.PaymentMethod;
            importType = el.ImportType;
        }
    });

    return {
        PaymentMethodId: paymentMethodId,
        PaymentMethod: paymentMethod,
        ImportType: importType
    };
}

function ReturnFormattedRows(formattingParameters, model, formatterPaymentMethods) {
    var mappings = [];
    $("#CsvContentTable thead tr th").each(function (idx, h) {
        var isDisabled = $(h).hasClass("disabled");
        if (!isDisabled) {
            mappings[idx] = $(h).find(".importcontenttype").val();
        } else {
            mappings[idx] = null;
        }
    });
    
    var rows = []; var row = {}; var column = 0;
    $("#CsvContentTable tbody tr").each(function (idxtr, tr) {
        row = {};
        column = 0;
        $(tr).find("td").each(function (idxth, th) {
            if (mappings[column] !== null) {
                var value = $(th)[0].innerHTML;
                switch (mappings[column]) {
                    case "Date":
                        row.Date = FormatDate(value, formattingParameters["Date"]);
                        row.RawDate = value;
                        break;
                    case "Description":
                        row.Description = FormatDescription(value);
                        row.RawDescription = value;
                        break;
                    case "Cost":
                        row.Cost = FormatCost(value);
                        row.RawCost = value;
                        break;
                    case "Payment Method":
                        var transformedPaymentMethod = FormatPaymentMethod(value, formatterPaymentMethods);
                        row.PaymentMethod = transformedPaymentMethod.PaymentMethod;
                        row.PaymentMethodId = transformedPaymentMethod.PaymentMethodId;
                        row.RawPaymentMethod = value;
                        row.ImportType = transformedPaymentMethod.ImportType;
                        break;
                    default:
                }
            }
            column++;
        });
        rows.push(row);
    });
    return rows;
}

function PopulateFormattedTable(rows, model) {

    var currency = model.AccountCurrencySymbol;
    var dateFormat = "DD/MM/YYYY";
    var lastMovementRegistered = FormatDate(model.DisplayedLastMovementRegistered);

    $.each(rows,
        function (idx, row) {
            var htmlrow = "";
            htmlrow += "<tr>";
            htmlrow += "<td>";
            if (lastMovementRegistered != null || row.Date > lastMovementRegistered) {
                htmlrow += "<input type='checkbox' checked class='cbxInclMvts' />";
            } else {
                htmlrow += "<input type='checkbox' class='cbxInclMvts' />";
            }
            htmlrow += "</td>";
            htmlrow += "<td>" + row.ImportType + "</td>";
            htmlrow += "<td>" + row.RawDate + "</td>";

            htmlrow += "<td>";
            if (row.ImportType !== "ATM Withdraws") {
                htmlrow += "<input class='form-control' type='text' value='" + row.Description + "' />";
            } 
            htmlrow += "</td>";
            htmlrow += "<td>" + currency + row.Cost + "</td>";
            htmlrow += "<td><label>" + row.PaymentMethod + "</label><input type='hidden' value='" + row.PaymentMethodId + "' /></td>";
            htmlrow += "<td>";
            if (row.ImportType === "Expenses") {
                htmlrow += "<select class='form-control'>";
                $.each(model.ExpenseTypes, function(i, type) {
                    htmlrow += "<option value='"+ type.Value +"'>" + type.Text + "</option>";
                });
                htmlrow += "</select>";
            }
            htmlrow += "</td>";
            htmlrow += "</tr>";
            $("#FormattedContentBody").append(htmlrow);
        });
}