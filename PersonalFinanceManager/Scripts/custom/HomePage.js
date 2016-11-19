function showDebitMovementsOverTime() {

    var url = "/Home/GetDebitMovementsOverTime";

    $.get(url)
        .done(function (data) {

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
                ]
            };

            var ctx = document.getElementById("lDebitMovementsOverTime").getContext("2d");
            var myLineChart = new Chart(ctx).Line(data);
        })
        .fail(function (error) {
            console.error("Something is wrong");
        });
}
