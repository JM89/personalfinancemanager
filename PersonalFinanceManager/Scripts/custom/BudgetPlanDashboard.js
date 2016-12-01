function showActualVsPlannedBarChart() {

    var url = "/Dashboard/GetActualVsPlanned";

    $.get(url)
        .done(function (data) {
            console.log("Good call! Here your data:");
            console.log(data);

            var data = {
                labels: data.Labels,
                datasets: [
                    {
                        label: "My First dataset",
                        fillColor: "rgba(38, 94, 217,0.5)",
                        strokeColor: "rgba(38, 94, 217,0.8)",
                        highlightFill: "rgba(38, 94, 217,0.75)",
                        highlightStroke: "rgba(38, 94, 217,1)",
                        data: data.ChartDatasets[0].Values
                    }
                    //,
                    //{
                    //    label: "My Second dataset",
                    //    fillColor: "rgba(217, 59, 38, 0.5)",
                    //    strokeColor: "rgba(217, 59, 38, 0.8)",
                    //    highlightFill: "rgba(217, 59, 38, 0.75)",
                    //    highlightStroke: "rgba(217, 59, 38, 1)",
                    //    data: data.ChartDatasets[1].Values
                    //}
                ]
            };

            var ctx = document.getElementById("bcBudgetExpenditures").getContext("2d");
            var myBarChart = new Chart(ctx).Bar(data);

        })
        .fail(function (error) {
            console.error("Something is wrong");
        });


}

function showTop10ExpenditureTypes()
{
    var url = "/Dashboard/GetTop10ExpenditureTypes";

    $.get(url)
        .done(function (data) {
            console.log("Good call! Here your data:");
            console.log(data);

            var data = {
                labels: data.Labels,
                datasets: [
                    {
                        pointColor: "rgba(38, 94, 217, 0.5)",
                        pointStrokeColor: "rgba(38, 94, 217, 0.5)",
                        fillColor: "rgba(38, 94, 217, 0.5)",
                        fillStrokeColor: "rgba(38, 94, 217, 0.5)",
                        data: data.ChartDatasets[0].Values
                    },
                    {
                        pointColor: "rgba(217, 59, 38, 0.5)",
                        pointStrokeColor: "rgba(217, 59, 38, 0.5)",
                        fillColor: "rgba(217, 59, 38, 0.5)",
                        fillStrokeColor: "rgba(217, 59, 38, 0.5)",
                        data: data.ChartDatasets[1].Values
                    }
                ]
            };

            var ctx = document.getElementById("rTop10ExpenditureTypes").getContext("2d");
            var myRadarChart = new Chart(ctx).Radar(data);
        })
        .fail(function (error) {
            console.error("Something is wrong");
        });


}

