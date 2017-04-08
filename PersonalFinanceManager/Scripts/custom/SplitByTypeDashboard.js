function updateLineChartActualVsExpectedExpensesOver12Months(detailedExpensesOver12Months) {
    $("#actualVsExpectedExpensesOver12months-title").text("Actual Vs Expected Expenses - Over 12 Months");

    var data = {
        labels: [],
        datasets: [
            {
                label: "Expected",
                fillColor: 'rgba(220, 220, 220,0.5)',
                strokeColor: "rgba(220, 220, 220,0.7)",
                pointColor: "rgba(220, 220, 220,1)",
                pointStrokeColor: "#fff",
                data: []
            },
            {
                label: "Actual",
                fillColor: 'rgba(248, 172, 89, 0.5)',
                strokeColor: "rgba(248, 172, 89, 0.7)",
                pointColor: "rgba(248, 172, 89, 1)",
                pointStrokeColor: "#fff",
                data: []
            }
        ]
    };

    $.each(detailedExpensesOver12Months, function (index, value) {
        data.labels.push(index);
        data.datasets[0].data.push(value.ExpenseExpectedValue);
        data.datasets[1].data.push(value.ExpenseValue);
    });

    var canvas = document.getElementById("actualVsExpectedExpensesOver12months-canvas");
    var ctx = canvas.getContext("2d");
    var chart = new Chart(ctx).Line(data);
}

function updateBarChartIncomesOutcomesOver6Months(detailedMovementsOver6Months) {
    $("#incomesOutcomesOver6months-title").text("Incomes Vs Outcomes Vs Savings - Over 6 Months");

    var data = {
        labels: [],
        datasets: [
            {
                fillColor: "rgba(26, 179, 148, 0.5)",
                strokeColor: "rgba(26, 179, 148, 0.8)",
                highlightFill: "rgba(26, 179, 148, 0.75)",
                highlightStroke: "rgba(26, 179, 148, 1)",
                data: []
            },
            {
                fillColor: "rgba(248, 172, 89, 0.5)",
                strokeColor: "rgba(248, 172, 89, 0.8)",
                highlightFill: "rgba(248, 172, 89, 0.75)",
                highlightStroke: "rgba(248, 172, 89, 1)",
                data: []
            },
            {
                fillColor: "rgba(237, 85, 101, 0.5)",
                strokeColor: "rgba(237, 85, 101, 0.8)",
                highlightFill: "rgba(237, 85, 101, 0.75)",
                highlightStroke: "rgba(237, 85, 101, 1)",
                data: []
            }
        ]
    };

    $.each(detailedMovementsOver6Months, function (index, value) {
        data.labels.push(index);
        data.datasets[0].data.push(value.IncomeValue);
        data.datasets[1].data.push(value.ExpenseValue);
        data.datasets[2].data.push(value.SavingValue);
    });

    var canvas = document.getElementById("incomesOutcomesOver6months-canvas");
    var ctx = canvas.getContext("2d");
    var chart = new Chart(ctx).Bar(data);
}

function updatePieChartExpensesOver12Months(expenses, totalSum, currencySymbol) {
    $("#expensesOver12months-title").text("Expenses By Category - Over 12 Months");

    var data = [];
    $.each(expenses, function (index, exp) {
        data.push({
                label: exp.CategoryName,
                value: exp.CostOver12Month,
                color: "#" + exp.CategoryColor
            });
        });

    var options = {
        tooltipFontSize: 10,
        tooltipTemplate: " <%if (label) {%><%= label %>: <%}%> <%= parseFloat(Math.round((value / " + totalSum + ") * 100)).toFixed(0) %>% (" + currencySymbol + "<%= parseFloat(value).toFixed(2) %>)"
    }

    var canvas = document.getElementById("expensesOver12months-canvas"); 
    var ctx = canvas.getContext('2d');
    var pieChart = new Chart(ctx).Pie(data, options);

    canvas.onclick = function () {
        if (pieChart.activeElements[0]) {
            var categoryNames = $("#categoryNames").data('categorynames');
            var selectedCategoryName = pieChart.activeElements[0].label;
            var table = $('#split-table').DataTable();
            table.page.jumpToData(selectedCategoryName, 1);
            var categoryId = categoryNames[selectedCategoryName];
            var category = $("#row-" + categoryId).data("category");
            if (category) {
                $("#selectedCategoryId").val(category.CategoryId);
                showExpenditureTypeOverTime(category);
            }
        }
    };
}

