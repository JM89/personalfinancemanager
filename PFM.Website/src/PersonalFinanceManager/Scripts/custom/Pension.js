function initializeForm() {
    $("#StartDate").datepicker({ dateFormat: "dd/mm/yy" });

    $("#EndDate").datepicker({ dateFormat: "dd/mm/yy" });

    $.validator.methods.date = function (value, element) {
        Globalize.culture("en-UK");
        return this.optional(element) || Globalize.parseDate(value) !== null;
    }
}
