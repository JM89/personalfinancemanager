function recompute() {
    var sum = 0;
    $.each($('#budgetPlanForm').serializeArray(), function (i, field) {
        if (field.name.indexOf("ExpectedValue") >= 0) {
            sum += parseFloat(field.value);
        }
    });

    var income = parseFloat($('#ReferenceMonthTotalIncome').val());
    var total = income - sum;
    $('#TotalExpectedValue').attr("value", sum.toFixed(2));
    $('#Total').attr("value", total.toFixed(2));
    if (total > 200) {
        $('#Total').css("background-color", "rgba(55, 152, 55, 0.5)");
    }
    else {
        $('#Total').css("background-color", "rgba(255,0,0,0.5)");
    }
}

function createBudgetPlan() {

    $.post('Create/', $('#budgetPlanForm').serialize())
        .done(function (data) {
            if (data.Result) {
                window.location = data.RedirectLocation;
            }
            else {
                handleJsonValidationErrors(data.ErrorMessages);
            }
        })
        .fail(function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status);
            console.log(xhr.responseText);
            console.log(thrownError);
        });
}

function editBudgetPlan() {

    var id = parseInt($("#Id").val());

    $.post(id, $('#budgetPlanForm').serialize())
        .done(function (data) {
            if (data.Result) {
                window.location = data.RedirectLocation;
            }
            else {
                handleJsonValidationErrors(data.ErrorMessages);
            }
        })
        .fail(function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status);
            console.log(xhr.responseText);
            console.log(thrownError);
        });
}
