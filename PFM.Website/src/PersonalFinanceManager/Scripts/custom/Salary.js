function initializeForm() {
    $("#StartDate").datepicker({ dateFormat: "dd/mm/yy" });

    $("#EndDate").datepicker({ dateFormat: "dd/mm/yy" });

    $.validator.methods.date = function (value, element) {
        Globalize.culture("en-UK");
        return this.optional(element) || Globalize.parseDate(value) !== null;
    }
}

function addDeduction() {
    var nbItem = parseInt($("#NbItems").val());
    var rowTemplate = $("#rowTemplate").html();
    var row = rowTemplate.split("${index}").join(nbItem);
    row = row.replace("${descriptionValue}", "");
    row = row.replace("${amountValue}", "");
    row = row.replace("${idValue}", "0");
    $("#tblSalaryDeductions tbody").append(row);
    $("#NbItems").val(nbItem + 1);
}

function removeDeduction(index) {

    // Remove the item
    $("#row-" + index).remove();

    // Recount
    var nbItem = parseInt($("#NbItems").val());
    $("#NbItems").val(nbItem - 1);

    // Save the salary deductions
    var savedSalaryDeductions = [];
    $("#tblSalaryDeductions tbody tr").each(function() {
        var inputs = $(this).find("input");
        var id = inputs[0].value;
        var description = inputs[1].value;
        var amount = inputs[2].value;
        savedSalaryDeductions.push({
            Id: id,
            Description: description,
            Amount: amount
        });
    });

    $("#tblSalaryDeductions tbody tr").remove();

    // Resetting all indexes
    var i = 0;
    var rowTemplate = $("#rowTemplate").html();
    $.each(savedSalaryDeductions, function (index, value) {
        var row = rowTemplate.split("${index}").join(i);
        row = row.replace("${descriptionValue}", value.Description);
        row = row.replace("${amountValue}", value.Amount);
        row = row.replace("${idValue}", value.Id);
        $("#tblSalaryDeductions tbody").append(row);
        i++;
    });
}