function showExpenditureTypeOverTime(category) {

    $("tr[id^='row-']").css('font-weight', 'normal');
    $("tr[id^='row-']").css('color', 'lightgrey');
    $("tr[id='row-" + category.CategoryId + "']").css('font-weight', 'bolder');
    $("tr[id='row-" + category.CategoryId + "']").css('color', 'black');

    $("#typeOverTimeDiv").hide();
    $("#detailExpensesContainer").hide();
    cleanGraph("typeOverTime", 600, 300);

    var data = {
        labels: [],
        datasets: [
            //{
            //    fillColor: getRGBA("D3D3D3", 0.5),
            //    strokeColor: getRGBA("D3D3D3", 0.8),
            //    highlightFill: getRGBA("D3D3D3", 0.75),
            //    highlightStroke: getRGBA("D3D3D3", 1),
            //    data: []
            //},
            {
                fillColor: getRGBA(category.CategoryColor, 0.5),
                strokeColor: getRGBA(category.CategoryColor, 0.8),
                highlightFill: getRGBA(category.CategoryColor, 0.75),
                highlightStroke: getRGBA(category.CategoryColor, 1),
                data: []
            }
        ]
    };

    $.each(category.ExpensesByMonth, function (index, value) {
        data.labels.push(index);
        //data.datasets[0].data.push(value.TotalExpenses);
        data.datasets[0].data.push(value.CategoryExpenses);
    });

    $("#typeOverTimeDiv").show();

    var canvas = document.getElementById("typeOverTime");
    var ctx = canvas.getContext("2d");
    var typeOverTimeChart = new Chart(ctx).Bar(data);

    canvas.onclick = function (evt) {
        if (typeOverTimeChart.activeElements[0]) {
            var month = typeOverTimeChart.activeElements[0].label;
            var categoryId = $("#selectedCategoryId").val();
            var category = $("#row-" + categoryId).data("category");
            showDetailExpenses(category, month);
        }
    };

    scrollToAnchor("typeOverTimeAnchor");

    $("#typeOverTime-title").text("Expenses for '" + category.CategoryName + "' over last 12 months");
}

function cleanGraph(canvasName, width, height) {
    $('#' + canvasName).remove();
    $('#' + canvasName + "-container").append('<canvas id="' + canvasName + '" width="' + width + '" height="' + height + '"><canvas>');
}

function scrollToAnchor(aid) {
    var aTag = $("a[name='" + aid + "']");
    $('html,body').animate({ scrollTop: aTag.offset().top }, 'slow');
}

function showDetailExpenses(category, month) {

    if ($.fn.DataTable.isDataTable('#detailExpensesMonthly-table')) {
        var table = $('#detailExpensesMonthly-table').DataTable();
        table.destroy();
    }
    $("#detailExpensesMonthly-table tbody").empty();

    $.each(category.Expenses[month], function (index, exp) {
        var rowTemplate = $("#rowTemplate").html();
        var row = rowTemplate.replace("${DisplayedDateExpenditure}", exp.DisplayedDateExpenditure);
        row = row.replace("${DisplayedCost}", exp.DisplayedCost);
        row = row.replace("${Description}", exp.Description);
        $("#detailExpensesMonthly-table tbody").append(row);
    });

    $('#detailExpensesMonthly-table').DataTable({
        "iDisplayLength": 6,
        "bLengthChange": false,
        "bFilter": false
    });

    var title = "Details for '" + category.CategoryName + "' for '" + month + "'";
    $("#detailExpensesMonthly-title").text(title);
    $("#detailExpensesContainer").show();
}

function closeTypeOverTimeDiv() {
    $("#typeOverTimeDiv").hide();
    $("tr[id^='row-']").css('font-weight', 'normal');
    $("tr[id^='row-']").css('color', 'black');
    $("#detailExpensesContainer").hide();
}

function closeDetailExpensesContainer() {
    $("#detailExpensesContainer").hide();
}