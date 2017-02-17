function showFrequence() {
    var frequenceOptionId = $("#FrequenceOptionId").val();
    switch (frequenceOptionId) {
        case "1":
            $("#Frequence").closest(".form-group").hide();
            break;
        case "2":
            $("#Frequence").closest(".form-group").show();
            break;
        default:
            $("#Frequence").closest(".form-group").hide();
            break;
            
    }
}

function initializeForm() {
    $("#StartDate").datepicker({ dateFormat: "dd/mm/yy" });

    $("#EndDate").datepicker({ dateFormat: "dd/mm/yy" });

    $.validator.methods.date = function (value, element) {
        Globalize.culture("en-UK");
        return this.optional(element) || Globalize.parseDate(value) !== null;
    }

    $("#FrequenceOptionId").change(function () {
        showFrequence();
    });

    showFrequence();
}
