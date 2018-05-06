function ReturnSummary(model) {

    var currency = model.AccountCurrencySymbol;
    var expenses = [], incomes = [], atmWithdraws = [];

    $("#FormattedContentTable tbody tr").each(function (idxtr, tr) {
        var row = $(tr).find("td");

        if ($(row[0]).find("input").val() === "on") 
        {
            switch (row[1].innerHTML) {
                case "Expenses":
                    expenses.push({
                        Date: row[2].innerHTML,
                        Description: $(row[3]).find("input").val(),
                        Cost:  row[4].innerHTML.substr(1),
                        PaymentMethod: $(row[5]).find("label").text(),
                        PaymentMethodId: $(row[5]).find("input[type='hidden']").val(),
                        ExpenseTypeId: $(row[6]).find("select").val(),
                        ExpenseType: $(row[6]).find("select option:selected").text()
                    });
                    break;
                case "ATM Withdraws":
                    atmWithdraws.push({
                        Date: row[2].innerHTML,
                        Cost: row[4].innerHTML.substr(1)
                    });
                    break;
                case "Incomes":
                    incomes.push({
                        Date: row[2].innerHTML,
                        Description: $(row[3]).find("input").val(),
                        Cost: row[4].innerHTML.substr(1)
                    });
                    break;
            }
        }
           
    });
    return {
        Expenses: expenses,
        Incomes: incomes,
        AtmWithdraws: atmWithdraws
    };
}

function PopulateSummary(summary, model) {

    var currency = model.AccountCurrencySymbol;

    $("#FinalExpensesContentBody").empty();
    $("#FinalIncomesContentBody").empty();
    $("#FinalAtmWithdrawsContentBody").empty();

    var totalExpenses = 0;
    $.each(summary.Expenses,
        function (idx, row) {
            var htmlrow = "";
            htmlrow += "<tr>";
            htmlrow += "<td>" + row.Date + "<input class='form-control' id='model_Expenses[" + idx + "].DateExpenditure' name='Expenses[" + idx + "].DateExpenditure' type='hidden' value='" + row.Date + "'></td>";
            htmlrow += "<td>" + row.Description + "<input class='form-control' id='model_Expenses[" + idx + "].Description' name='Expenses[" + idx + "].Description' type='hidden' value='" + row.Description + "'></td>";
            htmlrow += "<td>" + currency + row.Cost + "<input class='form-control' id='model_Expenses[" + idx + "].Cost' name='Expenses[" + idx + "].Cost' type='hidden' value='" + row.Cost + "'></td>";
            htmlrow += "<td>" + row.PaymentMethod + "<input class='form-control' id='model_Expenses[" + idx + "].PaymentMethodId' name='Expenses[" + idx + "].PaymentMethodId' type='hidden' value='" + row.PaymentMethodId + "'></td>";
            htmlrow += "<td>" + row.ExpenseType + "<input class='form-control' id='model_Expenses[" + idx + "].TypeExpenditureId' name='Expenses[" + idx + "].TypeExpenditureId' type='hidden' value='" + row.ExpenseTypeId + "'></td>";
            htmlrow += "</tr>";
            $("#FinalExpensesContentBody").append(htmlrow);
            totalExpenses += parseInt(row.Cost);
        });

    $("#summaryExpenses").html(currency + totalExpenses.toString())

    var totalIncomes = 0;
    $.each(summary.Incomes,
        function (idx, row) {
            var htmlrow = "";
            htmlrow += "<tr>";
            htmlrow += "<td>" + row.Date + "<input class='form-control' id='model_Incomes[" + idx + "].DateIncome' name='Incomes[" + idx + "].DateIncome' type='hidden' value='" + row.Date + "'></td>";
            htmlrow += "<td>" + row.Description + "<input class='form-control' id='model_Incomes[" + idx + "].Description' name='Incomes[" + idx + "].Description' type='hidden' value='" + row.Description + "'></td>";
            htmlrow += "<td>" + currency + row.Cost + "<input class='form-control' id='model_Incomes[" + idx + "].Cost' name='Incomes[" + idx + "].Cost' type='hidden' value='" + row.Cost + "'></td>";
            htmlrow += "</tr>";
            $("#FinalIncomesContentBody").append(htmlrow);
            totalIncomes += parseInt(row.Cost);
        });

    $("#summaryIncomes").html(currency + totalIncomes.toString())

    var totalAtmWithdraws = 0;
    $.each(summary.AtmWithdraws,
        function (idx, row) {
            var htmlrow = "";
            htmlrow += "<tr>";
            htmlrow += "<td>" + row.Date + "<input class='form-control' id='model_AtmWithdraws[" + idx + "].DateExpenditure' name='AtmWithdraws[" + idx + "].DateExpenditure' type='hidden' value='" + row.Date + "'></td>";
            htmlrow += "<td>ATM " + row.Date + " (Left: " + currency + row.Cost + ")";
            htmlrow += "<td>" + currency + row.Cost + "<input class='form-control' id='model_AtmWithdraws[" + idx + "].InitialAmount' name='AtmWithdraws[" + idx + "].InitialAmount' type='hidden' value='" + row.Cost + "'></td>";
            htmlrow += "</tr>";
            $("#FinalAtmWithdrawsContentBody").append(htmlrow);
            totalAtmWithdraws += parseInt(row.Cost);
        });

    $("#summaryAtmWithdraws").html(currency + totalAtmWithdraws.toString());

    var newBalance = model.AccountCurrentBalance + totalIncomes - (totalExpenses + totalAtmWithdraws);
    $("#summaryNewBalance").html(currency + newBalance.toString());
}