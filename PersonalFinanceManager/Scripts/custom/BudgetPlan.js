function recompute() {
    var sum = 0;
    $.each($('#budgetPlanForm').serializeArray(), function (i, field) {
        if (field.name.indexOf("ExpectedValue") >= 0) {
            sum += parseFloat(field.value);
        }
    });

    var incomes = parseFloat($('#ExpectedIncomes').val());
    var savings = parseFloat($('#ExpectedSavings').val());
    var total = incomes - sum - savings;
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
        });
}
