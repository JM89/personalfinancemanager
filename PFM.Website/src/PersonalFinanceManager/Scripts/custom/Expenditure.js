function showHideOptions()
{
    var selectedPaymentMethodId = $("#paymentMethodId").val();

    if (selectedPaymentMethodId) {
        var selectedPaymentMethodHasBeenAlreadyDebitedOption = $("#HasBeenAlreadyDebitedOption_" + selectedPaymentMethodId).val();

        if (selectedPaymentMethodHasBeenAlreadyDebitedOption == "True") {
            $("#HasBeenAlreadyDebitedOptionalField").show();
        }
        else {
            $("#HasBeenAlreadyDebitedOptionalField").hide();
        }

        var selectedPaymentMethodHasWithdrawOption = $("#HasAtmWithdrawOption_" + selectedPaymentMethodId).val();

        if (selectedPaymentMethodHasWithdrawOption == "True") {
            $("#HasAtmWithdrawOptionalField").show();
        }
        else {
            $("#HasAtmWithdrawOptionalField").hide();
        }

        var selectedPaymentMethodHasInternalAccountOption = $("#HasInternalAccountOption_" + selectedPaymentMethodId).val();

        if (selectedPaymentMethodHasInternalAccountOption == "True") {
            $("#HasInternalAccountOptionalField").show();
        }
        else {
            $("#HasInternalAccountOptionalField").hide();
        }
    }
    else {
        $("#HasBeenAlreadyDebitedOptionalField").hide();
        $("#HasInternalAccountOptionalField").hide();
        $("#HasAtmWithdrawOptionalField").hide();
    }
}

function stayHere() {
    $("#StayHere").val("true");
}

function returnToList() {
    $("#StayHere").val("false");
}