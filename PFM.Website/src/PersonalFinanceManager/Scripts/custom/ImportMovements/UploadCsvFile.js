function UploadCsvFile(delimiter, model) {
    var selectedFile = $("#csv")[0].files[0];
    var reader = new FileReader();
    reader.onload = function () {

        var headers = [];
        var rows = [];

        var lines = this.result.split('\n');
        var columnHeaders = lines[0].split(delimiter);
        for (var h = 0; h < columnHeaders.length; h++) {
            headers.push(columnHeaders[h].trim());
        }

        var nbCol = headers.length;
        for (var line = 1; line < lines.length; line++) {
            var columns = lines[line].split(delimiter);
            var row = [];
            for (var column = 0; column < columns.length; column++) {
                row.push(columns[column].trim());
            }
            if (nbCol === row.length) {
                rows.push(row);
            }
        }

        var html = "<table class='table table-striped' id='CsvContentTable'>";
        html += "<thead><tr>";
        var i = 1;
        headers.forEach(function (header) {
            html += "<th>";
            html += header;
            html += " <a id='linkIgnore_" + i + "' title='Ignore' onclick='IgnoreColumn(" + i + ")'><i class='fa fa-ban'></i></a>";
            html += " <a id='linkUndoIgnore_" + i + "' title='Undo Ignore' onclick='UndoIgnoreColumn(" + i + ")' style='display:none;'><i class='fa fa-plus'></i></a>";
            html += "<br /><select id='SelectProperty_" + i + "' class='importcontenttype' style='height:30px' onchange='UpdateConfigMapping(" + i + ")'>";
            $.each(model.MovementPropertyDefinitions, function (i, type) {
                html += "<option hasconfig=" + type.HasConfig + ">" + type.PropertyName + "</option>";
            });
            html += "</select>";
            html +=
                " <a id='linkConfigMapping_" + i + "' title='Map' onclick='OpenConfigMapping(" + i + ")' style='display:none' data-val data-values='' ><i class='fa fa-cog' /></a><i class='fa fa-remove' id='ConfigNotDefined_" + i + "'  style='display:none;color:red' title='Not defined'></i><i id='ConfigDefined_" + i + "' class='fa fa-check ConfigDefined' style='color:green;display:none' title='Defined'></i></th>";
            i++;
        });
        html += "</tr></thead><tbody>";
        rows.forEach(function (row) {
            html += "<tr>";
            row.forEach(function (col) {
                html += "<td>" + col + "</td>";
            });
            html += "</tr>";
        });
        html += "</tbody></table>";

        $("#CsvContent").html(html);
    }
    reader.readAsText(selectedFile);
}

