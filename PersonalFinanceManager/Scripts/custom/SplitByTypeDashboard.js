function updatePieChart(chartId, model, totalSum, propertyName, name, description) {

    $("#" + chartId + "-title").text(name);
    $("#" + chartId + "-desc").text(description);

    var data = [];

    $.each(model.SplitByTypes, function (index, type) {
        data.push({
            label: type.ExpenditureTypeName,
            value: type[propertyName],
            color: "#" + type.GraphColor
        });
    });

    var options = {
        tooltipFontSize: 10,
        tooltipTemplate: " <%if (label) {%><%= label %>: <%}%> <%= parseFloat(Math.round((value / " + totalSum + ") * 100)).toFixed(0) %>% (" + model.CurrencySymbol + "<%= parseFloat(value).toFixed(2) %>)"
    }

    var ctx = document.getElementById(chartId).getContext("2d");
    var chart = new Chart(ctx).Pie(data, options);
}

function updateRadarChart(model)
{
    var chartId = "top5";

    $("#" + chartId + "-title").text("Top 5");
    $("#" + chartId + "-desc").text(model.CurrentMonthName);

    var colorExpected = "rgba(128, 128, 128, 0.5)";
    var colorActual = "rgba(38, 94, 217, 0.5)";

    var nbItem = 0;

    var data = {
        labels: [],
        datasets: [
            {
                pointColor: colorActual,
                pointStrokeColor: colorActual,
                fillColor: colorActual,
                fillStrokeColor: colorActual,
                data: []
            },
            {
                pointColor: colorExpected,
                pointStrokeColor: colorExpected,
                fillColor: colorExpected,
                fillStrokeColor: colorExpected,
                data: []
            }
        ]
    }
    $.each(model.SplitByTypes, function (index, type) {
        if (nbItem < 5)
        {
            data.labels.push(type.ExpenditureTypeName);
            data.datasets[0].data.push(type.CurrentMonthCost);
            if (type.ExpectedCost)
            {
                data.datasets[1].data.push(type.ExpectedCost);
            }
        }
        nbItem++;
    });

    var ctx = document.getElementById(chartId).getContext("2d");
    var chart = new Chart(ctx).Radar(data);
}

function showExpenditureTypeOverTime(expenditureTypeId) {

    var url = "/Dashboard/GetSplitByTypeOverLast12Months?expenditureTypeId=" + expenditureTypeId;

    $("tr[id^='row-']").css('font-weight', 'normal');
    $("tr[id^='row-']").css('color', 'lightgrey');
    $("tr[id='row-" + expenditureTypeId + "']").css('font-weight', 'bolder');
    $("tr[id='row-" + expenditureTypeId + "']").css('color', 'black');

    $.get(url)
        .done(function (result) {

            $("#typeOverTimeDiv").hide();
            cleanGraph("typeOverTime", 800, 300);

            var data = {
                labels: [],
                datasets: [
                    {
                        fillColor: getRGBA(result.GraphColor, 0.5),
                        strokeColor: getRGBA(result.GraphColor, 0.8),
                        highlightFill: getRGBA(result.GraphColor, 0.75),
                        highlightStroke: getRGBA(result.GraphColor, 1),
                        data: []
                    }
                ]
            };

            $.each(result.Values, function (index, value) {
                data.labels.push(value.MonthName);
                data.datasets[0].data.push(value.Value);
            });

            $("#typeOverTimeDiv").show();

            var ctx = document.getElementById("typeOverTime").getContext("2d");
            var typeOverTimeChart = new Chart(ctx).Bar(data);

            scrollToAnchor("typeOverTimeAnchor");

            $("#typeOverTime-title").text("Expenditures for '" + result.ExpenditureTypeName + "' over last 12 months");

            $("#typeOverTime-average").text(result.DisplayAverageCost);
            $("#typeOverTime-average").css("color", getRGBA(result.GraphColor, 1));

            $("#typeOverTime-differencecurrentpreviouscost").text(result.DisplayDifferenceCurrentPreviousCost);
            $("#typeOverTime-differencecurrentpreviouscost").css("color", getRGBA(result.GraphColor, 1));
           
        })
        .fail(function (error) {
            console.error("Something is wrong");
        });
}

function cleanGraph(canvasName, width, height) {
    $('#' + canvasName).remove();
    $('#' + canvasName + "-container").append('<canvas id="' + canvasName + '" width="' + width + '" height="' + height + '"><canvas>');
}

function scrollToAnchor(aid) {
    var aTag = $("a[name='" + aid + "']");
    $('html,body').animate({ scrollTop: aTag.offset().top }, 'slow');
}